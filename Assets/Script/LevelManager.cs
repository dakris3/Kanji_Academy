using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI pointText;         // Menampilkan jumlah poin yang diperoleh
    public TextMeshProUGUI playCountText;     // Menampilkan jumlah penyelesaian level

    private int levelPointReward = 0;         // Poin yang akan diberikan

    private void Start()
    {
        // Hanya jalankan logika ini jika berada di scene "Hasil"
        if (SceneManager.GetActiveScene().name == "Hasil")
        {
            string lastLevel = PlayerPrefs.GetString("LastPlayedLevel", "");

            // Ambil jumlah berapa kali level ini telah diselesaikan
            int levelPlayCount = PlayerPrefs.GetInt($"LevelPlayCount_{lastLevel}", 0);

            // Hitung reward berdasarkan jumlah penyelesaian
            levelPointReward = GetRewardByPlayCount(levelPlayCount);

            // Tambahkan poin ke total poin pemain
            int totalPoints = PlayerPrefs.GetInt("TotalPoints", 0);
            totalPoints += levelPointReward;

            // Simpan informasi ke PlayerPrefs
            PlayerPrefs.SetInt("TotalPoints", totalPoints);
            PlayerPrefs.SetInt($"LevelPlayCount_{lastLevel}", levelPlayCount + 1);
            PlayerPrefs.SetInt("LastRewardPoint", levelPointReward); // Untuk Hasil.cs
            PlayerPrefs.Save();

            // Tampilkan hanya angka poin tanpa teks
            if (pointText != null)
            {
                pointText.text = $"{levelPointReward}";
            }

            // Tampilkan jumlah penyelesaian level secara dinamis
            if (playCountText != null)
            {
                playCountText.text = $"Level telah dimainkan {levelPlayCount + 1}x";
            }

            Debug.Log($"[LEVEL MANAGER] Level '{lastLevel}' dimainkan ke-{levelPlayCount + 1} kali. Dapat {levelPointReward} poin. Total sekarang: {totalPoints}");
        }
    }

    public void LevelComplete()
    {
        // Simpan nama level yang baru saja diselesaikan
        string currentLevel = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastPlayedLevel", currentLevel);
        PlayerPrefs.Save();

        // Pindah ke scene hasil
        SceneManager.LoadScene("Hasil");
    }

    private int GetRewardByPlayCount(int playCount)
    {
        // Sistem reward menurun berdasarkan jumlah penyelesaian
        switch (playCount)
        {
            case 0: return 100; // Pertama kali
            case 1: return 90;
            case 2: return 80;
            case 3: return 70;
            case 4: return 60;
            default: return 50; // Mulai dari penyelesaian ke-6 dan seterusnya
        }
    }
}
