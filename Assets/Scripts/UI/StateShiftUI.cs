using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StateShiftUI : MonoBehaviour
{
    public GameObject stateUI;
    public Sprite alive, dead, super;

    public void Switch(float state)
    {
        Image stateImage = stateUI.GetComponent<Image>();

        if (state == 1f)
        {
            stateImage.sprite = alive;
        }
        else if (state == 2f)
        {
            stateImage.sprite = dead;
        }
        else if (state == 3f)
        {
            stateImage.sprite = super;
        }
    }
}
