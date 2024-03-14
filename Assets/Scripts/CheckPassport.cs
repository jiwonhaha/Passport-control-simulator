using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPassport : MonoBehaviour
{
    [Header("Passport and UI Elements")]
    [SerializeField] private GameObject[] uiElements;
    private List<GameObject> instantiatedUIElements = new List<GameObject>();


    void Start()
    {
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
        string name = other.gameObject.name; 

        if (other.gameObject.tag == "Passport")
        {
            if (name.StartsWith("saudi"))
            {
                
                ActivateUIElement(0);
            }
            else if (name.StartsWith("thailand"))
            {
                ActivateUIElement(1); 
            }
            else if (name.StartsWith("croatia"))
            {
                ActivateUIElement(2); 
            }
            else if (name.StartsWith("brazil"))
            {
                ActivateUIElement(3); 
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
            GameObject selectedUI = uiElements[index];
            selectedUI.SetActive(true);

            // Spawn the selected GameObject
            // Inspector monitor
            GameObject instantiatedUI1 = Instantiate(selectedUI, new Vector3(-5.365f, 1.079f, 28.045f), Quaternion.Euler(0, 0, 0));
            // Supervisor monitor
            GameObject instantiatedUI2 = Instantiate(selectedUI, new Vector3(-5.442f, 1.077f, 31.999f), Quaternion.Euler(0, 180, 0));
            instantiatedUIElements.Add(instantiatedUI1);
            instantiatedUIElements.Add(instantiatedUI2);

        }
    }
}
