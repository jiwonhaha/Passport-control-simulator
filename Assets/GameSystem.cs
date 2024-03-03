using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public void TeleportTraveller()
    {
        player.transform.position = GameObject.Find("Marker (Traveller)").transform.position;
    }

    public void TeleportSupervisor()
    {
        player.transform.position = GameObject.Find("Marker (Supervisor)").transform.position;
    }

    public void TeleportInspector()
    {
        player.transform.position = GameObject.Find("Marker (Inspector)").transform.position;
    }
}
