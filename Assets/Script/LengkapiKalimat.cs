using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LengkapiKalimat : MonoBehaviour
{
    // UI Elements
    public TextMeshProUGUI japaneseText;
    public TextMeshProUGUI romajiText;
    public TextMeshProUGUI indonesianText;
    public Button[] answerButtons;

    // Tambahan untuk memanggil LevelManager
    public LevelManager levelManager;

    [Header("Sound Effects")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;
    private AudioSource audioSource;

    [System.Serializable]
    public class Question
    {
        public string japaneseText;
        public string romajiText;
        public string indonesianText;
        public string[] answers;
        public int correctAnswerIndex;
    }

    public Question[] questions;
    private int currentQuestionIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource tidak ditemukan! Harap tambahkan komponen AudioSource ke GameObject ini.");
        }

        ShuffleQuestions(); // Acak urutan soal
        ShowQuestion();
    }

    void ShowQuestion()
    {
        Question currentQuestion = questions[currentQuestionIndex];

        japaneseText.text = currentQuestion.japaneseText;
        romajiText.text = currentQuestion.romajiText;
        indonesianText.text = currentQuestion.indonesianText;

        List<string> shuffledAnswers = new List<string>(currentQuestion.answers);
        Shuffle(shuffledAnswers); // Acak jawaban

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < shuffledAnswers.Count)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = shuffledAnswers[i];
                int answerIndex = i;

                answerButtons[i].GetComponent<Image>().color = Color.white;

                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() =>
                    OnAnswerSelected(shuffledAnswers[answerIndex], currentQuestion.answers[currentQuestion.correctAnswerIndex]));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnAnswerSelected(string selectedAnswer, string correctAnswer)
    {
        if (selectedAnswer == correctAnswer)
        {
            Debug.Log("Jawaban benar!");
            if (correctSFX != null && audioSource != null)
                audioSource.PlayOneShot(correctSFX);

            NextQuestion();
        }
        else
        {
            Debug.Log("Jawaban salah, coba lagi.");

            foreach (Button btn in answerButtons)
            {
                if (btn.GetComponentInChildren<TextMeshProUGUI>().text == correctAnswer)
                    btn.GetComponent<Image>().color = Color.green;
            }

            foreach (Button btn in answerButtons)
            {
                if (btn.GetComponentInChildren<TextMeshProUGUI>().text == selectedAnswer)
                    btn.GetComponent<Image>().color = Color.red;
            }

            if (wrongSFX != null && audioSource != null)
                audioSource.PlayOneShot(wrongSFX);

            StartCoroutine(ResetButtonColor());
        }
    }

    IEnumerator ResetButtonColor()
    {
        yield return new WaitForSeconds(1);

        foreach (Button btn in answerButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
    }

    void NextQuestion()
    {
        if (currentQuestionIndex < questions.Length - 1)
        {
            currentQuestionIndex++;
            ShowQuestion();
        }
        else
        {
            Debug.Log("Semua pertanyaan telah dijawab!");

            if (levelManager != null)
            {
                levelManager.LevelComplete();
            }
            else
            {
                Debug.LogWarning("LevelManager belum di-assign!");
            }

            SceneManager.LoadScene("Hasil");
        }
    }

    void Shuffle(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            string temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void ShuffleQuestions()
    {
        for (int i = questions.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Question temp = questions[i];
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }
    }
}
