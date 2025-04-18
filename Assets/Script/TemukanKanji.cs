using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TemukanKanji : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI kanjiText;
    public Button[] hiraganaButtons;
    public Button checkButton;

    [Header("Kanji & Hiragana Data")]
    public List<string> kanjiList = new List<string>();          // Diisi dari Inspector
    public List<string> hiraganaAnswers = new List<string>();    // Diisi dari Inspector

    private string correctHiragana;
    private int totalQuestions;
    private int currentQuestionCount = 0;

    private List<int> remainingIndexes;
    private List<Button> selectedButtons = new List<Button>();
    private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

    private LevelManager levelManager;

    void Start()
    {
        // Cari LevelManager (jika ada di scene)
        levelManager = FindObjectOfType<LevelManager>();

        // Validasi data
        if (kanjiList == null || hiraganaAnswers == null || kanjiList.Count != hiraganaAnswers.Count || kanjiList.Count == 0)
        {
            Debug.LogError("❌ Data Kanji dan Hiragana tidak valid! Periksa Inspector.");
            kanjiText.text = "Data tidak valid!";
            return;
        }

        totalQuestions = kanjiList.Count;
        remainingIndexes = new List<int>();

        for (int i = 0; i < totalQuestions; i++)
        {
            remainingIndexes.Add(i);
        }

        Shuffle(remainingIndexes);
        ShowNewKanji();

        // Simpan warna awal tombol & pasang event listener
        foreach (Button btn in hiraganaButtons)
        {
            originalColors[btn] = btn.GetComponent<Image>().color;
            btn.onClick.AddListener(() => OnHiraganaButtonClick(btn));
        }

        // Event tombol Check
        checkButton.onClick.AddListener(CheckAnswer);
    }

    void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    void ShowNewKanji()
    {
        if (currentQuestionCount < totalQuestions)
        {
            int index = remainingIndexes[0];
            remainingIndexes.RemoveAt(0);

            kanjiText.text = kanjiList[index];
            correctHiragana = hiraganaAnswers[index];

            currentQuestionCount++;
            selectedButtons.Clear();
            ResetButtonColors();
        }
        else
        {
            // Semua soal sudah dijawab
            kanjiText.text = "Semua pertanyaan selesai!";

            Debug.Log("Level telah selesai");

            if (levelManager != null)
            {
                levelManager.LevelComplete();
            }
            else
            {
                Debug.LogWarning("LevelManager tidak ditemukan!");
            }

            // Pindah ke scene hasil setelah delay sebentar
            Invoke("LoadResultScene", 2f);
        }
    }

    void LoadResultScene()
    {
        SceneManager.LoadScene("Hasil");
    }

    public void OnHiraganaButtonClick(Button clickedButton)
    {
        if (!selectedButtons.Contains(clickedButton))
        {
            selectedButtons.Add(clickedButton);
            clickedButton.GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f); // Kuning
        }
        else
        {
            selectedButtons.Remove(clickedButton);
            if (originalColors.ContainsKey(clickedButton))
            {
                clickedButton.GetComponent<Image>().color = originalColors[clickedButton];
            }
        }
    }

    public void CheckAnswer()
    {
        if (selectedButtons.Count != correctHiragana.Length)
        {
            Debug.Log("Jumlah huruf tidak cocok.");
            HighlightIncorrect();
            Invoke("ResetButtonColors", 2f);
            return;
        }

        bool isCorrect = true;

        for (int i = 0; i < selectedButtons.Count; i++)
        {
            string selectedText = selectedButtons[i].GetComponentInChildren<TextMeshProUGUI>().text;

            if (selectedText == correctHiragana[i].ToString())
            {
                selectedButtons[i].GetComponent<Image>().color = Color.green;
            }
            else
            {
                selectedButtons[i].GetComponent<Image>().color = Color.red;
                isCorrect = false;
            }
        }

        if (isCorrect)
        {
            Invoke("ShowNewKanji", 2f);
        }
        else
        {
            Invoke("ResetButtonColors", 2f);
        }
    }

    void HighlightIncorrect()
    {
        foreach (Button btn in selectedButtons)
        {
            btn.GetComponent<Image>().color = Color.red;
        }
    }

    void ResetButtonColors()
    {
        foreach (Button btn in hiraganaButtons)
        {
            if (originalColors.ContainsKey(btn))
            {
                btn.GetComponent<Image>().color = originalColors[btn];
            }
        }

        selectedButtons.Clear();
    }
}
