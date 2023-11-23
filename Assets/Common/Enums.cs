public static class Enums
{
    public enum ESummon
    {
        SenorZorro,
        SpitGlider
        SenorZorro1,
        SenorZorro2,
        SenorZorro3,
        SenorZorro4,
        SenorZorro5,
        SenorZorro6,
        SenorZorro7,
        SenorZorro8,
        SenorZorro9,
        SenorZorro10
    }
    public enum ESummonStats
    {
        AttackRange,    //사거리
        MoveSpeed,    //이동속도
        Health,         //체력
        Defence,         //방어력
        PersonalDistance,    //너무 가까운 거리 판별
    }
    public enum ESummonAction
    {
        Attack,
        Skill,
        Ult
    }
    public enum ESkillStats
    {
        AttackRange,
        CoolTime,
        Damage
    }
    public enum ELeague
    {
        Amature
    }
    public enum ESponsor
    {
        Duck
    }
    public enum EEquipment
    {
        MagicDust
    }
    public enum EStrategy
    {
        Defensive
    }
    public enum ETrait
    {
        Mad
    }
    public enum ECC
    {
        None,
        Stun,
        KnockBack
    }
    public enum ECCStats
    {
        Time,
        Power
    }
    public enum ELanguage
    {
        KR,
        EN
    }
    public enum ETeamSide
    {
        TeamA,
        TeamB
    }
    public enum EElement
    {
        Sepia,
        Emerald,
        Aqua,
        None
    }
    public enum EBanPickState
    {
        SelectStage,
        Ban,
        Pick,
        SetPair,
        SelectStrategy,
        Done
    }
    public enum ESchedule
    {
        Scout,
        Break,
        Match
    }
    public enum ECompetitionType
    {
        FullLeague,
        Tournament,
        DoubleElimination,
    }
}