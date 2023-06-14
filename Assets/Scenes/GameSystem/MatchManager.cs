using System.Collections.Generic;

public class MatchManager
{
    public int PlayerNum;

    public Group GroupA;
    public Group GroupB;

//    private int _playerNum;

    public MatchManager(int playerNum, Team teamA, Team teamB)//Group a, Group b)//, int playerNum)
    {
        PlayerNum = playerNum;

        GroupA = new Group(teamA);
        GroupB = new Group(teamB);

        //        GroupA = a;
        //        GroupB = b;
        //        _playerNum = playerNum;
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

    public Group(Team team)//, List<Player> selectedOne)
    {
        Team = team;
        SelectedPlayers = new List<Player>();
        SelectedSummon = new List<Enums.ESummon>();
        BannedSummon = new List<Enums.ESummon>();
    }
}