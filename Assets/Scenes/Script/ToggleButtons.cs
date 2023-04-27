using UnityEngine;
using UnityEngine.UI;

enum HomeButtonPanelName
{
    TEAMANAGEMENT,
    RESOURCEMANAGEMENT,
    COMPETITION,
    SYSTEM
}

public class ToggleButtons : MonoBehaviour
{
    [SerializeField]
    private HomeButtonPanelName targetName;
    [SerializeField]
    private GameObject panelSet; // The panel containing the buttons to toggle

    private GameObject targetPanel;

    public void Start()
    {
        targetPanel = panelSet.transform.GetChild((int)targetName).gameObject;
    }

    public void TogglePanel()
    {
        targetPanel.SetActive(!targetPanel.activeSelf); // Toggle the active state of the panel

        foreach (Transform panel in panelSet.transform) // Off panel that is not target Panel
        {
            if (panel.gameObject != targetPanel)
            {
                panel.gameObject.SetActive(false);
            }
        }
    }
}
