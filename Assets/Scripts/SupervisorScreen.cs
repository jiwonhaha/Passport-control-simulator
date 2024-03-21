using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupervisorScreen : MonoBehaviour
{
    GameObject player;

    [Header("Role Vision")]
    [SerializeField] bool presentToTraveller;
    [SerializeField] bool presentToInspector;
    [SerializeField] bool presentToSupervisor;

    Vector3 originalScale;

    private void Start()
    {
        originalScale = gameObject.transform.localScale;
        player = GameObject.Find("/Local Player/XR Origin (XR Rig)");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.CompareTag("Supervisor") && presentToSupervisor)
        {
            gameObject.transform.localScale = originalScale;
        }
        else if (player.CompareTag("Inspector") && presentToInspector)
        {
            gameObject.transform.localScale = originalScale;
        }
        else if (player.CompareTag("Traveller") && presentToTraveller)
        {
            gameObject.transform.localScale = originalScale;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }
}
