using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameSystem : MonoBehaviour
{
    private GameObject traveller;
    private GameObject inspector;
    private GameObject supervisor;

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

    public void TagTraveller()
    {
        Debug.Log("Player choose traveller role!");
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
        if (traveller != null && inspector != null && supervisor != null)
        {
            traveller.transform.position = GameObject.Find("Marker (Traveller)").transform.position;
            inspector.transform.position = GameObject.Find("Marker (Inspector)").transform.position;
            supervisor.transform.position = GameObject.Find("Marker (Inspector)").transform.position;
        }
        
    }
}
