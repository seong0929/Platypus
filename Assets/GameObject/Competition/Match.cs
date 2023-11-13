using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

public class Match
{
    public int PlayerNum;
    public int CrystalMAXNum;
    public int WinPoint;
    public Team TeamA;
    public Team TeamB;

    public EMatchState MatchState;
    private Group GroupA;
    private Group GroupB;

    public EElement? Stage;
    public ETeam? Winner;
    public ETeam? Loser;


    private MatchStatePair _currentMatchState;
    private int _matchStateIndex = 0;
    public List<ESummon> PickableSummons;
    private List<ESummon> _unselectableSummons;


    public Match(int playerNum, int crystalMAXNum, int winPoint, Team teamA, Team teamB)// EElement stage = EElement.None , Group groupA, Group groupB)
    {
        PlayerNum = playerNum;
        CrystalMAXNum = crystalMAXNum;
        WinPoint = winPoint;
        TeamA = teamA;
        TeamB = teamB;

        MatchState = EMatchState.NotStarted;
        Winner = null;
        Loser = null;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
