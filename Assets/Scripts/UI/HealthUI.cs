using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public GameObject[] hearts;
    public Sprite[] decrease;
    private bool playingRoutine = false;

    private int numberOfHearts = 4;


    public void DecreaseHealth()
    {
        // if (playingRoutine)
        //     StopAllCoroutines();
        if (StaticVars.heartNums >= 0)
        {
            StartCoroutine(AnimateHeartDecrease(StaticVars.heartNums));
        }
    }

    IEnumerator AnimateHeartDecrease(int heartIndex)
    {
        Debug.Log("number of hearts: " + numberOfHearts);
        Debug.Log("heart index: " + heartIndex);
        playingRoutine = true;
        while (numberOfHearts >= heartIndex)
        {
            Image heartImage = hearts[heartIndex].GetComponent<Image>();
            if (heartImage != null)
            {
                float duration = 2f;
                float timePerFrame = duration / decrease.Length;
                foreach (var frame in decrease)
                {
                    heartImage.sprite = frame;
                    yield return new WaitForSeconds(timePerFrame);
                }
            }
            numberOfHearts--;
        }
        if (numberOfHearts < 0)
        {
            StaticVars.heartNums = 4;
            numberOfHearts = 4;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        playingRoutine = false;
    }
}
