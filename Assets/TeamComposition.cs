using UnityEngine;
using UnityEngine.UI;

public class TeamComposition : MonoBehaviour
{
    public GameObject list;
    private GameObject slotprefab;

    public void AddSlot(int index)
    {
        Enums.ESummon[] enumValues = (Enums.ESummon[])System.Enum.GetValues(typeof(Enums.ESummon));
        Enums.ESummon summon = enumValues[index];

        string resourceName = "Summons/" + summon.ToString();
        slotprefab = Resources.Load<GameObject>(resourceName);

        GameObject slot = Instantiate(slotprefab);
        slot.transform.SetParent(list.transform);
    }
}