using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int levelPointReward = 100; // Poin yang diberikan per level selesai
    public TextMeshProUGUI pointText;  // UI untuk menampilkan poin
    public bool allowReward = true;    // Default-nya true, nanti diatur otomatis

    private void Start()
    {
        // Cek jika berada di scene "Hasil"
        if (SceneManager.GetActiveScene().name == "Hasil")
        {
            // Ambil nama level terakhir yang disimpan sebelum load scene "Hasil"
            string lastLevel = PlayerPrefs.GetString("LastPlayedLevel", "");

            // Cek apakah level itu sudah pernah dimainkan
            if (PlayerPrefs.GetInt($"LevelPlayed_{lastLevel}", 0) == 1)
            {
                allowReward = false;
            }

            // Tampilkan poin ke UI
            if (pointText != null)
            {
                pointText.text = levelPointReward.ToString();
            }

            // Beri reward jika diperbolehkan
            if (allowReward)
            {
                int totalPoints = PlayerPrefs.GetInt("TotalPoints", 0);
                totalPoints += levelPointReward;
                PlayerPrefs.SetInt("TotalPoints", totalPoints);
                PlayerPrefs.SetInt($"LevelPlayed_{lastLevel}", 1); // Tandai sudah dimainkan
                PlayerPrefs.Save();

                Debug.Log($"[LEVEL LOADED] Pemain mendapatkan {levelPointReward} poin! Total sekarang: {totalPoints}");
            }
            else
            {
                Debug.Log("[LEVEL LOADED] Tidak memberi poin karena level sudah pernah dimainkan.");
            }
        }
    }

    public void LevelComplete()
    {
        // Ambil nama level dari scene saat ini
        string currentLevel = SceneManager.GetActiveScene().name;

        // Simpan nama level untuk pengecekan di scene "Hasil"
        PlayerPrefs.SetString("LastPlayedLevel", currentLevel);
        PlayerPrefs.Save();

        // Pindah ke scene "Hasil"
        SceneManager.LoadScene("Hasil");
    }
}
