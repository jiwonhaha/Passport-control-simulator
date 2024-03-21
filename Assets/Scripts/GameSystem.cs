using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    [System.NonSerialized]
    public bool isInGame;
    [System.NonSerialized]
    public bool isGameReset;
    [System.NonSerialized]
    public int currentRounds;
    [System.NonSerialized]
    public bool inspectorHasChosen;
    [System.NonSerialized]
    public bool supervisorHasChosen;

    [Header("XR objects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject deviceSimulator;

    
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
    [SerializeField] public GameObject[] passports;
    [SerializeField] public GameObject[] passportScreens;
    [SerializeField] public GameObject[] passportAnswers;
    [SerializeField] public GameObject[] stories;

    [Header("Teleportation Marker")]
    [SerializeField] Vector3 travellerMarker;
    [SerializeField] Vector3 inspectorMarker;
    [SerializeField] Vector3 supervisorMarker;
    [SerializeField] Vector3 lobbyMarker;
    [SerializeField] Vector3 passportSpawnPoint = new Vector3(7.5f, 0.75f, 30.0f);
    [SerializeField] Vector3 storySpawnPoint = new Vector3(5.5f, 1.0f, 33.0f);
    [SerializeField] Vector3 FResultSpawnPoint;

    [Header("Settings")]
    [SerializeField] int numberOfRounds;
    [SerializeField] int numberOfTravellers;
    [SerializeField] int numberOfInspectors;
    [SerializeField] int numberOfSupervisors;
    [SerializeField] int minimumPlayers = 2;

    [Header("Result UI")]
    [SerializeField] GameObject[] ResultUI;

    [Header("Instruction UI")]
    [SerializeField] GameObject[] InstructionUI;
    
    int currentNumberOftraveller = 0;
    int currentNumberOfinspector = 0;
    int currentNumberOfsupervisor = 0;

    int token;
    [System.NonSerialized]
    public List<int> passportIndices = new List<int>();
    List<bool> inspectorDecisionList = new List<bool>();
    List<bool> supervisorDecisionList = new List<bool>();

    private List<GameObject> instantiatedObjects = new List<GameObject>();


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

                    inspectorPassButton.GetComponent<PassActive>().OnDisable();
                    inspectorRejectButton.GetComponent<PassActive>().OnDisable();
                }
                if (inspectorReject)
                {
                    Debug.Log("Round: " + currentRounds + ", inspector chose rejected!");
                    inspectorDecisionList.Add(false);
                    cage.SetActive(true);
                    inspectorHasChosen = true;

                    inspectorPassButton.GetComponent<PassActive>().OnDisable();
                    inspectorRejectButton.GetComponent<PassActive>().OnDisable();
                }
            }

            if (!supervisorHasChosen)
            {
                if (supervisorPass)
                {
                    Debug.Log("Round: " + currentRounds + ", supervisor chose passed!");
                    supervisorDecisionList.Add(true);
                    supervisorHasChosen = true;

                    supervisorPassButton.GetComponent<PassActive>().OnDisable();
                    supervisorRejectButton.GetComponent<PassActive>().OnDisable();
                }
                if (supervisorReject)
                {
                    Debug.Log("Round: " + currentRounds + ", supervisor chose rejected!");
                    supervisorDecisionList.Add(false);
                    supervisorHasChosen = true;

                    supervisorPassButton.GetComponent<PassActive>().OnDisable();
                    supervisorRejectButton.GetComponent<PassActive>().OnDisable();
                }
            }

            if (inspectorHasChosen && supervisorHasChosen)
            {
                isGameReset = true;
                inspectorHasChosen = false;
                supervisorHasChosen = false;

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
            //if (currentNumberOfinspector == numberOfInspectors)
            {
                isInGame = true;
                currentRounds = 1;

                Debug.Log("Start round " + currentRounds);

                SpawnPassport(passportIndices[currentRounds - 1], currentRounds);

                Instantiate(InstructionUI[0], new Vector3(2.5f, 1.0f, 54.0f), Quaternion.Euler(0, 135, 0)); //Traveller
                Instantiate(InstructionUI[1], new Vector3(4.85300016f,0.985000014f,45.5f), Quaternion.Euler(0, 270, 0)); // inspector
                Instantiate(InstructionUI[2], new Vector3(1.23599994f,0.985000014f,45.387001f), Quaternion.Euler(0, 90, 0)); // supervisor

                HideFinalResult();

                Message m = new Message();
                m.token = token;
                m.passportIndices = passportIndices;
                m.totalOftraveller = currentNumberOftraveller;
                m.totalOfsupervisor = currentNumberOfsupervisor;
                m.totalOfinspector = currentNumberOfinspector;
                context.SendJson(m);

                inspectorDecisionList = new List<bool>();
                supervisorDecisionList = new List<bool>();
            }
        }
    }

    private void SpawnPassport(int randomIndex, int round)
    {
        GameObject selectedPassport = passports[randomIndex];
        GameObject selectedStory = stories[randomIndex];

        Instantiate(selectedPassport, passportSpawnPoint, UnityEngine.Random.rotation);

        if (round != 1){
            selectedStory.SetActive(true);
            Instantiate(selectedStory, storySpawnPoint, Quaternion.Euler(0, 135, 0));

        }
    }

    private void ShowFinalResult()
    {
        if (supervisorDecisionList.Count != inspectorDecisionList.Count || supervisorDecisionList.Count != passportIndices.Count)
        {
            Debug.LogError("The lists must have the same length.");
            return;
        }
        
        Vector3 FResultSpawnPoint = new Vector3(4.749f, 1.0f, 3.0f);
        Vector3 FAnsSpawnPoint = new Vector3(4.74f, 1.0f, 3.0f);

        for (int i = 0; i < supervisorDecisionList.Count; i++)
        {
            GameObject selectedPassport = passports[passportIndices[i]];
            GameObject selectedAnswer = passportAnswers[passportIndices[i]];

            int r = i + 1;
            if (supervisorDecisionList[i] == false){
                GameObject FaildUI = Instantiate(ResultUI[0], FResultSpawnPoint, Quaternion.Euler(0, 90, 0)); 
                instantiatedObjects.Add(FaildUI);
                ResultUIController uiController = FaildUI.GetComponent<ResultUIController>();
                uiController.Round = "Round " + r;
                uiController.Inspector = "Inspector Decision: " + (inspectorDecisionList[i] ? "Pass" : "Reject");
            }
            else{
                GameObject SucceedUI = Instantiate(ResultUI[1], FResultSpawnPoint, Quaternion.Euler(0, 90, 0)); 
                instantiatedObjects.Add(SucceedUI);
                ResultUIController uiController = SucceedUI.GetComponent<ResultUIController>();
                uiController.Round = "Round " + r;
                uiController.Inspector = "Inspector Decision: " + (inspectorDecisionList[i] ? "Pass" : "Reject");
            }

            GameObject answerUI  = Instantiate(selectedAnswer, FAnsSpawnPoint, Quaternion.Euler(0, 90, 0));
            instantiatedObjects.Add(answerUI);
            FResultSpawnPoint.z -= 2.0f;
            FAnsSpawnPoint.z -= 2.0f; 
        }
    
    }

    private void HideFinalResult()
    {
        foreach (GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();
    }

    IEnumerator GameReset()
    {

        yield return new WaitForSeconds(3);

        GameObject[] passports = GameObject.FindGameObjectsWithTag("Passport");
        foreach (GameObject passport in passports)
        {
            Destroy(passport);
        }

        GameObject[] stories = GameObject.FindGameObjectsWithTag("Story");
        foreach (GameObject story in stories)
        {
            Destroy(story);
        }

        GameObject[] screens = GameObject.FindGameObjectsWithTag("Passport UI");
        foreach (GameObject screen in screens)
        {
            Destroy(screen);
        }

        cage.transform.localPosition = new Vector3(-4.5f, 4f, 10f);
        cage.SetActive(false);

        leftDoorGate.SetActive(true);
        rightDoorGate.SetActive(true);

        inspectorPassButton.GetComponent<PassActive>().OnEnable();
        inspectorRejectButton.GetComponent<PassActive>().OnEnable();
        supervisorPassButton.GetComponent<PassActive>().OnEnable();
        supervisorRejectButton.GetComponent<PassActive>().OnEnable();

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

            ShowFinalResult();

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
            SpawnPassport(passportIndices[currentRounds - 1], currentRounds);

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
        GameObject u = GameObject.Find("/Ubiq Network Scene (Demo)/Avatar Manager");
        int numberOfPlayers = 0;
        foreach (Transform child in u.transform)
        {
            numberOfPlayers++;
        }

        if (numberOfTravellers > currentNumberOftraveller && numberOfPlayers >= minimumPlayers)
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
        GameObject u = GameObject.Find("/Ubiq Network Scene (Demo)/Avatar Manager");
        int numberOfPlayers = 0;
        foreach (Transform child in u.transform)
        {
            numberOfPlayers++;
        }

        if (numberOfSupervisors > currentNumberOfsupervisor && numberOfPlayers >= minimumPlayers)
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
        GameObject u = GameObject.Find("/Ubiq Network Scene (Demo)/Avatar Manager");
        int numberOfPlayers = 0;
        foreach (Transform child in u.transform)
        {
            numberOfPlayers++;
        }

        if (numberOfInspectors > currentNumberOfinspector && numberOfPlayers >= minimumPlayers)
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