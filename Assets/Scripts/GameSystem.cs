using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    [Header("XR objects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject deviceSimulator;

    [Header("Game Simulation")]
    [System.NonSerialized]
    public bool isInGame;
    public bool isGameReset;

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
    [SerializeField] GameObject leftDoorGate;
    [SerializeField] GameObject rightDoorGate;
    [SerializeField] GameObject cage;

    [Header("Passport List")]
    [SerializeField] GameObject[] passports;
    [SerializeField] GameObject[] passportUIs;
    [SerializeField] GameObject[] stories;

    [Header("Teleportation Marker")]
    [SerializeField] Vector3 travellerMarker;
    [SerializeField] Vector3 inspectorMarker;
    [SerializeField] Vector3 supervisorMarker;
    [SerializeField] Vector3 lobbyMarker;
    [SerializeField] Vector3 passportSpawnPoint = new Vector3(7.5f, 0.75f, 30.0f);
    [SerializeField] Vector3 storySpawnPoint = new Vector3(5.5f, 1.0f, 33.0f);

    [Header("Settings")]
    [SerializeField] int numberOfRounds;
    [SerializeField] int numberOfTravellers;
    [SerializeField] int numberOfInspectors;
    [SerializeField] int numberOfSupervisors;


    public int currentRounds;
    bool inspectorHasChosen;
    bool supervisorHasChosen;

    int currentNumberOftraveller = 0;
    int currentNumberOfinspector = 0;
    int currentNumberOfsupervisor = 0;

    int token;
    List<int> passportIndices = new List<int>();

    List<GameObject> finalResultList = new List<GameObject>();
    List<bool> inspectorDecisionList = new List<bool>();
    List<bool> supervisorDecisionList = new List<bool>();

    NetworkContext context;

    private void Awake()
    {
        travellerButton.onClick.AddListener(TagTraveller);
        inspectorButton.onClick.AddListener(TagInspector);
        supervisorButton.onClick.AddListener(TagSupervisor);

#if UNITY_EDITOR
        deviceSimulator.SetActive(true);
#endif
    }

    private void Start()
    {
        player.gameObject.tag = "Player";
        context = NetworkScene.Register(this);

        isInGame = false;
        isGameReset = false;

        inspectorHasChosen = false;
        supervisorHasChosen = false;

        token = 0;

        int count = 0;
        do
        {
            int index = new System.Random().Next(0, passports.Length);
            if (!passportIndices.Contains(index))
            {
                passportIndices.Add(index);
                count++;
            }
        } while (count < numberOfRounds);
    }

    private void Update()
    {
        // Remove ! to test the real play
        if (isInGame)
        {
            bool inspectorPass = inspectorPassButton.GetComponent<PassActive>().hasChosenChoice;
            bool inspectorReject = inspectorRejectButton.GetComponent<PassActive>().hasChosenChoice;
            bool supervisorPass = supervisorPassButton.GetComponent<PassActive>().hasChosenChoice;
            bool supervisorReject = supervisorRejectButton.GetComponent<PassActive>().hasChosenChoice;

            if (!inspectorHasChosen)
            {
                if (inspectorPass)
                {
                    Debug.Log("Round: " + currentRounds + ", inspector chose passed!");
                    inspectorDecisionList.Add(true);
                    leftDoorGate.SetActive(false);
                    rightDoorGate.SetActive(false);
                    inspectorHasChosen = true;
                }
                if (inspectorReject)
                {
                    Debug.Log("Round: " + currentRounds + ", inspector chose rejected!");
                    inspectorDecisionList.Add(false);
                    cage.SetActive(true);
                    inspectorHasChosen = true;
                }
            }

            if (!supervisorHasChosen)
            {
                if (supervisorPass)
                {
                    Debug.Log("Round: " + currentRounds + ", supervisor chose passed!");
                    supervisorDecisionList.Add(true);
                    supervisorHasChosen = true;
                }
                if (supervisorReject)
                {
                    Debug.Log("Round: " + currentRounds + ", supervisor chose rejected!");
                    supervisorDecisionList.Add(false);
                    supervisorHasChosen = true;
                }
            }

            if (inspectorHasChosen && supervisorHasChosen)
            {
                isGameReset = true;
                inspectorHasChosen = false;
                supervisorHasChosen = false;

                GameObject[] resultLists = GameObject.FindGameObjectsWithTag("Passport UI");
                if (resultLists.Length != 0)
                {
                    for (int i = 0; i < resultLists.Length; i++)
                    {
                        finalResultList.Add(resultLists[i].gameObject);
                    }
                }

                StartCoroutine(GameReset());
            }
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
            //if (currentNumberOfinspector == numberOfInspectors && currentNumberOfsupervisor == numberOfSupervisors)
            {
                isInGame = true;
                currentRounds = 1;

                Debug.Log("Start round " + currentRounds);

                SpawnPassport(passportIndices[currentRounds - 1]);

                //HideFinalResult();

                Message m = new Message();
                m.token = token;
                m.passportIndices = passportIndices;
                m.totalOftraveller = currentNumberOftraveller;
                m.totalOfsupervisor = currentNumberOfsupervisor;
                m.totalOfinspector = currentNumberOfinspector;
                context.SendJson(m);

                finalResultList = new List<GameObject>();
                inspectorDecisionList = new List<bool>();
                supervisorDecisionList = new List<bool>();
            }
        }
    }

    private void SpawnPassport(int randomIndex)
    {
        GameObject selectedPassport = passports[randomIndex];
        GameObject selectedStory = stories[randomIndex];

        Instantiate(selectedPassport, passportSpawnPoint, UnityEngine.Random.rotation);
        Instantiate(selectedStory, storySpawnPoint, Quaternion.Euler(0, 90, 0));
    }

    //private void ShowFinalResult()
    //{
    //    GameObject results = GameObject.Find("/Start Room/Results");

    //    for (int i = 0; i < numberOfRounds; i++)
    //    {
    //        GameObject result = finalResultList[i];
    //        float zPos = 3.2f - (float)i * 2.1f;
    //        Instantiate(result, new Vector3(4.3f, 1f, zPos), Quaternion.Euler(0, 90, 0));

    //        var renderer = results.transform.GetChild(i).gameObject.GetComponent<Renderer>();
    //        if (inspectorDecisionList[i])
    //        {
    //            renderer.material.SetColor("_Color", Color.green);
    //        }
    //        else
    //        {
    //            renderer.material.SetColor("_Color", Color.red);
    //        }

    //        renderer = results.transform.GetChild(i + numberOfRounds).gameObject.GetComponent<Renderer>();
    //        if (supervisorDecisionList[i])
    //        {
    //            renderer.material.SetColor("_Color", Color.green);
    //        }
    //        else
    //        {
    //            renderer.material.SetColor("_Color", Color.red);
    //        }
    //    }
    //}

    //private void HideFinalResult()
    //{
    //    GameObject[] resultLists = GameObject.FindGameObjectsWithTag("Passport UI");
    //    if (resultLists.Length != 0)
    //    {
    //        for (int i = 0; i < resultLists.Length; i++)
    //        {
    //            Destroy(resultLists[i]);
    //        }
    //    }
    //}

    IEnumerator GameReset()
    {

        yield return new WaitForSeconds(3);

        GameObject passport = GameObject.FindGameObjectsWithTag("Passport")[0];
        Destroy(passport);

        GameObject story = GameObject.FindGameObjectsWithTag("Story")[0];
        Destroy(story);

        GameObject[] screens = GameObject.FindGameObjectsWithTag("Passport UI");
        for (int i = 0; i < screens.Length; i++)
        {
            Destroy(screens[i]);
        }

        cage.transform.position = new Vector3(-6f, 3f, 30f);
        cage.SetActive(false);

        leftDoorGate.SetActive(true);
        rightDoorGate.SetActive(true);

        isGameReset = false;

        currentRounds++;
        if (currentRounds > numberOfRounds)
        {
            Debug.Log("Game End!");
            isInGame = false;

            currentRounds = 0;

            currentNumberOfinspector = 0;
            currentNumberOfsupervisor = 0;
            currentNumberOftraveller = 0;

            //ShowFinalResult();

            Message m = new Message();
            if (player.CompareTag("Traveller"))
            {
                m.token = 1;

                passportIndices = new List<int>();
                int count = 0;
                do
                {
                    int index = new System.Random().Next(0, passports.Length);
                    if (!passportIndices.Contains(index))
                    {
                        passportIndices.Add(index);
                        count++;
                    }
                } while (count < numberOfRounds);
            }
            else
            {
                m.token = 0;
                m.passportIndices = passportIndices;
            }
            m.totalOftraveller = currentNumberOftraveller;
            m.totalOfsupervisor = currentNumberOfsupervisor;
            m.totalOfinspector = currentNumberOfinspector;
            context.SendJson(m);

            player.gameObject.tag = "Player";
            player.transform.position = lobbyMarker;
        }
        else
        {
            Debug.Log("Start round " + currentRounds);
            SpawnPassport(passportIndices[currentRounds - 1]);

            Message m = new Message();
            if (player.CompareTag("Traveller"))
            {
                m.token = 1;
                player.transform.position = travellerMarker;
                player.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                m.token = 0;
            }
            m.passportIndices = passportIndices;
            m.totalOftraveller = currentNumberOftraveller;
            m.totalOfsupervisor = currentNumberOfsupervisor;
            m.totalOfinspector = currentNumberOfinspector;
            context.SendJson(m);
        }
    }

    public void TagTraveller()
    {
        if (numberOfTravellers > currentNumberOftraveller)
        {
            player.transform.position = travellerMarker;
            player.transform.rotation = Quaternion.Euler(0, 90, 0);
            player.gameObject.tag = "Traveller";

            Debug.Log("Player choose traveller role!");
            currentNumberOftraveller++;

            Message m = new Message();
            m.totalOftraveller = currentNumberOftraveller;
            m.totalOfsupervisor = currentNumberOfsupervisor;
            m.totalOfinspector = currentNumberOfinspector;
            m.token = 1;
            m.passportIndices = passportIndices;
            context.SendJson(m);
        }

    }

    public void TagSupervisor()
    {
        if (numberOfSupervisors > currentNumberOfsupervisor)
        {
            player.transform.position = supervisorMarker;
            player.gameObject.tag = "Supervisor";

            Debug.Log("Player choose supervisor role!");
            currentNumberOfsupervisor++;

            Message m = new Message();
            m.totalOftraveller = currentNumberOftraveller;
            m.totalOfsupervisor = currentNumberOfsupervisor;
            m.totalOfinspector = currentNumberOfinspector;
            m.token = 0;
            m.passportIndices = passportIndices;
            context.SendJson(m);
        }
    }

    public void TagInspector()
    {
        if (numberOfInspectors > currentNumberOfinspector)
        {
            player.transform.position = inspectorMarker;
            player.gameObject.tag = "Inspector";

            Debug.Log("Player choose inspector role!");
            currentNumberOfinspector++;

            Message m = new Message();
            m.totalOftraveller = currentNumberOftraveller;
            m.totalOfsupervisor = currentNumberOfsupervisor;
            m.totalOfinspector = currentNumberOfinspector;
            m.token = 0;
            m.passportIndices = passportIndices;
            context.SendJson(m);
        }
    }

    private struct Message
    {
        public int totalOftraveller;
        public int totalOfinspector;
        public int totalOfsupervisor;

        public int token;
        public List<int> passportIndices;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var Message = m.FromJson<Message>();

        currentNumberOftraveller = Message.totalOftraveller;
        currentNumberOfinspector = Message.totalOfinspector;
        currentNumberOfsupervisor = Message.totalOfsupervisor;

        if (token < Message.token)
        {
            passportIndices = Message.passportIndices;
        }
    }

}