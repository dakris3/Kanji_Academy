using UnityEngine;
using UnityEngine.SceneManagement;

public class Hasil : MonoBehaviour
{
    public string nextSceneName; // Nama scene berikutnya (atur di Inspector)

    public void ShowResult()
    {
        Debug.Log("Level telah selesai! Menampilkan hasil...");

        // Di sini kamu bisa tambahkan UI hasil, misalnya skor atau pesan
        // Contoh sederhana:
        Debug.Log("Selamat! Anda telah menyelesaikan kuis.");

        // Jika ingin pindah scene setelah beberapa detik
        Invoke("LoadNextScene", 0.5f); // Tunggu 1 detik sebelum pindah scene
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log("Mengganti scene ke: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene belum ditentukan. Atur di Inspector!");
        }
    }
}
