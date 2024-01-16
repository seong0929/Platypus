public struct SkillStats
{
    public float CoolTime;
    public float Damage;
    public float Range;
    public float Duration;
    public float Heal;
    public float CriticalChance;
    public float CriticalCoefficient;
}
public struct CCStats
{
    public ActionStats KnockBack;
    public ActionStats Stun;
    public ActionStats Slow;
}
public struct BuffDebuffStats
{
    public ActionStats MoveSpeed;
    public ActionStats Defence;
    public ActionStats AttackDamage;
    public ActionStats ActionSpeed;
    public ActionStats CriticalChance;
}
public struct ActionStats
{
    public float Duration;
    public float Amount;
}