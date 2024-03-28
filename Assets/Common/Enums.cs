public static class Enums
{
    public enum ESummon
    {
        SenorZorro,
//        SpitGlider,
  //      PoToad,

        // 테스트를 위한 Enum 값들
        SenorZorro2,
        SenorZorro3,
        SenorZorro4,
        SenorZorro5,
        SenorZorro6,
        SenorZorro7,
        SenorZorro8,
        SenorZorro9,
        SenorZorro10,
       // SpitGlider1,
    //    SpitGlider2,
      //  SpitGlider3
    }
    public enum EBuffType
    {
        MoveSpeed,
        Defence,
        Damage,
        ActionSpeed,
        CriticalChance
    }
    public enum ESkillState
    {
        Available,
        InUse,
        CoolingDown
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
    public enum ELanguage
    {
        KR,
        EN
    }
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
        Idle,    // 기본 상태
        Dead,   // 죽음
        Respawn,    // 리스폰 중
        HardCC,
        Attack,  // 공격 중
        Skill,   // 스킬 사용 중
        Ult, // Ult 사용 중
        Move    // 이동 중
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
        DoubleElimination
    }
}