using UnityEngine;
using TMPro;

[System.Serializable]
public class PlayerData
{
    public int totalPoints = 0;
}

public class Achievement : MonoBehaviour
{
    public GameObject[] achievementIcons;
    public int[] achievementThresholds;

    [SerializeField] // Expose this field in the Inspector
    private int _totalPointsDisplay; // Backing field for the property
    
    private bool _initialized = false;
    private int _cachedPoints = 0; // Cache the points in memory

    // Property to sync with PlayerPrefs
    public int TotalPointsDisplay
    {
        get 
        {
            // Only load from PlayerPrefs during initialization
            if (!_initialized)
            {
                _cachedPoints = PlayerPrefs.GetInt("TotalPoints", 0);
                Debug.Log($"[INITIAL LOAD] Points loaded from PlayerPrefs: {_cachedPoints}");
                _initialized = true;
            }
            return _cachedPoints;
        }
        set
        {
            // Only save if value actually changes
            if (_cachedPoints != value)
            {
                _cachedPoints = value;
                PlayerPrefs.SetInt("TotalPoints", value);
                PlayerPrefs.Save(); // Ensure the value is written to disk immediately
                Debug.Log($"[SAVE POINTS] Points saved to PlayerPrefs: {value}");
                _totalPointsDisplay = value; // Sync the Inspector field
                CheckAchievements(); // Update achievements when points change
            }
        }
    }

    void Awake()
    {
        // Force initialization in Awake
        int points = TotalPointsDisplay;
        Debug.Log($"[AWAKE] Initial points loaded: {points}");
    }

    void Start()
    {
        // Initialize the Inspector field with the value from PlayerPrefs
        _totalPointsDisplay = TotalPointsDisplay;
        Debug.Log($"[START] Using loaded points: {_totalPointsDisplay}");

        ValidateSetup();
        CheckAchievements();
    }

    void OnValidate()
    {
        // When the value is changed in the Inspector, update PlayerPrefs
        if (Application.isPlaying)
        {
            TotalPointsDisplay = _totalPointsDisplay;
        }
    }

    // Manual method to force a reload from PlayerPrefs
    public void ReloadFromPlayerPrefs()
    {
        _initialized = false; // Reset initialization flag
        _cachedPoints = TotalPointsDisplay; // This will trigger a reload
        _totalPointsDisplay = _cachedPoints;
        Debug.Log($"[FORCE RELOAD] Reloaded points from PlayerPrefs: {_cachedPoints}");
    }

    void Update()
    {
        // Sync the Inspector field with the cached value
        if (_totalPointsDisplay != _cachedPoints)
        {
            Debug.Log($"[SYNC] Points value changed in Inspector: {_totalPointsDisplay} -> {_cachedPoints}");
            TotalPointsDisplay = _totalPointsDisplay; // This will save to PlayerPrefs if different
        }
    }

    public void AddAchievementPoints(int amount)
    {
        Debug.Log($"[ADD POINTS] Before adding: {TotalPointsDisplay}");
        TotalPointsDisplay += amount; // Use the property to update points
        Debug.Log($"[ADD POINTS] Points added: {amount}, New total: {TotalPointsDisplay}");
    }

    private void CheckAchievements()
    {
        if (achievementIcons == null || achievementThresholds == null) return;

        int currentPoints = TotalPointsDisplay; 

        for (int i = 0; i < achievementThresholds.Length; i++)
        {
            if (i >= achievementIcons.Length || achievementIcons[i] == null)
            {
                Debug.LogWarning($"[ACHIEVEMENT] Achievement {i} tidak diatur dengan benar!");
                continue;
            }

            bool isAchieved = currentPoints >= achievementThresholds[i];

            // Aktifkan ikon jika belum aktif
            if (isAchieved && !achievementIcons[i].activeSelf)
            {
                achievementIcons[i].SetActive(true);
                Debug.Log($"[ACHIEVEMENT] Achievement {i} AKTIF (poin: {currentPoints} / {achievementThresholds[i]})");
            }
        }
    }

    private void ValidateSetup()
    {
        if (achievementIcons == null || achievementIcons.Length == 0)
        {
            Debug.LogError("[ERROR] Achievement icons belum diatur di Inspector!");
        }

        if (achievementThresholds == null || achievementThresholds.Length == 0)
        {
            Debug.LogError("[ERROR] Achievement thresholds belum diatur di Inspector!");
        }

        if (achievementIcons.Length != achievementThresholds.Length)
        {
            Debug.LogError("[ERROR] Jumlah achievementIcons dan achievementThresholds tidak sama!");
        }
    }

    public int GetTotalPoints()
    {
        return TotalPointsDisplay;
    }

    // Fungsi baru untuk menerima poin dari LevelManager
    public void AddPointsFromLevel(int points)
    {
        AddAchievementPoints(points);
    }

    // If your game has a way to reset progress, you can call this
    public void ResetPoints()
    {
        TotalPointsDisplay = 0;
        Debug.Log("[RESET] Points reset to 0 and saved to PlayerPrefs");
    }

    // Make sure to save when the game quits
    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
        Debug.Log("[QUIT] Saving points to PlayerPrefs before quitting");
    }
}