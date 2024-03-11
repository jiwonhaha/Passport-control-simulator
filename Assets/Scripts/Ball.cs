using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ball : MonoBehaviour
{
    XRGrabInteractable interactable;
    NetworkContext context;
    Transform parent;

    // The token decides who has priority when two messages conflict. The higher
    // one wins.
    public int token;

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
        token = Random.Range(1, 10000);
        isOwner = true; // Start by both exchanging the random tokens to see who wins...
    }

    void OnPickedUp(SelectEnterEventArgs ev)
    {
        Debug.Log("Picked up");
        TakeOwnership();
    }

    void OnDropped(SelectExitEventArgs ev)
    {
        Debug.Log("Dropped");
        transform.parent = parent;
        GetComponent<Rigidbody>().isKinematic = false;

    }

    private struct Message
    {
        public Vector3 position;
        public int token;
    }

    // When a Component Instance takes Ownership, that Peer decides the position
    // for everyone, either through the VR Controller or through its local Physics
    // Engine
    void TakeOwnership()
    {
        token++;
        isOwner = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isOwner)
        {
            Message m = new Message();
            m.position = this.transform.localPosition;
            m.token = token;
            context.SendJson(m);
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var message = m.FromJson<Message>();
        transform.localPosition = message.position;
        if(message.token > token)
        {
            isOwner = false;
            token = message.token;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        Debug.Log(gameObject.name + " Updated");
    }
}
