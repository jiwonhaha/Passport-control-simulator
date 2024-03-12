using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    private GameObject[] traveller;
    private GameObject[] inspector;
    private GameObject[] supervisor;

    GameObject avatarManager;

    GameObject[] player;

    [Header("Game Simulation")]
    [SerializeField] bool isInGame = false;

    [Header("Role Buttons")]
    [SerializeField] Button travellerButton;
    [SerializeField] Button inspectorButton;
    [SerializeField] Button supervisorButton;

    [Header("Setting Buttons")]
    [SerializeField] Button startButton;

    private void Awake()
    {
        travellerButton.onClick.AddListener(TagTraveller);
        inspectorButton.onClick.AddListener(TagInspector);
        supervisorButton.onClick.AddListener(TagSupervisor);

        startButton.onClick.AddListener(RoundBegin);
    }

    private void Update()
    {
        if (!isInGame)
        {
            avatarManager = GameObject.Find("Avatar Manager");
            if (avatarManager != null)
            {
                Transform parentTransform = avatarManager.GetComponent<Transform>();

                foreach (Transform childTransform in parentTransform)
                {
                    GameObject childGameObject = childTransform.gameObject;
                    if (!(childGameObject.CompareTag("Player") || childGameObject.CompareTag("Supervisor") || childGameObject.CompareTag("Traveller") || childGameObject.CompareTag("Inspector")))
                    {
                        childGameObject.gameObject.tag = "Player";
                    }    
                    if (childGameObject.GetComponent<PlayerRoleAssign>() == null)
                    {
                        PlayerRoleAssign _ = childGameObject.AddComponent<PlayerRoleAssign>();
                    }
                        
                }
            }
        }
    }

    public void TagTraveller()
    {
        Debug.Log("Player choose traveller role!");

        player = GameObject.FindGameObjectsWithTag("Player");
        player[0].transform.position = GameObject.Find("Marker (Traveller)").transform.position;
    }

    public void TagSupervisor()
    {
        Debug.Log("Player choose supervisor role!");
    }

    public void TagInspector()
    {
        Debug.Log("Player choose inspector role!");
    }

    public void RoundBegin()
    {
        traveller = GameObject.FindGameObjectsWithTag("Traveller");
        inspector = GameObject.FindGameObjectsWithTag("Inspector");
        supervisor = GameObject.FindGameObjectsWithTag("Supervisor");


        if (traveller.Length == 1 && inspector.Length == 1 && supervisor.Length == 1)
        {
            traveller[0].transform.position = GameObject.Find("Marker (Traveller)").transform.position;
            inspector[0].transform.position = GameObject.Find("Marker (Inspector)").transform.position;
            supervisor[0].transform.position = GameObject.Find("Marker (Supervisor)").transform.position;

            isInGame = true;
        }
        else
        {
            Debug.Log("Cannot start a game ");
        }
        
    }
}
