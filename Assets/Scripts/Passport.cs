using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Passport : MonoBehaviour
{
    XRGrabInteractable interactable;
    NetworkContext context;
    Transform parent;


    public bool isHolding;
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
        isOwner = true;
        isHolding = true;

        Message m = new Message();
        m.position = this.transform.localPosition;
        m.isHolding = isHolding;
        context.SendJson(m);
    }

    void OnDropped(SelectExitEventArgs ev)
    {
        transform.parent = parent;
        isOwner = false;
        isHolding = false;

        Message m = new Message();
        m.position = this.transform.localPosition;
        m.isHolding = isHolding;
        context.SendJson(m);
        
    }

    private struct Message
    {
        public Vector3 position;
        public bool isHolding;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOwner)
        {
            Message m = new Message();
            m.position = this.transform.localPosition;
            m.isHolding = isHolding;
            context.SendJson(m);
        }

        if (isHolding)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var message = m.FromJson<Message>();
        if (!isOwner)
        {
            this.transform.localPosition = message.position;

            if (message.isHolding != isHolding)
            {
                isHolding = message.isHolding;
            }
        }
    }
}