using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

// # Match:

public class Match
{
    public int PlayerNum;
    public int CrystalMAXNum;
    public int WinPoint;

    public Group GroupA;
    public Group GroupB;

    public EMatchState MatchState;

    public EElement? Stage;
    public ETeam? Winner;
    public ETeam? Loser;

    public Date Date;

    private MatchStatePair _currentMatchState;
    public List<Enums.ESummon> BannedSummon;
    private int _matchStateIndex = 0;
    public List<ESummon> PickableSummons;
    public List<ESummon> AvaiableSummons;


    public Match(int playerNum, int crystalMAXNum, int winPoint, Team teamA, Team teamB, Date date)// EElement stage = EElement.None , Group groupA, Group groupB)
    {
        PlayerNum = playerNum;
        CrystalMAXNum = crystalMAXNum;
        WinPoint = winPoint;

        GroupA = new Group(teamA);
        GroupB = new Group(teamB);

        Date = date;

        MatchState = EMatchState.NotStarted;
        Winner = null;
        Loser = null;
    }

    public void SetAvaiableSummons(List<ESummon> avaiableSummons)
    {
        AvaiableSummons = avaiableSummons;
    }

    public void SetPickableSummons()
    {
        PickableSummons = new List<ESummon>();
        foreach (ESummon summon in AvaiableSummons)
        {
            if (!BannedSummon.Contains(summon) && !GroupA.SelectedSummon.Contains(summon) && !GroupB.SelectedSummon.Contains(summon))
            {
                PickableSummons.Add(summon);
            }
        }
    }

    public List<ESummon> GetPickableSummons()
    {
        return PickableSummons;
    }

    public void AddBannedSummon(ESummon summon)
    {
        BannedSummon.Add(summon);
    }

}
