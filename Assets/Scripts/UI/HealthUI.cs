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
        Debug.Log("heart decreasing index is " + StaticVars.heartNums);
        if (StaticVars.heartNums >= 0)
            StartCoroutine(AnimateHeartDecrease(StaticVars.heartNums));
    }

    IEnumerator AnimateHeartDecrease(int heartIndex)
    {
        Image heartImage = hearts[heartIndex].GetComponent<Image>();
        Debug.Log("heart index " + heartIndex);
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
         {
            StaticVars.heartNums = 4;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
         }
    }
}
