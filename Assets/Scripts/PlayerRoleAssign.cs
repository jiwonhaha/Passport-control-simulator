using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoleAssign : MonoBehaviour
{
    GameObject travellerStand;
    GameObject inspectorStand;
    GameObject supervisorStand;

    public string currentRole;

    [Header("Distance")]
    [SerializeField] float distanceThreshold = 0.5f;

    float distantFromTraveller;
    float distantFromInspector;
    float distantFromSupervisor;

    // Start is called before the first frame update
    void Start()
    {
        travellerStand = GameObject.Find("Traveller Stand");
        inspectorStand = GameObject.Find("Inspector Stand");
        supervisorStand = GameObject.Find("Supervisor Stand");
    }

    // Update is called once per frame
    void Update()
    {
        distantFromTraveller = Mathf.Pow(travellerStand.transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(travellerStand.transform.position.z - gameObject.transform.position.z, 2);
        distantFromInspector = Mathf.Pow(inspectorStand.transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(inspectorStand.transform.position.z - gameObject.transform.position.z, 2);
        distantFromSupervisor = Mathf.Pow(supervisorStand.transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(supervisorStand.transform.position.z - gameObject.transform.position.z, 2);

        if (Mathf.Sqrt(distantFromTraveller) < distanceThreshold)
        {
            gameObject.tag = "Traveller";
            currentRole = "Traveller";
            Debug.Log(1);
        }
        else if (Mathf.Sqrt(distantFromInspector) < distanceThreshold)
        {
            gameObject.tag = "Inspector";
            currentRole = "Inspector";
            Debug.Log(2);
        }
        else if (Mathf.Sqrt(distantFromSupervisor) < distanceThreshold)
        {
            gameObject.tag = "Supervisor";
            currentRole = "Supervisor";
            Debug.Log(3);
        }
        else
        {
            gameObject.tag = "Player";
            currentRole = "None";
        }
    }
}
