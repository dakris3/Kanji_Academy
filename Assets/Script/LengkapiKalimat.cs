using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LengkapiKalimat : MonoBehaviour
{
    // UI Elements
    public TextMeshProUGUI japaneseText;
    public TextMeshProUGUI romajiText;
    public TextMeshProUGUI indonesianText;
    public Button[] answerButtons;

    // Pertanyaan untuk kuis
    [System.Serializable]
    public class Question
    {
        public string japaneseText;
        public string romajiText;
        public string indonesianText;
        public string[] answers; // Pilihan jawaban
        public int correctAnswerIndex; // Index jawaban yang benar
    }

    // Array pertanyaan yang bisa diatur untuk setiap level dari Editor
    public Question[] questions;

    private int currentQuestionIndex = 0; // Indeks pertanyaan saat ini

    void Start()
    {
        // Tampilkan pertanyaan pertama
        ShowQuestion();
    }

    void ShowQuestion()
    {
        // Mendapatkan pertanyaan saat ini dari array questions
        Question currentQuestion = questions[currentQuestionIndex];

        // Update UI untuk pertanyaan saat ini
        japaneseText.text = currentQuestion.japaneseText;
        romajiText.text = currentQuestion.romajiText;
        indonesianText.text = currentQuestion.indonesianText;

        // Acak pilihan jawaban
        List<string> shuffledAnswers = new List<string>(currentQuestion.answers);
        Shuffle(shuffledAnswers);

        // Set jawaban pada tombol dengan pengecekan jumlah
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Jika ada jawaban pada index ini, set teks tombol
            if (i < shuffledAnswers.Count)
            {
                answerButtons[i].gameObject.SetActive(true); // Aktifkan tombol
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = shuffledAnswers[i];
                int answerIndex = i;  // Capture index untuk listener

                // Reset warna tombol ke default
                answerButtons[i].GetComponent<Image>().color = Color.white;

                // Hapus listener sebelumnya dan tambahkan listener baru
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(shuffledAnswers[answerIndex], currentQuestion.answers[currentQuestion.correctAnswerIndex]));
            }
            else
            {
                // Jika tidak ada jawaban, disable tombol
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnAnswerSelected(string selectedAnswer, string correctAnswer)
    {
        if (selectedAnswer == correctAnswer)
        {
            Debug.Log("Jawaban benar!");

            // Lanjutkan ke pertanyaan berikutnya jika benar
            NextQuestion();
        }
        else
        {
            Debug.Log("Jawaban salah, coba lagi.");
            // Berikan feedback visual untuk jawaban yang salah
            foreach (Button btn in answerButtons)
            {
                if (btn.GetComponentInChildren<TextMeshProUGUI>().text == correctAnswer)
                {
                    btn.GetComponent<Image>().color = Color.green; // Tampilkan jawaban yang benar
                }
            }

            // Ubah warna tombol yang salah ke merah
            foreach (Button btn in answerButtons)
            {
                if (btn.GetComponentInChildren<TextMeshProUGUI>().text == selectedAnswer)
                {
                    btn.GetComponent<Image>().color = Color.red;
                }
            }

            // Kembalikan warna tombol ke default setelah beberapa detik
            StartCoroutine(ResetButtonColor());
        }
    }

    IEnumerator ResetButtonColor()
    {
        yield return new WaitForSeconds(1);

        // Reset warna semua tombol kembali ke putih
        foreach (Button btn in answerButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
    }

    void NextQuestion()
    {
        // Cek apakah masih ada pertanyaan berikutnya
        if (currentQuestionIndex < questions.Length - 1)
        {
            currentQuestionIndex++;
            ShowQuestion();
        }
        else
        {
            Debug.Log("Semua pertanyaan telah dijawab!");
            // Tampilkan pesan atau alihkan ke tampilan lain
        }
    }

    // Fungsi untuk mengacak jawaban
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
}
