using UnityEngine;
using UnityEngine.UI;

enum EHomeButtonPanelName
{
    TeamManagement,
    ResourceManagement,
    Competition,
    System
}

public class ToggleButtons : MonoBehaviour
{
    [SerializeField] EHomeButtonPanelName _targetName;
    [SerializeField] GameObject _panelSet; // The panel containing the buttons to toggle

    private GameObject _targetPanel;

    private void Start()
    {
        _targetPanel = _panelSet.transform.GetChild((int)_targetName).gameObject;
    }
    public void TogglePanel()
    {
        _targetPanel.SetActive(!_targetPanel.activeSelf); // Toggle the active state of the panel

        foreach (Transform panel in _panelSet.transform) // Off panel that is not target Panel
        {
            if (panel.gameObject != _targetPanel)
            {
                panel.gameObject.SetActive(false);
            }
        }
    }
    public void CloseAll()
    {
        foreach (Transform panel in _panelSet.transform) // Off panel that is not target Panel
        {
            panel.gameObject.SetActive(false);            
        }
    }
}
