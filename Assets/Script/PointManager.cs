using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    public TextMeshProUGUI pointText; // UI untuk menampilkan jumlah poin
    public int totalPointsDisplay; // Serialized field to display TotalPoints in Inspector
    
    private int _lastDisplayedPoints = -1; // Track the last displayed points value

    void Awake()
    {
        Debug.Log("[POINT MANAGER] Awake called");
        LoadPoints(); // Memuat poin sebelum Start
    }

    void Start()
    {
        Debug.Log("[POINT MANAGER] Start called");
        
        // Check if pointText is assigned
        if (pointText == null)
        {
            Debug.LogError("[POINT MANAGER] pointText not assigned in Inspector!");
        }
        else
        {
            Debug.Log($"[POINT MANAGER] pointText reference found: {pointText.name}");
        }
        
        // Force UI update in Start
        ForceUpdateUI();
    }

    void Update()
    {
        // Keep the Inspector value synced with PlayerPrefs
        totalPointsDisplay = GetTotalPoints();
        
        // Check if points changed since last UI update
        if (_lastDisplayedPoints != totalPointsDisplay)
        {
            UpdateUI();
        }
    }

    // Fungsi untuk menambahkan poin
    public void AddPoints(int amount)
    {
        int currentPoints = PlayerPrefs.GetInt("TotalPoints", 0);
        int newTotal = currentPoints + amount;
        
        PlayerPrefs.SetInt("TotalPoints", newTotal);
        PlayerPrefs.Save();
        
        Debug.Log($"[POINT MANAGER] Points added: {amount}, New total: {newTotal}");
        
        // Update totalPointsDisplay for immediate feedback
        totalPointsDisplay = newTotal;
        
        // Force UI update immediately
        ForceUpdateUI();
    }

    // Ambil total poin dari PlayerPrefs
    private void LoadPoints()
    {
        int loadedPoints = 0;
        
        if (PlayerPrefs.HasKey("TotalPoints"))
        {
            loadedPoints = PlayerPrefs.GetInt("TotalPoints");
            Debug.Log($"[POINT MANAGER] Loaded existing points: {loadedPoints}");
        }
        else
        {
            PlayerPrefs.SetInt("TotalPoints", 0);
            PlayerPrefs.Save();
            Debug.Log("[POINT MANAGER] No saved points found, initialized to 0");
        }
        
        totalPointsDisplay = loadedPoints;
    }

    // Force UI update regardless of conditions
    private void ForceUpdateUI()
    {
        if (pointText != null)
        {
            int currentPoints = GetTotalPoints();
            pointText.text = currentPoints.ToString();
            _lastDisplayedPoints = currentPoints;
            
            Debug.Log($"[POINT MANAGER] UI FORCE UPDATED with points: {currentPoints}");
        }
        else
        {
            Debug.LogError("[POINT MANAGER] Cannot update UI - pointText reference is null!");
        }
    }

    // Fungsi untuk mengupdate UI
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

    // Reset poin (bisa dipanggil saat debugging atau fitur reset)
    public void ResetPoints()
    {
        PlayerPrefs.SetInt("TotalPoints", 0);
        PlayerPrefs.Save();
        totalPointsDisplay = 0;
        ForceUpdateUI();
        Debug.Log("[POINT MANAGER] All points have been reset to 0");
    }

    // Get total points from PlayerPrefs
    public int GetTotalPoints()
    {
        return PlayerPrefs.GetInt("TotalPoints", 0);
    }
    
    // This ensures the text is properly updated even after scene transitions
    void OnEnable()
    {
        Debug.Log("[POINT MANAGER] OnEnable called");
        // Slight delay to ensure TextMeshProUGUI is fully initialized
        Invoke("ForceUpdateUI", 0.1f);
    }
}