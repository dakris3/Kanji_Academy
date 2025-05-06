using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Text;

public class KetikKata : MonoBehaviour
{
    public TMP_Text kanjiText;
    public TMP_InputField inputField;
    public Image kanjiImage;

    [System.Serializable]
    public class KanjiQuestion
    {
        public string kanji;
        public string answer;
        public Sprite image;
    }

    [Header("Question Settings")]
    public KanjiQuestion[] kanjiQuestions;
    public bool shuffleQuestions = true;

    private List<KanjiQuestion> remainingQuestions;
    private KanjiQuestion currentQuestion;

    public Button[] sisi1Buttons;
    public Button[] sisi2Buttons;
    public Button[] sisi3Buttons;

    public Button switch1Button;
    public Button switch2Button;
    public Button switch3Button;

    public Button checkButton;
    public Button deleteButton;

    private int currentSisi = 1;

    [Header("Sound Effects")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;
    private AudioSource audioSource;

    void Start()
    {
        inputField.inputType = TMP_InputField.InputType.Standard;
        inputField.keyboardType = TouchScreenKeyboardType.Default;
        inputField.characterValidation = TMP_InputField.CharacterValidation.None;
        inputField.characterLimit = 10;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource tidak ditemukan. Menambahkan komponen AudioSource.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        ResetQuestions();
        NextQuestion();

        AddListenersToButtons(sisi1Buttons);
        AddListenersToButtons(sisi2Buttons);
        AddListenersToButtons(sisi3Buttons);

        switch1Button.onClick.AddListener(() => SwitchToSisi(1));
        switch2Button.onClick.AddListener(() => SwitchToSisi(2));
        switch3Button.onClick.AddListener(() => SwitchToSisi(3));

        UpdateKeyboardVisibility();

        checkButton.onClick.AddListener(CheckAnswer);
        deleteButton.onClick.AddListener(DeleteLastCharacter);
    }

    void AddListenersToButtons(Button[] buttons)
    {
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                TMP_Text txt = button.GetComponentInChildren<TMP_Text>();
                if (txt != null)
                {
                    string charText = txt.text;
                    AddButtonListener(button, charText);
                }
            }
        }
    }

    void AddButtonListener(Button button, string character)
    {
        button.onClick.AddListener(() => {
            AddHiraganaToInputField(character);
            Debug.Log("Button clicked: " + character);
        });
    }

    void SwitchToSisi(int targetSisi)
    {
        if (currentSisi != targetSisi)
        {
            currentSisi = targetSisi;
            UpdateKeyboardVisibility();
        }
    }

    void UpdateKeyboardVisibility()
    {
        SetButtonsActive(sisi1Buttons, currentSisi == 1);
        SetButtonsActive(sisi2Buttons, currentSisi == 2);
        SetButtonsActive(sisi3Buttons, currentSisi == 3);
    }

    void SetButtonsActive(Button[] buttons, bool isActive)
    {
        foreach (Button b in buttons)
        {
            if (b != null) b.gameObject.SetActive(isActive);
        }
    }

    void ResetQuestions()
    {
        remainingQuestions = new List<KanjiQuestion>(kanjiQuestions);

        if (shuffleQuestions)
        {
            ShuffleQuestions();
        }
    }

    void ShuffleQuestions()
    {
        for (int i = 0; i < remainingQuestions.Count; i++)
        {
            KanjiQuestion temp = remainingQuestions[i];
            int randomIndex = Random.Range(i, remainingQuestions.Count);
            remainingQuestions[i] = remainingQuestions[randomIndex];
            remainingQuestions[randomIndex] = temp;
        }
    }

    public void CheckAnswer()
    {
        string playerAnswer = NormalizeInput(inputField.text);
        string correctAnswer = NormalizeInput(currentQuestion.answer);

        Debug.Log($"[DEBUG] Player Answer: '{playerAnswer}' | Correct Answer: '{correctAnswer}'");

        if (playerAnswer == correctAnswer)
        {
            Debug.Log("Jawaban benar!");
            if (correctSFX != null && audioSource != null) audioSource.PlayOneShot(correctSFX);
            NextQuestion();
        }
        else
        {
            Debug.Log("Jawaban salah!");
            if (wrongSFX != null && audioSource != null) audioSource.PlayOneShot(wrongSFX);
        }

        inputField.text = "";
    }

    void NextQuestion()
    {
        if (remainingQuestions.Count == 0)
        {
            Debug.Log("Semua soal telah dijawab!");
            SceneManager.LoadScene("Hasil");
            return;
        }

        currentQuestion = remainingQuestions[0];
        remainingQuestions.RemoveAt(0);

        kanjiText.text = currentQuestion.kanji;

        if (kanjiImage != null)
        {
            kanjiImage.sprite = currentQuestion.image;
            kanjiImage.gameObject.SetActive(currentQuestion.image != null);
        }
    }

    public void AddHiraganaToInputField(string hiraganaChar)
    {
        if (inputField.text.Length < inputField.characterLimit)
        {
            inputField.text += hiraganaChar;
        }
    }

    public void DeleteLastCharacter()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    string NormalizeInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        // Hapus spasi dan normalisasi unicode (penting untuk karakter Jepang)
        return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray())
            .Normalize(NormalizationForm.FormC)
            .ToLowerInvariant();
    }
}
