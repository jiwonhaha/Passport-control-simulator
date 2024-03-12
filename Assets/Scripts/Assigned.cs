using UnityEngine;
using UnityEngine.UI;

public class Assigned : MonoBehaviour
{
    public Button yourButton; // Assign this in the inspector
    private bool isClicked = false; // To prevent multiple clicks

    void Start()
    {
        if (yourButton != null)
        {
            yourButton.onClick.AddListener(() => ButtonClicked());
        }
    }

    void ButtonClicked()
    {
        if(isClicked) return; // Prevent function from running if the button was already clicked
        
        ColorBlock colors = yourButton.colors;
        colors.disabledColor = Color.red; // Set the disabled color to red
        yourButton.colors = colors;
        
        yourButton.interactable = false; // This now uses the red color for disabled state
        isClicked = true; // Mark as clicked to prevent future clicks
    }
}
