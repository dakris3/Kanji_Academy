using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    public TextMeshProUGUI pointText; // UI untuk menampilkan jumlah poin
    public int totalPointsDisplay; // Untuk ditampilkan di Inspector
    private int _lastDisplayedPoints = -1;

    void Awake()
    {
        Debug.Log("[POINT MANAGER] Awake called");
        LoadPoints();
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

    public void AddPoints(int amount)
    {
        int currentPoints = PlayerPrefs.GetInt("TotalPoints", 0);
        int newTotal = currentPoints + amount;

        PlayerPrefs.SetInt("TotalPoints", newTotal);
        PlayerPrefs.Save();

        Debug.Log($"[POINT MANAGER] Points added: {amount}, New total: {newTotal}");

        totalPointsDisplay = newTotal;
        ForceUpdateUI();
    }

    private void LoadPoints()
    {
        totalPointsDisplay = PlayerPrefs.GetInt("TotalPoints", 0);
        Debug.Log($"[POINT MANAGER] Loaded points: {totalPointsDisplay}");
    }

    private void ForceUpdateUI()
    {
        if (pointText != null)
        {
            pointText.text = totalPointsDisplay.ToString();
            _lastDisplayedPoints = totalPointsDisplay;
            Debug.Log($"[POINT MANAGER] UI updated with points: {totalPointsDisplay}");
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
            pointText.text = totalPointsDisplay.ToString();
            _lastDisplayedPoints = totalPointsDisplay;
            Debug.Log($"[POINT MANAGER] UI updated with points: {totalPointsDisplay}");
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
