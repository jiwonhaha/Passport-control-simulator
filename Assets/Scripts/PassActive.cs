using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Ubiq.Messaging;

public class PassActive : MonoBehaviour
{
    public XRBaseInteractable xrButton; // The XR button or interactable object

    public bool hasChosenChoice = false;

    NetworkContext context;

    [Header("Main Game Control")]
    [SerializeField] GameSystem gameSystem;

    private void Start()
    {
        context = NetworkScene.Register(this);
    }

    private void Update()
    {
        if (gameSystem.isGameReset)
        {
            hasChosenChoice = false;

            ButtonPress b = new ButtonPress();
            b.isPress = hasChosenChoice;
            context.SendJson(b);
        }
    }

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
        hasChosenChoice = true;

        ButtonPress b = new ButtonPress();
        b.isPress = hasChosenChoice;
        context.SendJson(b);
    }

    private struct ButtonPress
    {
        public bool isPress;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var message = m.FromJson<ButtonPress>();
        if (!hasChosenChoice && message.isPress)
        {
            hasChosenChoice = message.isPress;
        }
    }
}
