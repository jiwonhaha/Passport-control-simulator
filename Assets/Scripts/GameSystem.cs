using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    GameObject avatarManager;

    GameObject[] players;

    [Header("Game Simulation")]
    [SerializeField] bool isInGame = false;
    private bool prevIsInGame = false;

    [Header("UI Buttons")]
    [SerializeField] Button travellerButton;
    [SerializeField] Button inspectorButton;
    [SerializeField] Button supervisorButton;
    [SerializeField] Button startButton;

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

        startButton.onClick.AddListener(RoundBegin);
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players[0].GetComponent<CapsuleCollider>() == null)
        {
            CapsuleCollider _ = players[0].AddComponent<CapsuleCollider>();
        }
    }

    private void Update()
    {
        if (!isInGame)
        {

            if (currentNumberOftraveller == numberOfTravellers && currentNumberOfinspector == numberOfInspectors && currentNumberOfsupervisor == numberOfSupervisors)
            {
                isInGame = true;
            }
        }
        else
        {
            if (prevIsInGame != isInGame)
            {
                prevIsInGame = isInGame;
            }
        }
    }

    public void TagTraveller()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (numberOfTravellers > currentNumberOftraveller)
        {
            players[0].transform.position = GameObject.Find("Marker (Traveller)").transform.position;
            Debug.Log("Player choose traveller role!");
            currentNumberOftraveller++;
        }
            
    }

    public void TagSupervisor()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (numberOfSupervisors > currentNumberOfsupervisor)
        {
            players[0].transform.position = GameObject.Find("Marker (Supervisor)").transform.position;
            Debug.Log("Player choose supervisor role!");
            currentNumberOfsupervisor++;
        }
    }

    public void TagInspector()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (numberOfInspectors > currentNumberOfinspector)
        {
            players[0].transform.position = GameObject.Find("Marker (Inspector)").transform.position;
            Debug.Log("Player choose inspector role!");
            currentNumberOfinspector++;
        }
    }

    public void RoundBegin()
    {
        
    }
}
