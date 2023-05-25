using UnityEngine;
using UnityEngine.UI;


public class TogglePop : MonoBehaviour
{
    /*
    [SerializeField]
    private GameObject target;

    public void Toggle()
    {
        target.SetActive(!target.activeSelf);
    }     
     */

    public void Toggle(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }

    public void Remove(GameObject target)
    {
        target.SetActive(false);
    }
}