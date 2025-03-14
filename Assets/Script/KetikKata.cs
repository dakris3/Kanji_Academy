using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // Import for Button

public class KetikKata : MonoBehaviour
{
    public TMP_Text kanjiText;           // TextMesh Pro for displaying Kanji
    public TMP_InputField inputField;    // TMP InputField for typing answers

    [System.Serializable]
    public class KanjiQuestion
    {
        public string kanji;   // Kanji displayed
        public string answer;  // Expected answer (hiragana, kanji, or romaji)
    }

    public KanjiQuestion[] kanjiQuestions;
    private List<KanjiQuestion> remainingQuestions;
    private KanjiQuestion currentQuestion;

    // Array of buttons for each hiragana character
    public Button[] hiraganaButtons;
    public Button checkButton;  // Button to check the answer
    public Button deleteButton; // Button to delete the last character

    void Start()
    {
        // Initialize input field settings
        inputField.inputType = TMP_InputField.InputType.Standard;
        inputField.keyboardType = TouchScreenKeyboardType.Default;
        inputField.characterValidation = TMP_InputField.CharacterValidation.None;

        // Initialize the question pool
        ResetQuestions();
        NextQuestion();

        // Add onClick listeners for each hiragana button
        foreach (Button button in hiraganaButtons)
        {
            string hiraganaChar = button.GetComponentInChildren<TMP_Text>().text;
            button.onClick.AddListener(() => AddHiraganaToInputField(hiraganaChar));
        }

        // Add onClick listener for the check button
        checkButton.onClick.AddListener(CheckAnswer);

        // Add onClick listener for the delete button
        deleteButton.onClick.AddListener(DeleteLastCharacter);  // Add delete functionality
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

    // Function to add selected hiragana to the input field
    public void AddHiraganaToInputField(string hiraganaChar)
    {
        inputField.text += hiraganaChar;
    }

    // Function to delete the last character in the input field
    public void DeleteLastCharacter()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1); // Remove last character
        }
    }

    string NormalizeInput(string input)
    {
        return input.Trim().ToLower();
    }
}
