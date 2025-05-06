using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Kuis : MonoBehaviour
{
    public Image quizImage;
    public Button audioButton;
    public Button[] answerButtons;
    private AudioSource audioSource;
    private List<int> questionOrder;
    private int currentQuestionIndex = 0;

    public LevelManager levelManager;

    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color defaultColor = Color.white;

    [Header("Sound Effects")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

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

        ResetButtonColors();

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
        int correctIndex = questions[questionIndex].correctAnswerIndex;

        answerButtons[correctIndex].GetComponent<Image>().color = correctColor;

        if (index != correctIndex)
        {
            Debug.Log("Jawaban salah, coba lagi.");
            answerButtons[index].GetComponent<Image>().color = wrongColor;

            if (wrongSFX != null)
                audioSource.PlayOneShot(wrongSFX);

            StartCoroutine(ResetAfterDelay(0.5f));
            return;
        }

        Debug.Log("Jawaban benar!");
        if (correctSFX != null)
            audioSource.PlayOneShot(correctSFX);

        StartCoroutine(NextQuestionAfterDelay(0f));
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetButtonColors();
    }

    IEnumerator NextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetButtonColors();
        NextQuestion();
    }

    void ResetButtonColors()
    {
        foreach (Button btn in answerButtons)
        {
            btn.GetComponent<Image>().color = defaultColor;
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

            SceneManager.LoadScene("Hasil");
        }
    }
}
