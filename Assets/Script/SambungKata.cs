using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class SambungKata : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();

    public List<Button> romajiButtons = new List<Button>();
    public List<Button> kanjiButtons = new List<Button>();
    public List<Button> soundButtons = new List<Button>(); // Tombol suara

    public List<string> romajiSet1 = new List<string>();
    public List<string> kanjiSet1 = new List<string>();
    public List<string> romajiSet2 = new List<string>();
    public List<string> kanjiSet2 = new List<string>();

    public List<string> correctPairSet1 = new List<string>();
    public List<string> correctPairSet2 = new List<string>();

    private Button selectedRomaji;
    private Button selectedKanji;

    private List<Button> connectedRomajiButtons = new List<Button>();
    private List<Button> connectedKanjiButtons = new List<Button>();

    public Color correctButtonColor;
    public Color defaultButtonColor;

    private int currentSet = 1;

    public List<AudioClip> kanjiSounds = new List<AudioClip>();
    public AudioSource audioSource;

    void Start()
    {
        lineRenderer.enabled = false;
        SetupListeners();
        SetContent(romajiSet1, kanjiSet1);
    }

    void SetupListeners()
    {
        for (int i = 0; i < romajiButtons.Count; i++)
        {
            int index = i;
            romajiButtons[i].onClick.AddListener(() => OnRomajiButtonClicked(romajiButtons[index]));
        }
        for (int i = 0; i < kanjiButtons.Count; i++)
        {
            int index = i;
            kanjiButtons[i].onClick.AddListener(() => OnKanjiButtonClicked(kanjiButtons[index], index));
        }
        for (int i = 0; i < soundButtons.Count; i++)
        {
            int index = i;
            soundButtons[i].onClick.AddListener(() => PlayKanjiSound(index));
        }
    }

    void SetContent(List<string> romajiContent, List<string> kanjiContent)
    {
        for (int i = 0; i < romajiButtons.Count; i++)
        {
            romajiButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = romajiContent[i];
        }
        for (int i = 0; i < kanjiButtons.Count; i++)
        {
            kanjiButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = kanjiContent[i];
        }
    }

    void OnRomajiButtonClicked(Button clickedButton)
    {
        if (selectedRomaji == null)
        {
            selectedRomaji = clickedButton;
        }
    }

    void OnKanjiButtonClicked(Button clickedButton, int index)
    {
        if (selectedRomaji != null && selectedKanji == null)
        {
            selectedKanji = clickedButton;
            points.Clear();
            points.Add(selectedRomaji.transform.position);
            points.Add(selectedKanji.transform.position);

            if (IsCorrectPair(selectedRomaji.GetComponentInChildren<TextMeshProUGUI>().text, 
                              selectedKanji.GetComponentInChildren<TextMeshProUGUI>().text))
            {
                Debug.Log("Correct");
                selectedRomaji.GetComponent<Image>().color = correctButtonColor;
                clickedButton.GetComponent<Image>().color = correctButtonColor;
                connectedRomajiButtons.Add(selectedRomaji);
                connectedKanjiButtons.Add(selectedKanji);

                if (connectedRomajiButtons.Count == 5)
                {
                    if (currentSet == 1)
                    {
                        currentSet = 2;
                        SwitchToSet2();
                    }
                    else
                    {
                        NextQuestion();
                    }
                }
            }

            selectedRomaji = null;
            selectedKanji = null;
        }
    }

    bool IsCorrectPair(string romaji, string kanji)
    {
        if (currentSet == 1)
        {
            return correctPairSet1.Contains(romaji + "-" + kanji);
        }
        else if (currentSet == 2)
        {
            return correctPairSet2.Contains(romaji + "-" + kanji);
        }
        return false;
    }

    void SwitchToSet2()
    {
        connectedRomajiButtons.Clear();
        connectedKanjiButtons.Clear();
        foreach (Button button in romajiButtons)
        {
            button.GetComponent<Image>().color = defaultButtonColor;
        }
        foreach (Button button in kanjiButtons)
        {
            button.GetComponent<Image>().color = defaultButtonColor;
        }
        SetContent(romajiSet2, kanjiSet2);
        Debug.Log("Switched to Set 2");
    }

    void NextQuestion()
    {
        Debug.Log("Level telah selesai");
        SceneManager.LoadScene("Hasil");
    }

    void PlayKanjiSound(int index)
    {
        int adjustedIndex = (currentSet == 2) ? index + 5 : index; // Pindah ke suara 6-10 jika Set 2
        if (audioSource != null && adjustedIndex < kanjiSounds.Count)
        {
            audioSource.PlayOneShot(kanjiSounds[adjustedIndex]);
        }
    }
}
