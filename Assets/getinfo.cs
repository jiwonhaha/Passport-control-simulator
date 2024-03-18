using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class getinfo : MonoBehaviour
{
    public TextMeshProUGUI informationText;
    private GameSystem gameSystem;


    void Start()
    {
        // Find the GameSystem component in the scene. This assumes there is only one GameSystem.
        gameSystem = FindObjectOfType<GameSystem>();

        // Example call to update UI based on the lists
        UpdateUIBasedOnDecisions();
    }

     void UpdateUIBasedOnDecisions()
    {
        if (gameSystem != null)
        {
            // Assuming these methods return the full lists.
            var inspectorDecisions = gameSystem.GetInspectorDecisionList();
            var supervisorDecisions = gameSystem.GetSupervisorDecisionList();

            string inspectorDecisionText = inspectorDecisions.Count > 0 ? inspectorDecisions[0].ToString() : "No decision";
            string supervisorDecisionText = supervisorDecisions.Count > 0 ? supervisorDecisions[0].ToString() : "No decision";

            // Update the UI text with the first decisions, if available.
            if (informationText != null)
            {
                informationText.text = $"Inspector First Decision: {inspectorDecisionText}, Supervisor First Decision: {supervisorDecisionText}";
            }
        }
    }

}
