using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Hasil : MonoBehaviour
{
    public string nextSceneName;
    public string levelID = "Level_1"; // Ganti sesuai nama unik level

    public TMP_Text textSelamat;
    public GameObject coin;
    public GameObject point;

    [TextArea]
    public string sudahMainSelamat = "Level telah selesai.\nおつかれさまでした";

    public bool sudahPernahMain = false;

    // Bisa diakses dari skrip lain seperti LevelManager
    public static bool isLevelAlreadyPlayed = false;

    void Start()
    {
        // Cek apakah level sudah pernah dimainkan
        sudahPernahMain = PlayerPrefs.GetInt(levelID, 0) == 1;
        isLevelAlreadyPlayed = sudahPernahMain;

        // Debug log
        Debug.Log("[HASIL] Cek level " + levelID + " - Sudah pernah main: " + sudahPernahMain);

        ShowResult();

        // Tandai sudah main kalau ini pertama kali
        if (!sudahPernahMain)
        {
            PlayerPrefs.SetInt(levelID, 1);
            PlayerPrefs.Save();
        }
    }

    public void ShowResult()
    {
        Debug.Log("Level telah selesai! Menampilkan hasil...");

        if (sudahPernahMain)
        {
            Debug.Log("Level sudah pernah dimainkan, tidak ada poin.");
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
