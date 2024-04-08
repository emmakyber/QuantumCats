using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialSkip : MonoBehaviour
{
    public GameObject catDescription;
    public GameObject aliveDescription;
    public GameObject deadDescription;
    public GameObject superPositionDescription;

    void Start()
    {
        // Initially deactivate all panels
        DeactivateAll();
        catDescription.SetActive(true);
    }

    public void ActivatePanel(GameObject panelToActivate)
    {
        // Deactivate all panels
        DeactivateAll();

        // Activate the selected panel
        panelToActivate.SetActive(true);
    }

    public void toGamePlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    
    }

    public void DeactivateAll()
    {
        catDescription.SetActive(false);
        aliveDescription.SetActive(false);
        deadDescription.SetActive(false);
        superPositionDescription.SetActive(false);
    }
}
