using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    private Group groupA;
    private Group groupB;

//    private int PlayerNum;

    MatchManager(Group a, Group b)//, int playerNum)
    {
        groupA = a;
        groupB = b;
//        PlayerNum = playerNum;
    }
    public void SetSelectedPlayer(bool isA, List<Player> selectedOne)
    {

    }
    public void SetBanPick()
    {

    }
    /*
    Team (coach, player, roster), 
    n:n(playerNum), 
    Selected Player, 
    Ban&Pick, 
    wiring player&Summon
     */ 

}

public class Group
{
    public Team Team;
    public List<Player> SelectedPlayers;
    public List<Enums.ESummon> SelectedSummon;
    public List<Enums.ESummon> BannedSummon;

    Group(Team team, List<Player> selectedOne)
    {
        Team = team;
        SelectedPlayers = selectedOne;
        SelectedSummon = new List<Enums.ESummon>();
        BannedSummon = new List<Enums.ESummon>();
    }

}