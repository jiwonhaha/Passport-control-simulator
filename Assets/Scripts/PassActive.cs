using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Ubiq.Messaging;

public class PassActive : MonoBehaviour
{

    [System.NonSerialized]
    public bool hasChosenChoice = false;

    NetworkContext context;

    [Header("XR Simple Interactable")]
    [SerializeField]
    XRBaseInteractable xrButton; // The XR button or interactable object

    [Header("Main Game Control")]
    [SerializeField] GameSystem gameSystem;

    private void Start()
    {
        context = NetworkScene.Register(this);
        xrButton = this.gameObject.GetComponent<XRSimpleInteractable>();
    }

    private void Update()
    {
        if (gameSystem.isGameReset)
        {
            hasChosenChoice = false;

            Message b = new Message();
            b.isPress = hasChosenChoice;
            context.SendJson(b);
        }
    }

    public void OnEnable()
    {
        // Subscribe to the interactable's events
        xrButton.onSelectEntered.AddListener(HandleSelectEntered);
    }

    public void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or errors when the object is destroyed
        xrButton.onSelectEntered.RemoveListener(HandleSelectEntered);
    }

    private void HandleSelectEntered(XRBaseInteractor interactor)
    {
        if (gameSystem.isInGame)
        {
            hasChosenChoice = true;

            Message b = new Message();
            b.isPress = hasChosenChoice;
            context.SendJson(b);
        }
        
    }

    private struct Message
    {
        public bool isPress;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var message = m.FromJson<Message>();
        if (!hasChosenChoice && message.isPress)
        {
            hasChosenChoice = message.isPress;
        }
    }
}
