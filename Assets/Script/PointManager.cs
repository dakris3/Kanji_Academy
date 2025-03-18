using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    public int totalPoints = 0; // Menyimpan total poin
    public TextMeshProUGUI pointText; // UI untuk menampilkan jumlah poin

    void Start()
    {
        LoadPoints(); // Memuat poin yang tersimpan
        UpdateUI();
    }

    // Fungsi untuk menambahkan poin
    public void AddPoints(int amount)
    {
        totalPoints += amount;
        SavePoints(); // Simpan poin ke PlayerPrefs
        UpdateUI();
    }

    // Fungsi untuk menyimpan poin ke PlayerPrefs
    void SavePoints()
    {
        PlayerPrefs.SetInt("TotalPoints", totalPoints);
        PlayerPrefs.Save();
    }

    // Fungsi untuk memuat poin dari PlayerPrefs
    void LoadPoints()
    {
        if (PlayerPrefs.HasKey("TotalPoints"))
        {
            totalPoints = PlayerPrefs.GetInt("TotalPoints");
        }
    }

    // Memperbarui tampilan UI
    void UpdateUI()
    {
        if (pointText != null)
        {
            pointText.text = "+ " + totalPoints.ToString();
        }
    }
}
