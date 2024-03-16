using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checklist : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameSystem gameSystem;

    // Update is called once per frame
    void Update()
    {
        if (gameSystem.isInGame)
        {
            if (player.CompareTag("Supervisor"))
            {
                gameObject.SetActive(true);
            }
        }
        
        else
        {
            gameObject.SetActive(false);
        }
    }
}
