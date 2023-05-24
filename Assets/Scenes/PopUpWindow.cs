using UnityEngine;
using UnityEngine.UI;

public class PopUpWindow: MonoBehaviour
{
    [SerializeField]
    private GameObject Window;
    [SerializeField]
    private GameObject Content;


    public void ToggleWindow()
    {
        Window.SetActive(!Window.activeSelf);
        Content.SetActive(!Window.activeSelf);
    }
}