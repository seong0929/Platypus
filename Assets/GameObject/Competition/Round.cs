using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

// # Match:

public class Round
{
    public int PlayerNum;
    public int CrystalMAXNum;
    public int WinPoint;

    public Group GroupA;
    public Group GroupB;

    public EElement? Stage;
    public ETeam? Winner;
    public ETeam? Loser;

    public List<ESummon> AvaiableSummons;

    public bool IsDone;

    public Round(int playerNum, int crystalMAXNum, int winPoint, Team teamA, Team teamB)
    {
        PlayerNum = playerNum;
        CrystalMAXNum = crystalMAXNum;
        WinPoint = winPoint;

        GroupA = new Group(teamA);
        GroupB = new Group(teamB);

        Winner = null;
        Loser = null;

        IsDone = false;
    }
            
    public void SetAvaiableSummons(List<ESummon> avaiableSummons)
    {
        AvaiableSummons = avaiableSummons;
    }
}

public class Group
{
    public Enums.ETeam TeamSide;
    public Team Team;
    public List<Player> SelectedPlayers;
    public List<Enums.ESummon> SelectedSummon;
    public List<Enums.ESummon> BannedSummon;
    public Dictionary<Player, Enums.ESummon> PlayerSummonPair;
    public int Score;

    public Group(Team team)//, List<Player> selectedOne)
    {
        Team = team;
        SelectedPlayers = new List<Player>();
        SelectedSummon = new List<Enums.ESummon>();
        BannedSummon = new List<Enums.ESummon>();
        PlayerSummonPair = new Dictionary<Player, Enums.ESummon>();
    }

}