using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class Match
{
    public int PlayerNum;
    public int CrystalMAXNum;
    public int WinPoint;

    public Team TeamA;
    public Team TeamB;

    public ETeamSide? Winner;
    public ETeamSide? Loser;

    public Date Date;

    public List<ESummon> AvaiableSummons;

    public bool IsDone;

    public List<Round> Rounds;

    public Match(int playerNum, int crystalMAXNum, int winPoint, Team teamA, Team teamB, Date date, List<ESummon> avaiableSummons)
    {
        PlayerNum = playerNum;
        CrystalMAXNum = crystalMAXNum;
        WinPoint = winPoint;
        TeamA = teamA;
        TeamB = teamB;
        Date = date;
        AvaiableSummons = avaiableSummons;
    }
}
