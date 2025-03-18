using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Pastikan TMP sudah di-import

public class Kuis : MonoBehaviour
{
    // Referensi untuk elemen UI
    public Image quizImage;            // Gambar kuis
    public Button audioButton;         // Tombol untuk memutar audio
    public Button[] answerButtons;     // Tombol jawaban

    private AudioSource audioSource;   // Komponen AudioSource untuk memutar audio
    private List<int> questionOrder;   // Urutan soal acak
    private int currentQuestionIndex = 0; // Indeks soal saat ini

    public PointManager pointManager;

    [System.Serializable]
    public class QuestionData
    {
        public Sprite questionImage;    // Gambar untuk pertanyaan
        public AudioClip questionAudio; // Audio untuk pertanyaan
        public string[] answers;        // Jawaban pilihan ganda
        public int correctAnswerIndex;  // Indeks jawaban benar
    }

    // Array pertanyaan yang langsung diatur di dalam skrip
    public QuestionData[] questions;

    void Start()
    {
        pointManager = FindObjectOfType<PointManager>();
        audioSource = GetComponent<AudioSource>(); // Mengambil komponen AudioSource
        
        // Cek apakah audioSource sudah diatur dengan benar
        if (audioSource == null)
        {
            Debug.LogError("AudioSource tidak ditemukan pada GameObject ini.");
            return; // Hentikan eksekusi jika AudioSource tidak ditemukan
        }

        // Cek apakah array `questions` sudah diisi
        if (questions == null || questions.Length == 0)
        {
            Debug.LogError("Pertanyaan belum diatur. Tambahkan pertanyaan di Inspector.");
            return;
        }

        // Cek apakah `answerButtons` sudah diatur di Inspector
        if (answerButtons == null || answerButtons.Length == 0)
        {
            Debug.LogError("Answer buttons belum diatur di Inspector.");
            return;
        }

        InitializeQuestionOrder();  // Menginisialisasi urutan pertanyaan
        DisplayQuestion();          // Menampilkan pertanyaan pertama

        // Tambahkan listener untuk tombol audio
        audioButton.onClick.AddListener(PlayQuestionAudio);
    }

    // Membuat urutan pertanyaan secara acak
    void InitializeQuestionOrder()
    {
        questionOrder = new List<int>();
        for (int i = 0; i < questions.Length; i++)
        {
            questionOrder.Add(i);
        }

        ShuffleList(questionOrder);
    }

    // Fungsi untuk mengacak list
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

    // Menampilkan pertanyaan di UI
    void DisplayQuestion()
    {
        int questionIndex = questionOrder[currentQuestionIndex];

        // Menampilkan gambar kuis
        quizImage.sprite = questions[questionIndex].questionImage;

        // Memastikan audio pertanyaan yang benar siap diputar
        audioSource.clip = questions[questionIndex].questionAudio;

        // Menampilkan pilihan jawaban
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TMP_Text buttonText = answerButtons[i].GetComponentInChildren<TMP_Text>(); // Menggunakan TMP_Text untuk teks tombol
            if (buttonText != null)
            {
                buttonText.text = questions[questionIndex].answers[i];
            }
            else
            {
                Debug.LogError("TMP_Text tidak ditemukan pada tombol " + i);
            }

            answerButtons[i].onClick.RemoveAllListeners();  // Menghapus listener sebelumnya
            int index = i;  // Local copy untuk digunakan di listener
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    // Memutar audio pertanyaan
    void PlayQuestionAudio()
    {
        // Memutar audio pertanyaan yang terkait dengan pertanyaan saat ini
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip belum diatur untuk pertanyaan ini.");
        }
    }

    // Memeriksa apakah jawaban yang dipilih benar atau salah
    void CheckAnswer(int index)
    {
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

    // Lanjut ke pertanyaan berikutnya
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
    pointManager.AddPoints(100);
    Debug.Log(pointManager.totalPoints);

    // Panggil hasil dari LevelResultHandler
    FindObjectOfType<Hasil>().ShowResult();
        }

    }
}
