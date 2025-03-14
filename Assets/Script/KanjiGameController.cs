using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemukanKata : MonoBehaviour
{
    // Referensi untuk elemen UI
    public TextMeshProUGUI kanjiText;        // Teks untuk menampilkan Kanji
    public Button[] hiraganaButtons;         // Tombol-tombol Hiragana
    public Button checkButton;               // Tombol Check untuk memeriksa jawaban
    public string correctHiragana;           // Jawaban Hiragana yang benar

    private List<string> kanjiList = new List<string>() { "人", "子", "女", "男", "目", "口", "耳", "手", "足", "力" };  // List 10 Kanji
    private List<string> hiraganaAnswers = new List<string>() { "ひと", "こども", "おんな", "おとこ", "め", "くち", "みみ", "て", "あし", "ちから" }; // Jawaban Hiragana

    private int totalQuestions = 10;    // Jumlah total pertanyaan
    private int currentQuestionCount = 0; // Pertanyaan yang telah dijawab

    private List<int> remainingIndexes;  // Index yang tersisa setelah shuffle
    private List<Button> selectedButtons = new List<Button>(); // List untuk menyimpan tombol yang dipilih
    public Color defaultButtonColor;    // Warna default dari tombol Hiragana

    void Start()
    {
        remainingIndexes = new List<int>();

        // Tambahkan semua indeks ke dalam list
        for (int i = 0; i < kanjiList.Count; i++)
        {
            remainingIndexes.Add(i);
        }

        // Shuffle index secara acak
        Shuffle(remainingIndexes);

        // Set Kanji pertama
        ShowNewKanji();

        // Simpan warna default dari tombol Hiragana
        defaultButtonColor = hiraganaButtons[0].GetComponent<Image>().color;

        // Assign fungsi klik ke setiap tombol Hiragana
        foreach (Button btn in hiraganaButtons)
        {
            btn.onClick.AddListener(() => OnHiraganaButtonClick(btn));
        }

        // Assign fungsi klik ke tombol Check
        checkButton.onClick.AddListener(CheckAnswer);
    }

    // Fungsi untuk mengacak daftar indeks (Fisher-Yates shuffle)
    void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Fungsi untuk menampilkan Kanji baru
    void ShowNewKanji()
    {
        if (currentQuestionCount < totalQuestions)
        {
            // Ambil index pertama dari list yang sudah di-shuffle
            int index = remainingIndexes[0];
            remainingIndexes.RemoveAt(0);  // Hapus index yang telah digunakan
            
            // Set Kanji dan jawaban Hiragana yang benar
            kanjiText.text = kanjiList[index];
            correctHiragana = hiraganaAnswers[index];

            currentQuestionCount++;  // Tambah hitungan pertanyaan yang telah dijawab

            // Kosongkan pilihan sebelumnya dan reset warna tombol
            selectedButtons.Clear();
            ResetButtonColors();
        }
        else
        {
            // Semua pertanyaan telah dijawab
            kanjiText.text = "Semua pertanyaan selesai!";  // Menampilkan pesan saat semua pertanyaan selesai
        }
    }

    // Fungsi saat tombol Hiragana diklik
    public void OnHiraganaButtonClick(Button clickedButton)
    {
        // Jika tombol ini belum dipilih, tambahkan ke list
        if (!selectedButtons.Contains(clickedButton))
        {
            selectedButtons.Add(clickedButton);
            // Ubah warna tombol sebagai tanda pilihan
            clickedButton.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            // Jika tombol sudah dipilih sebelumnya, batalkan pilihannya
            selectedButtons.Remove(clickedButton);
            clickedButton.GetComponent<Image>().color = defaultButtonColor;
        }
    }

    // Fungsi untuk memeriksa jawaban ketika tombol Check diklik
    public void CheckAnswer()
    {
        bool isCorrect = true;  // Flag apakah jawaban benar

        // Loop melalui setiap tombol yang dipilih pemain
        for (int i = 0; i < selectedButtons.Count; i++)
        {
            Button selectedButton = selectedButtons[i];

            // Dapatkan teks Hiragana dari tombol yang dipilih
            string selectedText = selectedButton.GetComponentInChildren<TextMeshProUGUI>().text;

            // Cek apakah urutan huruf yang dipilih sesuai dengan jawaban yang benar
            if (i < correctHiragana.Length && selectedText == correctHiragana[i].ToString())
            {
                // Tandai tombol yang benar dengan warna hijau
                selectedButton.GetComponent<Image>().color = Color.green;
            }
            else
            {
                // Tandai tombol yang salah dengan warna merah
                selectedButton.GetComponent<Image>().color = Color.red;
                isCorrect = false;  // Tandai bahwa ada kesalahan
            }
        }

        if (isCorrect)
        {
            // Jika semua jawaban benar, lanjutkan ke pertanyaan berikutnya
            Invoke("ShowNewKanji", 2f);
        }
        else
        {
            // Jika ada jawaban yang salah, reset tombol setelah 2 detik
            Invoke("ResetButtonColors", 2f);
        }
    }

    // Fungsi untuk mereset warna tombol ke warna default
    void ResetButtonColors()
    {
        foreach (Button btn in hiraganaButtons)
        {
            btn.GetComponent<Image>().color = defaultButtonColor;
        }
        selectedButtons.Clear();  // Kosongkan daftar tombol yang dipilih
    }
}
