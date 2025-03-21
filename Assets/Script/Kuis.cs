using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Tambahkan ini untuk memuat scene

public class Kuis : MonoBehaviour
{
    public Image quizImage;
    public Button audioButton;
    public Button[] answerButtons;
    private AudioSource audioSource;
    private List<int> questionOrder;
    private int currentQuestionIndex = 0;

    public LevelManager levelManager; // Hubungkan ke LevelManager

    [System.Serializable]
    public class QuestionData
    {
        public Sprite questionImage;
        public AudioClip questionAudio;
        public string[] answers;
        public int correctAnswerIndex;
    }

    public QuestionData[] questions;

    void Start()
    {
        // Pastikan LevelManager ditemukan
        levelManager = FindObjectOfType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("LevelManager tidak ditemukan di scene!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource tidak ditemukan pada GameObject ini.");
            return;
        }

        if (quizImage == null || audioButton == null || answerButtons == null || answerButtons.Length == 0 || questions == null || questions.Length == 0)
        {
            Debug.LogError("Ada komponen yang belum diatur di Inspector!");
            return;
        }

        InitializeQuestionOrder();
        DisplayQuestion();
        audioButton.onClick.AddListener(PlayQuestionAudio);
    }

    void InitializeQuestionOrder()
    {
        questionOrder = new List<int>();
        for (int i = 0; i < questions.Length; i++)
        {
            questionOrder.Add(i);
        }
        ShuffleList(questionOrder);
    }

    void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(0, list.Count);
            int temp = list[rnd];
            list[rnd] = list[i];
            list[i] = temp;
        }
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex >= questionOrder.Count)
        {
            Debug.LogError("currentQuestionIndex melebihi jumlah pertanyaan!");
            return;
        }

        int questionIndex = questionOrder[currentQuestionIndex];

        quizImage.sprite = questions[questionIndex].questionImage;
        audioSource.clip = questions[questionIndex].questionAudio;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TMP_Text buttonText = answerButtons[i].GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = questions[questionIndex].answers[i];
            }

            answerButtons[i].onClick.RemoveAllListeners();
            int index = i;
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    void PlayQuestionAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    void CheckAnswer(int index)
    {
        if (currentQuestionIndex >= questionOrder.Count)
        {
            Debug.LogError("currentQuestionIndex melebihi jumlah pertanyaan!");
            return;
        }

        int questionIndex = questionOrder[currentQuestionIndex];

        if (index == questions[questionIndex].correctAnswerIndex)
        {
            Debug.Log("Jawaban benar!");
            NextQuestion();
        }
        else
        {
            Debug.Log("Jawaban salah, coba lagi.");
        }
    }

    void NextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex < questionOrder.Count)
        {
            DisplayQuestion();
        }
        else
        {
            Debug.Log("Level telah selesai");

            if (levelManager != null)
            {
                levelManager.LevelComplete();
            }
            else
            {
                Debug.LogError("LevelManager tidak ditemukan! Tidak bisa menyelesaikan level.");
            }

            // Pindah ke scene "Hasil"
            SceneManager.LoadScene("Hasil");
        }
    }
}
