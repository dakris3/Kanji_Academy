using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public void FinishTutorial()
    {
        if (!string.IsNullOrEmpty(LevelRedirect.targetSceneName))
        {
            SceneManager.LoadScene(LevelRedirect.targetSceneName);
        }
        else
        {
            Debug.LogWarning("Target scene belum diatur!");
        }
    }
}
