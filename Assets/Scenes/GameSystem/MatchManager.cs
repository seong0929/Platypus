using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MatchManager
{
    public int PlayerNum;
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
    public List<Enums.ESummon> PickableSummons;
    private List<Enums.ESummon> _unselectableSummons;

    public MatchManager()
    {
    }

    public void InitializeMatch(int playerNum, Enums.EStage stage, Team teamA, Team teamB)
    {
        PlayerNum = playerNum;
        Stage = stage;

        GroupA = new Group(teamA);
        GroupA.TeamSide = Enums.ETeam.TeamA;

        GroupB = new Group(teamB);
        GroupB.TeamSide = Enums.ETeam.TeamB;

        Enums.ESummon[] tmpPickableSummons = (Enums.ESummon[]) Enum.GetValues(typeof(Enums.ESummon));
        SetPickableSummons(tmpPickableSummons.ToList());
    }

    public void SetSelectedPlayer(Enums.ETeam teamSide, List<Player> selectedOne)
    {
        Group targetGroup = GetGroup(teamSide);

        targetGroup.SelectedPlayers = selectedOne;
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
}

public class Group
{
    public Enums.ETeam TeamSide;
    public Team Team;
    public List<Player> SelectedPlayers;
    public List<Enums.ESummon> SelectedSummon;
    public List<Enums.ESummon> BannedSummon;
    public Dictionary<Player, Enums.ESummon> PlayerSummonPair;

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