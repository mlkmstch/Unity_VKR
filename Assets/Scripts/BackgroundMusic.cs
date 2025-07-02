using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public AudioMixerGroup musicMixerGroup; // Группа микшера Music
    private AudioSource audioSource;

    private void Awake()
    {
        if (FindObjectsOfType<BackgroundMusic>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;

        audioSource.outputAudioMixerGroup = musicMixerGroup;

        audioSource.Play();
    }
}
