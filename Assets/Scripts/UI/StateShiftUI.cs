using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StateShiftUI : MonoBehaviour
{
    public GameObject stateUI;
    public Sprite[] aliveSprites, deadSprites, superSprites;

    // Variables to maintain state across coroutine calls
    private int superIndex = 0;
    public GameObject background;
    private int superQuarterIndex = 0;
    private float superQuarterTimer = 6.0f; // Initialize to full quarter duration

    void Start()
    {
        StartCoroutine(AliveUI());
    }

    public void Switch(float state)
    {
        Image stateImage = stateUI.GetComponent<Image>();
        StopAllCoroutines();

        if (state == 1f)
        {
            background.GetComponent<Image>().color = new Color(255, 255, 255, 0.25f);
            StartCoroutine(AliveUI());
        }
        else if (state == 2f)
        {
            background.GetComponent<Image>().color = new Color(255, 255, 255, 0.25f);
            StartCoroutine(DeadUI());
        }
        else if (state == 3f)
        {
            background.GetComponent<Image>().color = new Color(255, 0, 0, 0.25f);
            StartCoroutine(SuperUI(superIndex, superQuarterIndex, superQuarterTimer));
        }
    }

    IEnumerator AliveUI()
    {
        int index = 0;
        while (true)
        {
            stateUI.GetComponent<Image>().sprite = aliveSprites[index];
            index = (index + 1) % aliveSprites.Length;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator DeadUI()
    {
        int index = 0;
        while (true)
        {
            stateUI.GetComponent<Image>().sprite = deadSprites[index];
            index = (index + 1) % deadSprites.Length;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SuperUI(int index, int quarterIndex, float quarterTimer)
    {
        float spriteChangeInterval = 0.5f;
        int spritesPerQuarter = 3;
        float totalTimer = StaticVars.superPositionTimer;

        while (totalTimer > 0)
        {
            stateUI.GetComponent<Image>().sprite = superSprites[index];
            yield return new WaitForSeconds(spriteChangeInterval);

            quarterTimer -= spriteChangeInterval;
            totalTimer -= spriteChangeInterval;

            if (quarterTimer <= 0)
            {
                quarterTimer = 6.0f;
                quarterIndex = (quarterIndex + 1) % 4;
                index = quarterIndex * spritesPerQuarter;
            }
            else
            {
                index++;
                if ((index - quarterIndex * spritesPerQuarter) >= spritesPerQuarter)
                {
                    index = quarterIndex * spritesPerQuarter;
                }
            }

            // Save the state
            superIndex = index;
            superQuarterIndex = quarterIndex;
            superQuarterTimer = quarterTimer;
        }
        StaticVars.superPositionActive = false;
        StaticVars.superPositionEmpty = true;
        StartCoroutine(AliveUI());
    }
}
