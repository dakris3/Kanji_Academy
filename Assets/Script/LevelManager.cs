using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int levelPointReward = 100; // Poin yang diberikan per level selesai
    public TextMeshProUGUI pointText; // UI untuk menampilkan poin

    private void Start()
    {
        // Cek jika berada di scene "Hasil"
        if (SceneManager.GetActiveScene().name == "Hasil")
        {
            // Tampilkan hanya 100 poin di UI
            if (pointText != null)
            {
                pointText.text = levelPointReward.ToString();
            }
            
            int totalPoints = PlayerPrefs.GetInt("TotalPoints", 0);
            totalPoints += levelPointReward;
            PlayerPrefs.SetInt("TotalPoints", totalPoints);
            PlayerPrefs.Save();

            Debug.Log($"[LEVEL LOADED] Pemain mendapatkan {levelPointReward} poin! Total sekarang: {totalPoints}");
        }
    }

    public void LevelComplete()
    {
        // Pindah ke scene "Hasil"
        SceneManager.LoadScene("Hasil");
    }
}
