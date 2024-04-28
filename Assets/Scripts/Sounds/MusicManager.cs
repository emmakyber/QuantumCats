using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip gameWonMusic;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Prevent the music player from being destroyed on load.
        audioSource = GetComponent<AudioSource>();

        // Check if another MusicManager exists
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject); // Destroy this instance if there's another one already
        }
        else
        {
            audioSource.Play(); // Play the initial music if this is the only instance
        }
        // Register to listen for scene loaded events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unregister the scene loaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game Won")
        {
            SetVolume(0.25f); // Set main theme volume to 25%
            PlayLoopedMusic(gameWonMusic, 0.5f); // Start "Game Won" music at 50% volume
        }
    }

    public void PlayLoopedMusic(AudioClip newClip, float volume)
    {
        audioSource.Stop(); // Stop any currently playing music
        audioSource.clip = newClip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play(); // Start the new clip
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}

