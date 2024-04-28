using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Prevent the music player from being destroyed on load.

        // Check if another MusicManager exists
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject); // Destroy this if there's another one already (prevent duplicates)
        }
        else
        {
            GetComponent<AudioSource>().Play(); // Play music if this is the only instance
        }
    }
}