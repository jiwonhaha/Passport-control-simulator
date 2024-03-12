using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PassActive : MonoBehaviour
{
    public XRBaseInteractable xrButton; // The XR button or interactable object
    public GameObject objectToEnable; // The GameObject to enable when the button is interacted with

    void OnEnable()
    {
        // Subscribe to the interactable's events
        xrButton.onSelectEntered.AddListener(HandleSelectEntered);
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or errors when the object is destroyed
        xrButton.onSelectEntered.RemoveListener(HandleSelectEntered);
    }

    private void HandleSelectEntered(XRBaseInteractor interactor)
    {
        // Action to perform when the XR button is interacted with
        objectToEnable.SetActive(false);
    }
}
