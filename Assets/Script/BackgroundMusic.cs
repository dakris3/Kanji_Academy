using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    [Header("Daftar Musik")]
    public AudioClip[] musicClips; // Masukkan musik dari Inspector
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject); // Hindari duplikasi
        }
    }

    public void PlayMusic(int index)
    {
        if (musicClips == null || index < 0 || index >= musicClips.Length)
        {
            Debug.LogWarning("Index musik tidak valid.");
            return;
        }

        if (audioSource.clip != musicClips[index])
        {
            audioSource.clip = musicClips[index];
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
