using System.Collections;
using System.Collections.Generic;
using System;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameSystem : MonoBehaviour
{
    NetworkContext context;

    GameObject[] players;

    [Header("Game Simulation")]
    [SerializeField] bool isInGame = false;

    [Header("UI Buttons")]
    [SerializeField] Button travellerButton;
    [SerializeField] Button inspectorButton;
    [SerializeField] Button supervisorButton;

    [Header("Object Button")]
    [SerializeField] GameObject inspectorPassButton;
    [SerializeField] GameObject inspectorRejectButton;
    [SerializeField] GameObject supervisorPassButton;
    [SerializeField] GameObject supervisorRejectButton;

    [Header("Scene Objects")]
    [SerializeField] GameObject doorGate;
    [SerializeField] GameObject cage;

    [Header("Passport")]
    [SerializeField] GameObject[] passports;

    [Header("Settings")]
    [SerializeField] int numberOfRounds;
    [SerializeField] int numberOfTravellers;
    [SerializeField] int numberOfInspectors;
    [SerializeField] int numberOfSupervisors;

    private int currentRounds;

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
        context = NetworkScene.Register(this);

#if UNITY_EDITOR
        GameObject.Find("XR Device Simulator").SetActive(true);
#endif
    }

    private void Update()
    {
       
        if (isInGame)
        {

        }

        else
        {
            // Traveller Button
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
            // Supervisor Button
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
            // Inspector Button
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

            //if (currentNumberOfinspector == numberOfInspectors && currentNumberOfsupervisor == numberOfSupervisors && currentNumberOftraveller == numberOfTravellers)
            if (currentNumberOfinspector == numberOfInspectors && currentNumberOftraveller == numberOfTravellers)
            {
                isInGame = true;
                currentRounds = 1;
            }
        }
    }

    private struct ButtonMessage
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

            // Randomly activate one of the four objects
            ActivateRandomObject();

            ButtonMessage m = new ButtonMessage();
            m.totalOftraveller = currentNumberOftraveller;
            m.totalOfsupervisor = currentNumberOfsupervisor;
            m.totalOfinspector = currentNumberOfinspector;
            context.SendJson(m);
        }
            
    }

    private void ActivateRandomObject()
    {

        // Generate a random index between 0 and 3 (since array indices are 0-based)
        int randomIndex = new System.Random().Next(0, passports.Length); // Random.Next is inclusive at the start, exclusive at the end

        // Select the GameObject using the random index
        GameObject selectedPassport = passports[randomIndex];

        // Spawn the selected GameObject
        Instantiate(selectedPassport, new Vector3(7.5f, 0.75f, 30.0f), UnityEngine.Random.rotation);
    }

    public void TagSupervisor()
    {
        if (numberOfSupervisors > currentNumberOfsupervisor)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            players[0].transform.position = GameObject.Find("Marker (Supervisor)").transform.position;

            Debug.Log("Player choose supervisor role!");
            currentNumberOfsupervisor++;

            ButtonMessage m = new ButtonMessage();
            m.totalOftraveller = currentNumberOftraveller;
            m.totalOfsupervisor = currentNumberOfsupervisor;
            m.totalOfinspector = currentNumberOfinspector;
            context.SendJson(m);
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

            ButtonMessage m = new ButtonMessage();
            m.totalOftraveller = currentNumberOftraveller;
            m.totalOfsupervisor = currentNumberOfsupervisor;
            m.totalOfinspector = currentNumberOfinspector;
            context.SendJson(m);
        }
    }

    public void ProcessButtonMessage(ReferenceCountedSceneGraphMessage m)
    {
        var ButtonMessage = m.FromJson<ButtonMessage>();

        currentNumberOftraveller = ButtonMessage.totalOftraveller;
        currentNumberOfinspector = ButtonMessage.totalOfinspector;
        currentNumberOfsupervisor = ButtonMessage.totalOfsupervisor;


    }
}
