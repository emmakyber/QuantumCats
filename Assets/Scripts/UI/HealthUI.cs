using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public GameObject[] hearts;
    public Sprite[] decrease;

    
    public void DecreaseHealth()
    {
        if (StaticVars.heartNums >= 0)
            StartCoroutine(AnimateHeartDecrease(StaticVars.heartNums));
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator AnimateHeartDecrease(int heartIndex)
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
        StaticVars.heartNums--;
        if (StaticVars.heartNums < 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
