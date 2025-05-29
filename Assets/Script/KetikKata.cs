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
    public TMP_Text hintText;
    public TMP_InputField inputField;
    public Image kanjiImage;
    public Button audioButton;

    [System.Serializable]
    public class KanjiQuestion
    {
        [Header("Hint 1 (Contoh: Kanji)")]
        public string hint1;

        [Header("Hint 2 (Contoh: Romaji)")]
        public string hint2;

        public string answer;
        public Sprite image;
        public AudioClip audio;
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

    public Button hintToggleButton; // 🔁 Satu tombol hint

    private int currentSisi = 1;
    private bool showingHint1 = true; // Untuk toggle

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

        checkButton.onClick.AddListener(CheckAnswer);
        deleteButton.onClick.AddListener(DeleteLastCharacter);

        if (audioButton != null)
        {
            audioButton.onClick.AddListener(PlayKanjiAudio);
        }

        if (hintToggleButton != null)
        {
            hintToggleButton.onClick.AddListener(ToggleHint);
        }

        UpdateKeyboardVisibility();
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
        button.onClick.AddListener(() =>
        {
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

        if (kanjiImage != null)
        {
            kanjiImage.sprite = currentQuestion.image;
            kanjiImage.gameObject.SetActive(currentQuestion.image != null);
        }

        if (hintText != null)
        {
            hintText.text = "";
        }

        showingHint1 = true; // Reset ke Hint 1 setiap soal baru
    }

    void PlayKanjiAudio()
    {
        if (currentQuestion != null && currentQuestion.audio != null && audioSource != null)
        {
            audioSource.PlayOneShot(currentQuestion.audio);
        }
    }

    // 🔁 Toggle Hint 1 dan Hint 2
    void ToggleHint()
    {
        if (currentQuestion != null && hintText != null)
        {
            if (showingHint1)
            {
                hintText.text = currentQuestion.hint1;
            }
            else
            {
                hintText.text = currentQuestion.hint2;
            }

            showingHint1 = !showingHint1;
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

        return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray())
            .Normalize(NormalizationForm.FormC)
            .ToLowerInvariant();
    }
}
