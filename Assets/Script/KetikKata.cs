using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    public Button[] hiraganaButtons;
    public Button[] dakutenButtons;
    public Button toggleButton;

    public Button checkButton;
    public Button deleteButton;

    private bool showingDakuten = false;

    void Start()
    {
        inputField.inputType = TMP_InputField.InputType.Standard;
        inputField.keyboardType = TouchScreenKeyboardType.Default;
        inputField.characterValidation = TMP_InputField.CharacterValidation.None;

        ResetQuestions();
        NextQuestion();

        foreach (Button button in hiraganaButtons)
        {
            string charText = button.GetComponentInChildren<TMP_Text>().text;
            button.onClick.AddListener(() => AddHiraganaToInputField(charText));
        }

        foreach (Button button in dakutenButtons)
        {
            string charText = button.GetComponentInChildren<TMP_Text>().text;
            button.onClick.AddListener(() => AddHiraganaToInputField(charText));
        }

        toggleButton.onClick.AddListener(ToggleKeyboard);
        UpdateKeyboardVisibility();

        checkButton.onClick.AddListener(CheckAnswer);
        deleteButton.onClick.AddListener(DeleteLastCharacter);
    }

    void ToggleKeyboard()
    {
        showingDakuten = !showingDakuten;
        UpdateKeyboardVisibility();
    }

    void UpdateKeyboardVisibility()
    {
        foreach (Button b in hiraganaButtons)
            b.gameObject.SetActive(!showingDakuten);

        foreach (Button b in dakutenButtons)
            b.gameObject.SetActive(showingDakuten);
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
            return;
        }

        currentQuestion = remainingQuestions[0];
        remainingQuestions.RemoveAt(0);
        kanjiText.text = currentQuestion.kanji;
    }

    public void AddHiraganaToInputField(string hiraganaChar)
    {
        inputField.text += hiraganaChar;
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
