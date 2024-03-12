using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameSystem : MonoBehaviour
{
    NetworkContext context;

    GameObject[] players;

    [Header("Game Simulation")]
    [SerializeField] bool isInGame = false;
    private bool prevIsInGame = false;

    [Header("UI Buttons")]
    [SerializeField] Button travellerButton;
    [SerializeField] Button inspectorButton;
    [SerializeField] Button supervisorButton;

    [Header("Object Button")]
    [SerializeField] GameObject inspectorPassButton;
    [SerializeField] GameObject inspectorRejectButton;
    [SerializeField] GameObject supervisorPassButton;
    [SerializeField] GameObject supervisorRejectButton;

    [Header("Settings")]
    [SerializeField] int numberOfRounds;

    [Header("Maximum number of each role")]
    [SerializeField] int numberOfTravellers;
    [SerializeField] int numberOfInspectors;
    [SerializeField] int numberOfSupervisors;

    int currentNumberOftraveller = 0;
    int currentNumberOfinspector = 0;
    int currentNumberOfsupervisor = 0;

    private void Awake()
    {
        travellerButton.onClick.AddListener(TagTraveller);
        inspectorButton.onClick.AddListener(TagInspector);
        supervisorButton.onClick.AddListener(TagSupervisor);
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players[0].GetComponent<CapsuleCollider>() == null)
        {
            CapsuleCollider _ = players[0].AddComponent<CapsuleCollider>();
        }
        context = NetworkScene.Register(this);
    }

    private void Update()
    {
        if (currentNumberOftraveller == numberOfTravellers)
        {
            ColorBlock colors = travellerButton.colors;
            colors.disabledColor = Color.red; // Set the disabled color to red
            travellerButton.colors = colors;

            travellerButton.interactable = false;
        }
        else
        {
            ColorBlock colors = travellerButton.colors;
            colors.disabledColor = Color.green;
            travellerButton.colors = colors;

            travellerButton.interactable = true;
        }

        if (currentNumberOfsupervisor == numberOfSupervisors)
        {
            ColorBlock colors = supervisorButton.colors;
            colors.disabledColor = Color.red; // Set the disabled color to red
            supervisorButton.colors = colors;

            supervisorButton.interactable = false;
        }
        else
        {
            ColorBlock colors = supervisorButton.colors;
            colors.disabledColor = Color.green;
            supervisorButton.colors = colors;

            supervisorButton.interactable = true;
        }

        if (currentNumberOfinspector == numberOfInspectors)
        {
            ColorBlock colors = inspectorButton.colors;
            colors.disabledColor = Color.red; // Set the disabled color to red
            inspectorButton.colors = colors;

            inspectorButton.interactable = false;
        }
        else
        {
            ColorBlock colors = inspectorButton.colors;
            colors.disabledColor = Color.green;
            inspectorButton.colors = colors;

            inspectorButton.interactable = true;
        }

        Message m = new Message();
        m.totalOftraveller = currentNumberOftraveller;
        m.totalOfsupervisor = currentNumberOfsupervisor;
        m.totalOfinspector = currentNumberOfinspector;
        context.SendJson(m);
    }

    private struct Message
    {
        public int totalOftraveller;
        public int totalOfinspector;
        public int totalOfsupervisor;
    }

    public void TagTraveller()
    {
        if (numberOfTravellers > currentNumberOftraveller)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            players[0].transform.position = GameObject.Find("Marker (Traveller)").transform.position;

            Debug.Log("Player choose traveller role!");
            currentNumberOftraveller++;
        }
            
    }

    public void TagSupervisor()
    {
        if (numberOfSupervisors > currentNumberOfsupervisor)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            players[0].transform.position = GameObject.Find("Marker (Supervisor)").transform.position;

            Debug.Log("Player choose supervisor role!");
            currentNumberOfsupervisor++;
        }
    }

    public void TagInspector()
    {
        if (numberOfInspectors > currentNumberOfinspector)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            players[0].transform.position = GameObject.Find("Marker (Inspector)").transform.position;

            Debug.Log("Player choose inspector role!");
            currentNumberOfinspector++;
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var message = m.FromJson<Message>();

        if (currentNumberOftraveller != message.totalOftraveller)
        {
            currentNumberOftraveller = message.totalOftraveller;
        }
        if (currentNumberOfinspector != message.totalOfinspector)
        {
            currentNumberOfinspector = message.totalOfinspector;
        }
        if (currentNumberOfsupervisor != message.totalOfsupervisor)
        {
            currentNumberOfsupervisor = message.totalOfsupervisor;
        }
    }
}
