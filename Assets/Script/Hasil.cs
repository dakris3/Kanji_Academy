using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Hasil : MonoBehaviour
{
    public string nextSceneName;

    public TMP_Text textSelamat;
    public TMP_Text pointText;            // Tambahan: untuk menampilkan poin
    public GameObject coin;
    public GameObject point;

    [TextArea]
    public string pesanSelamat = "Level telah selesai.\nおつかれさまでした";

    [Header("Sound Effect")]
    public AudioClip sceneOpenedSFX;
    private AudioSource audioSource;

    private string levelID;

    void Start()
    {
        // Setup Audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("[HASIL] AudioSource tidak ditemukan. Menambahkan komponen.");
        }

        if (sceneOpenedSFX != null)
        {
            audioSource.PlayOneShot(sceneOpenedSFX);
            Debug.Log("[HASIL] Memainkan suara pembuka scene.");
        }

        // Ambil ID Level
        levelID = PlayerPrefs.GetString("PreviousLevelID", "");
        if (string.IsNullOrEmpty(levelID))
        {
            Debug.LogError("[HASIL] Level ID tidak ditemukan di PlayerPrefs!");
            return;
        }

        // Ambil poin terakhir dari LevelManager
        int rewardPoint = PlayerPrefs.GetInt("LastRewardPoint", 0);

        Debug.Log($"[HASIL] Menampilkan hasil untuk level '{levelID}'. Poin diperoleh: {rewardPoint}");

        // Tampilkan hasil di UI
        ShowResult(rewardPoint);
    }

    public void ShowResult(int rewardPoint)
    {
        if (textSelamat != null)
        {
            textSelamat.text = pesanSelamat;
            textSelamat.alignment = TextAlignmentOptions.Center;
            textSelamat.enableAutoSizing = false;
        }

        if (pointText != null)
        {
            pointText.text = $"Poin Diperoleh: {rewardPoint}";
        }

        if (coin != null) coin.SetActive(true);
        if (point != null) point.SetActive(true);
    }

    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene belum ditentukan!");
        }
    }
}
