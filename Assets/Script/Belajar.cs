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

        // Urutan gambar 1
        if (urutanDisplay1 != null)
        {
            if (hasUrutan1)
            {
                urutanDisplay1.sprite = kanji.urutanImage1;
                urutanDisplay1.enabled = true;
            }
            else
            {
                urutanDisplay1.sprite = null;
                urutanDisplay1.enabled = false;
            }
        }

        // Urutan gambar 2
        if (urutanDisplay2 != null)
        {
            if (hasUrutan2)
            {
                urutanDisplay2.sprite = kanji.urutanImage2;
                urutanDisplay2.enabled = true;
            }
            else
            {
                urutanDisplay2.sprite = null;
                urutanDisplay2.enabled = false;
            }
        }

        // Atur posisi jika salah satu atau dua gambar urutan aktif
        if (urutanDisplay1 != null && urutanDisplay2 != null)
        {
            if (hasUrutan1 && !hasUrutan2)
            {
                urutanDisplay1.rectTransform.anchoredPosition = new Vector2(0f, 85f); // di tengah atas
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
        if (kanjiList != null && kanjiList.Count > 0 && kanjiList[currentKanjiIndex].kanjiAudio != null)
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
