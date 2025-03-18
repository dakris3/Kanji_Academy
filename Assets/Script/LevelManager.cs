using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PointManager pointManager; // Reference ke PointManager

    void Start()
    {
        if (pointManager == null)
        {
            pointManager = FindObjectOfType<PointManager>();
        }
    }

    public void LevelComplete()
    {
        if (pointManager != null)
        {
            Debug.Log("Level selesai! Menambahkan 100 poin.");
            pointManager.AddPoints(100); // Tambah 100 poin
        }
        else
        {
            Debug.LogWarning("PointManager tidak ditemukan!");
        }
    }
}
