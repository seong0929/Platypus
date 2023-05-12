using UnityEngine;
using UnityEngine.UI;


public class PopUpWindow: MonoBehaviour
{
    [SerializeField]
    private GameObject Window;

    // Start is called before the first frame update
    /*
    void Start()
    {

    }
     * 
     */

    public void ToggleWindow()
    {
        Window.SetActive(!Window.activeSelf);
    }

}