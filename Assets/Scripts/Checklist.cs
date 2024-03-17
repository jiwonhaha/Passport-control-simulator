using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checklist : MonoBehaviour
{
    [SerializeField] GameObject player;

    Vector3 originalScale;

    private void Start()
    {
        originalScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.CompareTag("Supervisor"))
        {
            gameObject.transform.localScale = originalScale;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }
}
