public static class Enums
{
    public enum ESummon
    {
        SenorZorro,
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
        NormalDamage,   //일반 공격 데미지
        Defence,         //방어력
        PersonalDistance,    //너무 가까운 거리 판별
        CoolTime,    //스킬 쿨 타임
        UltGauge   //궁극기 게이지
    }

    public enum ESummonAction
    {
        Skill,
        Ult
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
    public enum ETeam
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
        // Started Yet,
        None,
        SelectPlayer,
        SelectStage,
        Ban,
        Pick,
        SetPair,
        SelectStrategy,
        Battle,
        End
    }
    public enum ESchedule
    {
        Scout,
        Break,
        Match
    }
}