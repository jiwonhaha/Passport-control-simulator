using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPassport : MonoBehaviour
{
    private GameObject gameSystem;

    private GameObject[] uiElements;
    private GameObject[] answerElements;
    private GameObject[] passports;
    private List<GameObject> instantiatedUIElements = new List<GameObject>();

    [Header("Screen Monitor")]
    [SerializeField] Vector3 inspectorMonitor;
    [SerializeField] Vector3 supervisorMonitor;

    void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("Game System");

        passports = gameSystem.GetComponent<GameSystem>().passports;
        uiElements = gameSystem.GetComponent<GameSystem>().passportScreens;
        answerElements = gameSystem.GetComponent<GameSystem>().passportAnswers;

        // Initially, deactivate all UI elements
        if (uiElements != null)
        {
            foreach (GameObject uiElement in uiElements)
            {
                uiElement.SetActive(false);
            }
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        for (int i = 0; i < passports.Length; i++)
        {
            if (passports[i] == other.gameObject)
            {
                ActivateUIElement(i);
                break;
            }
        }
    }

    void OnCollisionExit(Collision other) 
    {
        // Deactivate all UI elements when the passport is no longer colliding
        foreach (GameObject instantiatedUI in instantiatedUIElements)
        {
            Destroy(instantiatedUI);
        
        }

        instantiatedUIElements.Clear(); // Clear the list after destroying the objects
    }

    private void ActivateUIElement(int index)
    {
        // Deactivate all UI elements before activating the specified one
        foreach (GameObject uiElement in uiElements)
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false);
            }
        }

        // Check if the index is within the range of uiElements array and activate the UI element
        if (index >= 0 && index < uiElements.Length && uiElements[index] != null)
        {
            GameObject selectedUIinspector = uiElements[index];
            GameObject selectedUIsupervisor = answerElements[index];
            selectedUIinspector.SetActive(true);
            selectedUIsupervisor.SetActive(true);

            // Spawn the selected GameObject
            // Inspector monitor
            GameObject instantiatedUI1 = Instantiate(selectedUIinspector, inspectorMonitor, Quaternion.Euler(0, -90, 0));
            // Supervisor monitor
            GameObject instantiatedUI2 = Instantiate(selectedUIsupervisor, supervisorMonitor, Quaternion.Euler(0, 90, 0));
            instantiatedUIElements.Add(instantiatedUI1);
            instantiatedUIElements.Add(instantiatedUI2);

        }
    }
}
