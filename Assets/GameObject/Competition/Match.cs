using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Match
{
    public int PlayerNum;
    public int CrystalMAXNum;
    public int WinPoint;
    public Enums.EElement Stage;
    public Group GroupA;
    public Group GroupB;
    public Enums.ETeam Winner;
    public Enums.ETeam Loser;

    private List<MatchStatePair> _matchStateList;
    private MatchStatePair _currentMatchState;
    private int _matchStateIndex = 0;
    public List<Enums.ESummon> PickableSummons;
    private List<Enums.ESummon> _unselectableSummons;


    public Match(int playerNum, int crystalMAXNum, int winPoint, Enums.EElement stage = Enums.EElement.None , Group groupA, Group groupB)
    {
        PlayerNum = playerNum;
        CrystalMAXNum = crystalMAXNum;
        WinPoint = winPoint;
        Stage = stage;
        GroupA = groupA;
        GroupB = groupB;
        Winner = Enums.ETeam.None;
        Loser = Enums.ETeam.None;

        _matchStateList = new List<MatchStatePair>();
        _matchStateList.Add(new MatchStatePair(Enums.EMatchState.PickSummon, new PickSummonState(this)));
        _matchStateList.Add(new MatchStatePair(Enums.EMatchState.PickCrystal, new PickCrystalState(this)));
        _matchStateList.Add(new MatchStatePair(Enums.EMatchState.PickPlayer, new PickPlayerState(this)));
        _matchStateList.Add(new MatchStatePair(Enums.EMatchState.Battle, new BattleState(this)));
        _matchStateList.Add(new MatchStatePair(Enums.EMatchState.Result, new ResultState(this)));
        _matchStateList.Add(new MatchStatePair(Enums.EMatchState.End, new EndState(this)));

        _currentMatchState = _matchStateList[_matchStateIndex];
        _currentMatchState.MatchState.Enter();
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
