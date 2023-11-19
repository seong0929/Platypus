using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class BanPickRunner
{

    public Round Round;
    public RoundStatePair CurrentRoundState => RoundStates[RoundStateIndex];
    public List<RoundStatePair> RoundStates;
    public int RoundStateIndex = 0;

    public int PlayerNum;
    public List<ESummon> AvaiableSummons;
    
    public List<ESummon> PickableSummons;

    public List<ESummon> BannedSummonsTeamA;
    public List<ESummon> BannedSummonsTeamB;

    public List<ESummon> PickedSummonsTeamA;
    public List<ESummon> PickedSummonsTeamB;


    private void SetRoundStates()
    {
        RoundStates = new List<RoundStatePair>();
        
        switch(PlayerNum)
        {
            case 1:
                RoundStates = BanPickOrder.OnePlayerBanPickOrder;
                break;
            case 2:
                RoundStates = BanPickOrder.TwoPlayerMatchOrder;
                break;
            case 3:
                RoundStates = BanPickOrder.ThreePlayerMatchOrder;
                break;
            case 4:
                RoundStates = BanPickOrder.FourPlayerMatchOrder;
                break;
            default:
                break;
        }
    }

}
public struct RoundStatePair
{
    public Enums.EBanPickState State { get; set; }
    public Enums.ETeam Turn { get; set; }
}