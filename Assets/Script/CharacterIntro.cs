using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public Button characterButton;
    public AudioClip greetingVoice;
    public AudioClip clickVoice;
}

public class CharacterIntro : MonoBehaviour
{
    public List<CharacterData> characters = new List<CharacterData>();
    public float greetingDelay = 1.5f; // Delay antar sapaan karakter

    private AudioSource audioSource;
    private Coroutine currentVoiceCoroutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        foreach (var character in characters)
        {
            CharacterData localChar = character;
            if (character.characterButton != null)
            {
                character.characterButton.onClick.AddListener(() => OnCharacterClicked(localChar));
            }
        }

        if (greetingDelay > 0f)
            StartCoroutine(PlayGreetingsSequentially());
        else
            PlayGreetingsSimultaneously();
    }

    IEnumerator PlayGreetingsSequentially()
    {
        foreach (var character in characters)
        {
            if (character.greetingVoice != null)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(character.greetingVoice);
                yield return new WaitForSeconds(character.greetingVoice.length + greetingDelay);
            }
        }
    }

    void PlayGreetingsSimultaneously()
    {
        foreach (var character in characters)
        {
            if (character.greetingVoice != null)
            {
                audioSource.PlayOneShot(character.greetingVoice);
            }
        }
    }

    void OnCharacterClicked(CharacterData character)
    {
        if (character.clickVoice != null)
        {
            if (currentVoiceCoroutine != null)
            {
                StopCoroutine(currentVoiceCoroutine);
            }

            currentVoiceCoroutine = StartCoroutine(PlayClickVoice(character.clickVoice));
        }
    }

    IEnumerator PlayClickVoice(AudioClip clip)
    {
        audioSource.Stop(); // Hentikan suara sebelumnya
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        currentVoiceCoroutine = null;
    }
}
