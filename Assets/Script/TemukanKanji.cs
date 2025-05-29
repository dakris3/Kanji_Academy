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
    public TextMeshProUGUI hintText;
    public Button hintToggleButton; // 🔁 Satu tombol untuk kedua hint
    public Button[] hiraganaButtons;
    public Button checkButton;

    [Header("Kanji & Hiragana Data")]
    public List<string> kanjiList = new List<string>();
    public List<string> hiraganaAnswers = new List<string>();
    public List<string> hint1List = new List<string>(); // Hint 1 (misal: romaji)
    public List<string> hint2List = new List<string>(); // Hint 2 (misal: arti)

    [Header("Sound Effects")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    private AudioSource audioSource;
    private string correctHiragana;
    private int totalQuestions;
    private int currentQuestionCount = 0;

    private List<int> remainingIndexes;
    private List<Button> selectedButtons = new List<Button>();
    private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

    private LevelManager levelManager;
    private int currentIndex;
    private bool showingHint1 = true; // 🔁 Status toggle hint

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource tidak ditemukan pada GameObject ini.");
        }

        if (kanjiList.Count != hiraganaAnswers.Count || kanjiList.Count != hint1List.Count || kanjiList.Count != hint2List.Count || kanjiList.Count == 0)
        {
            Debug.LogError("Data Kanji, Hiragana, atau Hint tidak valid! Periksa Inspector.");
            kanjiText.text = "Data tidak valid!";
            if (hintText != null) hintText.text = "";
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

        foreach (Button btn in hiraganaButtons)
        {
            originalColors[btn] = btn.GetComponent<Image>().color;
            btn.onClick.AddListener(() => OnHiraganaButtonClick(btn));
        }

        checkButton.onClick.AddListener(CheckAnswer);

        // 🔁 Tambahkan listener tombol toggle hint
        if (hintToggleButton != null)
            hintToggleButton.onClick.AddListener(ToggleHint);
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
            currentIndex = remainingIndexes[0];
            remainingIndexes.RemoveAt(0);

            kanjiText.text = kanjiList[currentIndex];
            correctHiragana = hiraganaAnswers[currentIndex];

            if (hintText != null)
            {
                hintText.text = "";
                hintText.enabled = true;
            }

            currentQuestionCount++;
            showingHint1 = true; // 🔁 Reset toggle ke Hint 1
            selectedButtons.Clear();
            ResetButtonColors();
        }
        else
        {
            kanjiText.text = "Semua pertanyaan selesai!";
            if (hintText != null) hintText.text = "";

            Debug.Log("Level telah selesai");

            if (levelManager != null)
            {
                levelManager.LevelComplete();
            }
            else
            {
                Debug.LogWarning("LevelManager tidak ditemukan!");
            }

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
            if (wrongSFX != null) audioSource.PlayOneShot(wrongSFX);
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
            if (correctSFX != null) audioSource.PlayOneShot(correctSFX);
            Invoke("ShowNewKanji", 1f);
        }
        else
        {
            if (wrongSFX != null) audioSource.PlayOneShot(wrongSFX);
            Invoke("ResetButtonColors", 1f);
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

    // 🔁 Fungsi toggle antara Hint 1 dan Hint 2
    public void ToggleHint()
    {
        if (hintText != null)
        {
            if (showingHint1 && currentIndex < hint1List.Count)
            {
                hintText.text = hint1List[currentIndex];
            }
            else if (!showingHint1 && currentIndex < hint2List.Count)
            {
                hintText.text = hint2List[currentIndex];
            }

            showingHint1 = !showingHint1;
        }
    }
}
