using UnityEngine;

[CreateAssetMenu(fileName = "Summon", menuName = "ScriptableObjects/Summon")]
public class SummonScriptableObject : ScriptableObject
{
    [Header("Summon Stats")]
    public float AttackRange;
    public float MovementSpeed;
}
