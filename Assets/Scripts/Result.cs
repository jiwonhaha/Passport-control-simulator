using UnityEngine;
using TMPro; 

public class ResultUIController : MonoBehaviour
{
    public TextMeshProUGUI roundTextTMP; 
    public TextMeshProUGUI inspectorDecisionTextTMP;

    public string Round
    {
        set
        {
            if (roundTextTMP != null)
                roundTextTMP.text = value;
        }
    }

    public string Inspector
    {
        set
        {
            if (inspectorDecisionTextTMP != null)
                inspectorDecisionTextTMP.text = value;
        }
    }
}
