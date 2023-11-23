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
        MoveSpeed,  //이동속도
        Health, //체력
        Defence    //방어력
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
<<<<<<< HEAD
    // 노드 상태
    public enum ENodeState
    {
        Running,
        Success,
        Failure
    }
    // 소환수 노드 상태
    public enum ESummonState
    {
        Default,    // 기본 상태
        Dead,   // 죽음
        Respawn,    // 리스폰 중
        CC, // CC 걸림
        Attack,  // 공격 중
        Skill,   // 스킬 사용 중
        Ult, // Ult 사용 중
        Move    // 이동 중
=======
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
>>>>>>> c92a5d25fb9101545b0ea2cc03c91f12e159c031
    }
}