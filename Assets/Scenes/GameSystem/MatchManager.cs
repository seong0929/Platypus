using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;


//public struct Match
//{
//    public int PlayerNum;
//    public int CrystalMAXNum;
//    public int WinPoint;
//    public Enums.EElement Stage;
//    public Group GroupA;
//    public Group GroupB;
//    public Enums.ETeam Winner;
//}
public struct MatchPlan
{
    public int PlayerNum;
    public int CrystalMAXNum;
    public int WinPoint;
    public Team TeamA;
    public Team TeamB;
}
public class MatchManager
{
    
    private Match _match;

    public int PlayerNum;
    private int _crystalLimitNum;
    public Enums.EElement Stage;
    public Group GroupA;
    public Group GroupB;

    #region BanPickOrders
    private static readonly List<MatchStatePair> TwoPlayerMatchOrder = new List<MatchStatePair>
    {
        new MatchStatePair { State = Enums.EBanPickState.SelectPlayer, Turn = Enums.ETeam.TeamA},
        new MatchStatePair { State = Enums.EBanPickState.SelectPlayer, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.SelectStage, Turn = Enums.ETeam.TeamB },
        // Ban
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        // Pick
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        // SetPair
        new MatchStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamB },
        // SetStrategy
        new MatchStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamB }
    };

    private static readonly List<MatchStatePair> ThreePlayerMatchOrder = new List<MatchStatePair>
    {
        new MatchStatePair { State = Enums.EBanPickState.SelectPlayer, Turn = Enums.ETeam.TeamA},
        new MatchStatePair { State = Enums.EBanPickState.SelectPlayer, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.SelectStage, Turn = Enums.ETeam.TeamB },
        //Ban
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        //Pick
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        // SetPair
        new MatchStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamB },
        // SetStrategy
        new MatchStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamB }

    };

    private static readonly List<MatchStatePair> FourPlayerMatchOrder = new List<MatchStatePair>
    {
        new MatchStatePair { State = Enums.EBanPickState.SelectPlayer, Turn = Enums.ETeam.TeamA},
        new MatchStatePair { State = Enums.EBanPickState.SelectPlayer, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.SelectStage, Turn = Enums.ETeam.TeamB },
        //Ban
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        //Pick
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        //Ban
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        //Pick
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        // SetPair
        new MatchStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamB },
        // SetStrategy
        new MatchStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamA },
        new MatchStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamB }

    };

    #endregion

    private List<MatchStatePair> _matchStateList;
    private MatchStatePair _currentMatchState;
    private int _matchStateIndex = 0;
    public List<Enums.ESummon> PickableSummons;
    private List<Enums.ESummon> _unselectableSummons;

    #region Event
    public static UnityEvent<Enums.ESummon> RecieveBanPickSummon = new UnityEvent<Enums.ESummon>();
    #endregion

    public MatchManager()
    {

    }

    public void InitializeMatch(int playerNum, Team teamA, Team teamB)
    {
        PlayerNum = playerNum;

        GroupA = new Group(teamA);
        GroupA.TeamSide = Enums.ETeam.TeamA;

        GroupB = new Group(teamB);
        GroupB.TeamSide = Enums.ETeam.TeamB;

        Enums.ESummon[] tmpPickableSummons = (Enums.ESummon[]) Enum.GetValues(typeof(Enums.ESummon));
        SetPickableSummons(tmpPickableSummons.ToList());
    }

    public void InitializeMatch(Match match)
    {
        _match = match;
        _match.GroupA.TeamSide = Enums.ETeam.TeamA;
        _match.GroupB.TeamSide = Enums.ETeam.TeamB;

        Enums.ESummon[] tmpPickableSummons = (Enums.ESummon[]) Enum.GetValues(typeof(Enums.ESummon));
        SetPickableSummons(tmpPickableSummons.ToList());
    }

    public void SetSelectedPlayer(Enums.ETeam teamSide, List<Player> selectedOne)
    {
        Group targetGroup = GetGroup(teamSide);

        targetGroup.SelectedPlayers = selectedOne;
    }

    public void SetMatchState(MatchStatePair matchState)
    {
        _currentMatchState = matchState;
    }

    public void AddBanned(Enums.ETeam teamSide, Enums.ESummon bannedSummon)
    {
        Group targetGroup = GetGroup(teamSide);
        targetGroup.BannedSummon.Add(bannedSummon);

        _unselectableSummons.Add(bannedSummon);
    }

    public void AddPicked(Enums.ETeam teamSide, Enums.ESummon selectedSummon)
    {
        Group targetGroup = GetGroup(teamSide);
        targetGroup.SelectedSummon.Add(selectedSummon);

        _unselectableSummons.Add(selectedSummon);
    }
    public List<Enums.ESummon> GetUnselectableSummons()
    {
        return _unselectableSummons;
    }
    public void SetPickableSummons(List<Enums.ESummon> pickable)
    {
        PickableSummons = pickable;
    }
    public void SetPlayerSummonPair(Enums.ETeam teamSide, Dictionary<Player, Enums.ESummon> playerSummonPair)
    {
        Group targetGroup = GetGroup(teamSide);

        targetGroup.PlayerSummonPair = playerSummonPair;
    }

    private void ToNextTurn()
    {
        _matchStateIndex++;
        SetMatchState(_matchStateList[_matchStateIndex]);
        switch (_currentMatchState.State)
        {
            case Enums.EBanPickState.SelectPlayer:
                InitiateSelectPlayer();
                break;
            case Enums.EBanPickState.SelectStage:
                InitiateSelectStage();
                break;
            case Enums.EBanPickState.Ban:
                InitiateBan();
                break;
            case Enums.EBanPickState.Pick:
                InitiatePick();
                break;
            case Enums.EBanPickState.SetPair:
                InitiaiteSetPair();
                break;
            case Enums.EBanPickState.SelectStrategy:
                InitiateSelectStrategy();
                break;
        }
    }

    private Group GetGroup(Enums.ETeam teamSide)
    {
        if(teamSide == Enums.ETeam.TeamA)
        {
            return GroupA;
        }
        else
        {
            return GroupB;
        }
    }

    private void RecieveBanPickRequest(Enums.ESummon eSummon)
    {
        Group targetGroup = GetGroup(_currentMatchState.Turn);

        if(_currentMatchState.State == Enums.EBanPickState.Ban)
        {
            AddBanned(_currentMatchState.Turn, eSummon);
        }
        else if(_currentMatchState.State == Enums.EBanPickState.Pick)
        {
            AddPicked(_currentMatchState.Turn, eSummon);
        }
    }

    private void InitializeEvents()
    {
        RecieveBanPickSummon = new UnityEvent<Enums.ESummon>();
        RecieveBanPickSummon.AddListener((eSummon)=>RecieveBanPickRequest(eSummon));
    }
    private void InitializeMatchState()
    {
        switch (PlayerNum)
        {
            case 2:
                _matchStateList = TwoPlayerMatchOrder;
                break;
            case 3:
                _matchStateList = ThreePlayerMatchOrder;
                break;
            case 4:
                _matchStateList = FourPlayerMatchOrder;
                break;
        }

        _matchStateIndex = 0;

        SetMatchState(_matchStateList[_matchStateIndex]);
    }

    private void InitiateSelectPlayer()
    {
        Debug.Log("MatchManager: Initiate Select Player");

    }
    private void InitiateSelectStage()
    {
        Debug.Log("MatchManager: Initiate Select Stage");

    }
    private void InitiateBan()
    {
        Debug.Log("MatchManager: Initiate Ban");
        
    }
    private void InitiatePick()
    {
        Debug.Log("MatchManager: Initiate Pick");
        
    }
    private void InitiaiteSetPair()
    {
        Debug.Log("MatchManager: Initiate Set Pair");

    }
    private void InitiateSelectStrategy()
    {
        Debug.Log("MatchManager: Initiate Select Strategy");

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
public struct MatchStatePair
{
    public Enums.EBanPickState State { get; set; }
    public Enums.ETeam Turn { get; set; }
}