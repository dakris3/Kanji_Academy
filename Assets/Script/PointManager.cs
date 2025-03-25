using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    public TextMeshProUGUI pointText; // UI untuk menampilkan jumlah poin
    public int totalPointsDisplay; // Serialized field to display TotalPoints in Inspector
    private int _lastDisplayedPoints = -1; // Track the last displayed points value
    private const int MAX_POINTS = 3100; // Batas maksimal poin

    void Awake()
    {
        Debug.Log("[POINT MANAGER] Awake called");
        LoadPoints(); // Memuat poin sebelum Start
    }

    void Start()
    {
        Debug.Log("[POINT MANAGER] Start called");

        if (pointText == null)
        {
            Debug.LogError("[POINT MANAGER] pointText not assigned in Inspector!");
        }
        else
        {
            Debug.Log($"[POINT MANAGER] pointText reference found: {pointText.name}");
        }

        ForceUpdateUI();
    }

    void Update()
    {
        totalPointsDisplay = GetTotalPoints();

        if (_lastDisplayedPoints != totalPointsDisplay)
        {
            UpdateUI();
        }
    }

    // Fungsi untuk menambahkan poin dengan batas maksimal 3100
    public void AddPoints(int amount)
    {
        int currentPoints = PlayerPrefs.GetInt("TotalPoints", 0);
        
        // Cek apakah poin yang baru akan melebihi batas maksimal
        if (currentPoints >= MAX_POINTS)
        {
            Debug.LogWarning("[POINT MANAGER] Poin sudah mencapai batas maksimal!");
            return;
        }

        int newTotal = Mathf.Min(currentPoints + amount, MAX_POINTS);
        
        PlayerPrefs.SetInt("TotalPoints", newTotal);
        PlayerPrefs.Save();

        Debug.Log($"[POINT MANAGER] Points added: {amount}, New total: {newTotal}");

        totalPointsDisplay = newTotal;
        ForceUpdateUI();
    }

    private void LoadPoints()
    {
        int loadedPoints = PlayerPrefs.GetInt("TotalPoints", 0);
        if (loadedPoints > MAX_POINTS)
        {
            loadedPoints = MAX_POINTS;
            PlayerPrefs.SetInt("TotalPoints", MAX_POINTS);
            PlayerPrefs.Save();
        }

        totalPointsDisplay = loadedPoints;
        Debug.Log($"[POINT MANAGER] Loaded points: {loadedPoints}");
    }

    private void ForceUpdateUI()
    {
        if (pointText != null)
        {
            int currentPoints = GetTotalPoints();
            pointText.text = currentPoints.ToString();
            _lastDisplayedPoints = currentPoints;
            Debug.Log($"[POINT MANAGER] UI updated with points: {currentPoints}");
        }
        else
        {
            Debug.LogError("[POINT MANAGER] Cannot update UI - pointText reference is null!");
        }
    }

    private void UpdateUI()
    {
        if (pointText != null)
        {
            int currentPoints = GetTotalPoints();
            pointText.text = currentPoints.ToString();
            _lastDisplayedPoints = currentPoints;
            Debug.Log($"[POINT MANAGER] UI updated with points: {currentPoints}");
        }
        else
        {
            Debug.LogWarning("[POINT MANAGER] pointText not assigned - UI update skipped");
        }
    }

    public void ResetPoints()
    {
        PlayerPrefs.SetInt("TotalPoints", 0);
        PlayerPrefs.Save();
        totalPointsDisplay = 0;
        ForceUpdateUI();
        Debug.Log("[POINT MANAGER] All points have been reset to 0");
    }

    public int GetTotalPoints()
    {
        return PlayerPrefs.GetInt("TotalPoints", 0);
    }

    void OnEnable()
    {
        Debug.Log("[POINT MANAGER] OnEnable called");
        Invoke("ForceUpdateUI", 0.1f);
    }
}
