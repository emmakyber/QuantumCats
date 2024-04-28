using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    void Start()
    {
        // Deactivate the pause menu initially
        pauseMenu.SetActive(false);
    }

    public void TogglePauseMenuOn()
    {
        Debug.Log("Pause Menu Toggled");
        // Toggle the visibility of the pause menu
        pauseMenu.SetActive(true);

        // Pause or resume the game time based on the menu's visibility
        Time.timeScale = pauseMenu.activeSelf ? 0f : 1f;
    }

    public void TogglePauseMenuOff()
    {
        // Hide the pause menu
        pauseMenu.SetActive(false);

        // Resume the game time
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        SceneManager.LoadScene("Home Screen");
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void Play()
    {
        if (StaticVars.restarted)
        {
            SceneManager.LoadScene("Level 1 - Jungle");
            StaticVars.level = 1;
            Time.timeScale = 1f;
        }
        else
        {
            SceneManager.LoadScene("Loading");
            Time.timeScale = 1f;
        }
        
    }

    public void Quit()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }

    public void RestartGame()
    {
        StaticVars.Reset();
        SceneManager.LoadScene("Home Screen");
        Time.timeScale = 1f;
    }
}
