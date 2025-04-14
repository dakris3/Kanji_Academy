using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // <-- Tambahkan ini untuk pindah scene

public class KetikKata : MonoBehaviour
{
    public TMP_Text kanjiText;
    public TMP_InputField inputField;

    [System.Serializable]
    public class KanjiQuestion
    {
        public string kanji;
        public string answer;
    }

    public KanjiQuestion[] kanjiQuestions;
    private List<KanjiQuestion> remainingQuestions;
    private KanjiQuestion currentQuestion;

    public Button[] sisi1Buttons;
    public Button[] sisi2Buttons;
    public Button toggleButton;

    public Button checkButton;
    public Button deleteButton;

    private bool showingSisi2 = false;

    void Start()
    {
        inputField.inputType = TMP_InputField.InputType.Standard;
        inputField.keyboardType = TouchScreenKeyboardType.Default;
        inputField.characterValidation = TMP_InputField.CharacterValidation.None;
        inputField.characterLimit = 10;

        ResetQuestions();
        NextQuestion();

        foreach (Button button in sisi1Buttons)
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

        foreach (Button button in sisi2Buttons)
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

        toggleButton.onClick.AddListener(ToggleKeyboard);
        UpdateKeyboardVisibility();

        checkButton.onClick.AddListener(CheckAnswer);
        deleteButton.onClick.AddListener(DeleteLastCharacter);
    }

    void AddButtonListener(Button button, string character)
    {
        button.onClick.AddListener(() => {
            AddHiraganaToInputField(character);
            Debug.Log("Button clicked: " + character);
        });
    }

    void ToggleKeyboard()
    {
        showingSisi2 = !showingSisi2;
        UpdateKeyboardVisibility();
    }

    void UpdateKeyboardVisibility()
    {
        foreach (Button b in sisi1Buttons)
        {
            if (b != null) b.gameObject.SetActive(!showingSisi2);
        }

        foreach (Button b in sisi2Buttons)
        {
            if (b != null) b.gameObject.SetActive(showingSisi2);
        }
    }

    void ResetQuestions()
    {
        remainingQuestions = new List<KanjiQuestion>(kanjiQuestions);
        ShuffleQuestions();
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

        if (playerAnswer == correctAnswer)
        {
            Debug.Log("Jawaban benar!");
            NextQuestion();
        }
        else
        {
            Debug.Log("Jawaban salah!");
        }

        inputField.text = "";
    }

    void NextQuestion()
    {
        if (remainingQuestions.Count == 0)
        {
            Debug.Log("Semua soal telah dijawab!");
            SceneManager.LoadScene("Hasil"); // <-- Pindah ke scene Hasil
            return;
        }

        currentQuestion = remainingQuestions[0];
        remainingQuestions.RemoveAt(0);
        kanjiText.text = currentQuestion.kanji;
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
        return input.Trim().ToLower();
    }
}
