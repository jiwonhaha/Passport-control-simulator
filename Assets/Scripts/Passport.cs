using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Passport : MonoBehaviour
{
    XRGrabInteractable interactable;
    NetworkContext context;
    Transform parent;


    // Does this instance of the Component control the transforms for everyone?
    public bool isOwner;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        interactable = GetComponent<XRGrabInteractable>();
        interactable.firstSelectEntered.AddListener(OnPickedUp);
        interactable.lastSelectExited.AddListener(OnDropped);
        context = NetworkScene.Register(this);
        isOwner = false;
    }

    void OnPickedUp(SelectEnterEventArgs ev)
    {
        Debug.Log("Picked up");
        isOwner = true;
    }

    void OnDropped(SelectExitEventArgs ev)
    {
        Debug.Log("Dropped");
        transform.parent = parent;
        isOwner = false;

    }

    private struct PassportMessage
    {
        public Vector3 position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOwner)
        {
            PassportMessage m = new PassportMessage();
            m.position = this.transform.localPosition;
            context.SendJson(m);
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var message = m.FromJson<PassportMessage>();
        if (!isOwner)
        {
            this.transform.localPosition = message.position;
        }
    }
}