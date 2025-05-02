using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Belajar : MonoBehaviour
{
    [System.Serializable]
    public class KanjiInfo
    {
        public Sprite kanjiImage;
        public Sprite writingImage;
        public string meaning;
        public string kunyomi;
        public string romaji;
        public string onyomi;
        public AudioClip kanjiAudio;

        public Sprite urutanImage1;
        public Sprite urutanImage2;
    }

    public List<KanjiInfo> kanjiList;

    public Image kanjiDisplay;
    public Image writingDisplay;
    public Image urutanDisplay1;
    public Image urutanDisplay2;

    public Text meaningText;
    public Text kunyomiText;
    public Text romajiText;
    public Text onyomiText;
    public AudioSource audioSource;

    public float kanjiBoxHeightRatio = 0f;
    public float kanjiBoxWidthRatio = 0f;
    public float writingBoxHeightRatio = 0f;
    public float writingBoxWidthRatio = 0f;

    private int currentKanjiIndex = 0;

    [Header("Scene Tujuan Setelah Tutorial")]
    public string targetSceneAfterTutorial;

    [Header("Nama Scene Tutorial (misal: Tutorial1, Tutorial2, dst.)")]
    public string selectedTutorialScene = "Tutorial1";

    void Start()
    {
        if (kanjiList != null && kanjiList.Count > 0)
        {
            ShowKanji(currentKanjiIndex);
        }
        else
        {
            Debug.LogWarning("Kanji list kosong atau belum diatur!");
        }
    }

    public void ShowKanji(int index)
    {
        if (index < 0 || index >= kanjiList.Count) return;

        KanjiInfo kanji = kanjiList[index];

        if (kanjiDisplay != null) kanjiDisplay.sprite = kanji.kanjiImage;
        if (writingDisplay != null) writingDisplay.sprite = kanji.writingImage;

        if (meaningText != null) meaningText.text = kanji.meaning;
        if (kunyomiText != null) kunyomiText.text = kanji.kunyomi;
        if (romajiText != null) romajiText.text = kanji.romaji;
        if (onyomiText != null) onyomiText.text = kanji.onyomi;

        bool hasUrutan1 = kanji.urutanImage1 != null;
        bool hasUrutan2 = kanji.urutanImage2 != null;

        if (urutanDisplay1 != null)
        {
            urutanDisplay1.sprite = hasUrutan1 ? kanji.urutanImage1 : null;
            urutanDisplay1.enabled = hasUrutan1;
        }

        if (urutanDisplay2 != null)
        {
            urutanDisplay2.sprite = hasUrutan2 ? kanji.urutanImage2 : null;
            urutanDisplay2.enabled = hasUrutan2;
        }

        if (urutanDisplay1 != null && urutanDisplay2 != null)
        {
            if (hasUrutan1 && !hasUrutan2)
            {
                urutanDisplay1.rectTransform.anchoredPosition = new Vector2(0f, 85f);
            }
            else if (hasUrutan1 && hasUrutan2)
            {
                urutanDisplay1.rectTransform.anchoredPosition = new Vector2(-67f, 85f);
                urutanDisplay2.rectTransform.anchoredPosition = new Vector2(67f, 85f);
            }
        }
    }

    public void PlayKanjiAudio()
    {
        if (audioSource != null &&
            kanjiList != null && 
            kanjiList.Count > 0 && 
            kanjiList[currentKanjiIndex].kanjiAudio != null)
        {
            audioSource.clip = kanjiList[currentKanjiIndex].kanjiAudio;
            audioSource.Play();
        }
    }

    public void NextKanji()
    {
        currentKanjiIndex = (currentKanjiIndex + 1) % kanjiList.Count;
        ShowKanji(currentKanjiIndex);
    }

    public void PreviousKanji()
    {
        currentKanjiIndex = (currentKanjiIndex - 1 + kanjiList.Count) % kanjiList.Count;
        ShowKanji(currentKanjiIndex);
    }

    public void StartTutorial()
    {
        if (!string.IsNullOrEmpty(targetSceneAfterTutorial) && !string.IsNullOrEmpty(selectedTutorialScene))
        {
            LevelRedirect.targetSceneName = targetSceneAfterTutorial;
            SceneManager.LoadScene(selectedTutorialScene);
        }
        else
        {
            Debug.LogWarning("Nama scene tutorial atau scene tujuan setelah tutorial belum ditentukan!");
        }
    }
}
