using UnityEngine;

public class TeamComposition : MonoBehaviour
{
    public GameObject List;
    private GameObject _slotprefab;

    public void AddSlot(int index)
    {
        Enums.ESummon[] enumValues = (Enums.ESummon[])System.Enum.GetValues(typeof(Enums.ESummon));
        Enums.ESummon summon = enumValues[index];

        string resourceName = "Summons/" + summon.ToString();
        _slotprefab = Resources.Load<GameObject>(resourceName);

        GameObject slot = Instantiate(_slotprefab);
        slot.transform.SetParent(List.transform);
    }
}