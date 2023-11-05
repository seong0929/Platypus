using UnityEngine;

[CreateAssetMenu(fileName = "Summon", menuName = "ScriptableObjects/Summon")]
public class SummonScriptableObject : ScriptableObject
{
    [Header("Summon Stats")]
    public float AttackRange;
    public float MovementSpeed;
    public float Health;
    public float NormalDamage;
    public float Defense;

    [Header("Sprites")]
    public Sprite DefaultSprite;

    [Header("Types")]
    public Enums.EElement ElementType;

    [Header("Price")]
    public int CrystalNum;
}
