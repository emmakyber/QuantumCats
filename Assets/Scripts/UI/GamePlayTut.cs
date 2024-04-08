using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GamePlayTut : MonoBehaviour
{
    public GameObject ControlDescription;
    void Start()
    {
        Time.timeScale = 0f;
        ControlDescription.SetActive(true);
    }

    // Update is called once per frame
    public void toGamePlay()
    {
        ControlDescription.SetActive(false);
        Time.timeScale = 1f;
    }
}
