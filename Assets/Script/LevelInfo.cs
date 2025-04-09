using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfo : MonoBehaviour
{
    public string levelID = "Level_1";

    private void Start()
    {
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetString("PreviousLevelID", levelID);
        PlayerPrefs.Save();
    }
}
