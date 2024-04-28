using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharSwitch : MonoBehaviour
{
    public GameObject aliveCat, deadCat, superPositionCat;
    public Vector3 lastKnownPosition;
    public Vector3 currentPos;
    public StateShiftUI state;
    void Start()
    {
        aliveCat.SetActive(true);
        deadCat.SetActive(false);
        superPositionCat.SetActive(false);
        lastKnownPosition = aliveCat.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || StaticVars.superPositionEmpty)
        {
            StaticVars.superPositionEmpty = false;
            ActivateCat(aliveCat);
            state.Switch(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateCat(deadCat);
            state.Switch(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && StaticVars.superPositionTimer > 0)
        {
            ActivateCat(superPositionCat);
            StaticVars.superPositionActive = true;
            state.Switch(3);
        }
        if (aliveCat.activeSelf)
            currentPos = aliveCat.transform.position;
        else if (deadCat.activeSelf)
            currentPos = deadCat.transform.position;
        else if (superPositionCat.activeSelf)
            currentPos = superPositionCat.transform.position;
    }
    public void ActivateCat(GameObject catToActivate)
    {
       if (aliveCat.activeSelf)
            lastKnownPosition = aliveCat.transform.position;
        else if (deadCat.activeSelf)
            lastKnownPosition = deadCat.transform.position;
        else if (superPositionCat.activeSelf)
            lastKnownPosition = superPositionCat.transform.position;

        aliveCat.SetActive(false);
        deadCat.SetActive(false);
        superPositionCat.SetActive(false);

        catToActivate.SetActive(true);
        catToActivate.transform.position = lastKnownPosition;

    }
}
