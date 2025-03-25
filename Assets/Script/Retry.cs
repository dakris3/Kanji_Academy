using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    private static Stack<string> sceneHistory = new Stack<string>();

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Hindari menambahkan scene "Hasil" ke dalam history jika sudah ada scene sebelumnya
        if (sceneHistory.Count == 0 || sceneHistory.Peek() != currentScene)
        {
            sceneHistory.Push(currentScene);
        }
    }

    public void LoadPreviousScene()
    {
        if (sceneHistory.Count > 1) // Pastikan ada scene sebelumnya
        {
            sceneHistory.Pop(); // Hapus scene saat ini
            string previousScene = sceneHistory.Peek(); // Ambil scene sebelumnya
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("Tidak ada scene sebelumnya yang disimpan!");
        }
    }
}
