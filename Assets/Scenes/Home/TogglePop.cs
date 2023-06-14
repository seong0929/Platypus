using UnityEngine;

public class TogglePop : MonoBehaviour
{
    public void Toggle(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }

    public void Remove(GameObject target)
    {
        target.SetActive(false);
    }
}