using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Hasil : MonoBehaviour
{
    public string nextSceneName;

    public TMP_Text textSelamat;
    public GameObject coin;
    public GameObject point;

    [TextArea]
    public string sudahMainSelamat = "Level telah selesai.\nおつかれさまでした";

    public bool sudahPernahMain = false;
    public static bool isLevelAlreadyPlayed = false;

    private string levelID;
    private int rewardPoint = 100;

    void Start()
    {
        levelID = PlayerPrefs.GetString("PreviousLevelID", "");

        if (string.IsNullOrEmpty(levelID))
        {
            Debug.LogError("[HASIL] LevelID tidak ditemukan di PlayerPrefs!");
            return;
        }

        sudahPernahMain = PlayerPrefs.GetInt(levelID, 0) == 1;
        isLevelAlreadyPlayed = sudahPernahMain;

        Debug.Log("[HASIL] Cek level " + levelID + " - Sudah pernah main: " + sudahPernahMain);

        if (!sudahPernahMain)
        {
            // Tambahkan coin
            int currentCoins = PlayerPrefs.GetInt("coins", 0);
            PlayerPrefs.SetInt("coins", currentCoins + rewardPoint);

            // Tambahkan poin ke sistem Achievement
            int totalPoints = PlayerPrefs.GetInt("TotalPoints", 0);
            totalPoints += rewardPoint;
            PlayerPrefs.SetInt("TotalPoints", totalPoints);

            // Simpan bahwa level ini sudah dimainkan
            PlayerPrefs.SetInt(levelID, 1);
            PlayerPrefs.Save();

            Debug.Log($"[HASIL] Poin diberikan untuk level '{levelID}': {rewardPoint}");
            Debug.Log($"[HASIL] Total poin pemain sekarang: {totalPoints}");
        }
        else
        {
            Debug.Log("[HASIL] Level sudah pernah dimainkan, tidak ada poin.");
        }

        ShowResult();
    }

    public void ShowResult()
    {
        Debug.Log("Level telah selesai! Menampilkan hasil...");

        if (sudahPernahMain)
        {
            TampilkanPerubahanJikaSudahMain();
        }
    }

    void TampilkanPerubahanJikaSudahMain()
    {
        if (textSelamat != null)
        {
            textSelamat.text = sudahMainSelamat;
            textSelamat.alignment = TextAlignmentOptions.Center;
            textSelamat.fontSize *= 1.1f;
            textSelamat.enableAutoSizing = false;
        }

        if (coin != null) coin.SetActive(false);
        if (point != null) point.SetActive(false);
    }

    void LoadNextScene()
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
