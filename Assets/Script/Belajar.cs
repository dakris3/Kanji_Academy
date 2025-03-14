using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public AudioClip kanjiAudio; // Tambahkan audio untuk setiap kanji
    }

    public List<KanjiInfo> kanjiList;

    public Image kanjiDisplay;
    public Image writingDisplay;
    public Text meaningText;
    public Text kunyomiText;
    public Text romajiText;
    public Text onyomiText;
    public AudioSource audioSource; // Komponen AudioSource untuk memutar audio

    public float kanjiBoxHeightRatio = 0f;
    public float kanjiBoxWidthRatio = 0f;
    public float writingBoxHeightRatio = 0f;
    public float writingBoxWidthRatio = 0f;

    private int currentKanjiIndex = 0;

    void Start()
    {
        ShowKanji(currentKanjiIndex);
    }

    public void ShowKanji(int index)
    {
        if (index < 0 || index >= kanjiList.Count) return;

        KanjiInfo kanji = kanjiList[index];
        kanjiDisplay.sprite = kanji.kanjiImage;
        writingDisplay.sprite = kanji.writingImage;

        meaningText.text = kanji.meaning;
        kunyomiText.text = kanji.kunyomi;
        romajiText.text = kanji.romaji;
        onyomiText.text = kanji.onyomi;
    }

    public void PlayKanjiAudio()
    {
        if (kanjiList[currentKanjiIndex].kanjiAudio != null)
        {
            audioSource.clip = kanjiList[currentKanjiIndex].kanjiAudio;
            audioSource.Play();
        }
    }

    public void NextKanji()
    {
        currentKanjiIndex++;
        if (currentKanjiIndex >= kanjiList.Count)
        {
            currentKanjiIndex = 0;
        }
        ShowKanji(currentKanjiIndex);
    }
}
