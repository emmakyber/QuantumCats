using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharSwitch : MonoBehaviour
{
    public GameObject aliveCat, deadCat, superPositionCat;
    public Vector3 lastKnownPosition;
    public Vector3 currentPos;
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateCat(aliveCat);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateCat(deadCat);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateCat(superPositionCat);
        }
        if (aliveCat.activeSelf)
            currentPos = aliveCat.transform.position;
        else if (deadCat.activeSelf)
            currentPos = deadCat.transform.position;
        else if (superPositionCat.activeSelf)
            currentPos = superPositionCat.transform.position;
    }
    void ActivateCat(GameObject catToActivate)
    {
       if (aliveCat.activeSelf)
            lastKnownPosition = aliveCat.transform.position;
        else if (deadCat.activeSelf)
            lastKnownPosition = deadCat.transform.position;
        else if (superPositionCat.activeSelf)
            lastKnownPosition = superPositionCat.transform.position;

        // Deactivate all cats
        aliveCat.SetActive(false);
        deadCat.SetActive(false);
        superPositionCat.SetActive(false);

        // Activate the selected cat and apply the last known position
        catToActivate.SetActive(true);
        catToActivate.transform.position = lastKnownPosition;
    }
}
