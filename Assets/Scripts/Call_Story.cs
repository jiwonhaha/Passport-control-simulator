using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Call_Story : MonoBehaviour
{
    [SerializeField] private GameObject[] storyUI;

    GameObject gameSystem;

    [SerializeField] private GameObject currentPage;

    int index;

    private void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("Game System");
    }

    private void Update() 
    {
        index = gameSystem.GetComponents<GameSystem>()[0].passportIndices[0];
    }

    public void TurnPage()
    {
        GameObject selectedStory = storyUI[index];
        Instantiate(selectedStory, new Vector3(2.5f, 1.0f, 54.0f), Quaternion.Euler(0, 135, 0));
    
        Destroy(gameObject);
    }
}
