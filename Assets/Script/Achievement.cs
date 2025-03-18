using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public int totalPoints = 0; // Total poin pemain
}

public class Achievement : MonoBehaviour
{
    public TextMeshProUGUI pointText;
    public PointManager pointManager;
    private string filePath;
    private PlayerData playerData;
    public GameObject[] achievementIcons; // Ikon achievement yang ditampilkan
    public int[] achievementThresholds = { 300, 600, 900, 1200, 1500 }; // Batas poin

    void Start()
    {
        pointManager = FindObjectOfType<PointManager>();
        filePath = Application.persistentDataPath + "/playerData.json";
        LoadPoints();
        UpdateUI();
        CheckAchievements();
    }

    // ✅ Menambahkan poin dan update tampilan
    public void AddPoints(int amount)
    {
        if (playerData == null) playerData = new PlayerData(); // Pastikan tidak null
        playerData.totalPoints += amount;
        SavePoints();
        UpdateUI();
        CheckAchievements();
    }

    // ✅ Simpan poin ke JSON
    private void SavePoints()
    {
        if (playerData == null) return;
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, json);
    }

    // ✅ Muat poin dari JSON dengan pengecekan null
    private void LoadPoints()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }

        if (playerData == null) // Jika gagal membaca file, buat baru
        {
            playerData = new PlayerData();
            SavePoints();
        }
    }

    // ✅ Update UI untuk menampilkan total poin
    private void UpdateUI()
    {
        if (pointText != null && playerData != null)
        {
            pointText.text = playerData.totalPoints.ToString();
        }
    }

    // ✅ Cek achievement dengan pengecekan aman
    private void CheckAchievements()
    {
        if (achievementIcons == null || achievementIcons.Length == 0)
        {
            Debug.LogWarning("Achievement icons belum diatur!");
            return;
        }

        for (int i = 0; i < achievementThresholds.Length; i++)
        {
            if (i >= achievementIcons.Length) 
            {
                Debug.LogWarning($"Index {i} melebihi jumlah achievementIcons! Pastikan jumlah ikon sesuai dengan threshold.");
                continue;
            }

            if (playerData.totalPoints >= achievementThresholds[i])
            {
                achievementIcons[i].SetActive(true); // Aktifkan achievement
            }
            // else
            // {
            //     achievementIcons[i].SetActive(false); // Sembunyikan achievement
            // }
        }
    }

    // ✅ Fungsi untuk mendapatkan total poin
    public int GetTotalPoints()
    {
        return playerData != null ? playerData.totalPoints : 0;
    }

    void update(){
        pointText.text = "Points: " + pointManager.totalPoints.ToString();
    }
}
