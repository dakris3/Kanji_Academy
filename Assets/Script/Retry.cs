using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public void RetryLevel()
    {
        string previousScene = PlayerPrefs.GetString("PreviousScene", "");

        if (!string.IsNullOrEmpty(previousScene))
        {
            Debug.Log("Retry level: " + previousScene);
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("Previous scene belum disimpan.");
        }
    }
}
