using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneAutomatically(string partialName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string scene = System.IO.Path.GetFileNameWithoutExtension(path);

            if (scene.ToLower().Contains(partialName.ToLower()))
            {
                LoadScene(scene);
                return;
            }
        }

        Debug.LogError("Scene dengan nama '" + partialName + "' tidak ditemukan dalam Build Settings!");
    }

    private void LoadScene(string sceneName)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(SceneManager.GetSceneByName(sceneName).buildIndex));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
#else
        SceneManager.LoadScene(sceneName);
#endif
    }
}
