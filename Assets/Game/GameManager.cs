using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UserState User;
    private TeamManager TeamManager;
    private CoachManager CoachManager;
    private PlayerManager PlayerManager;



    public void NewGame()
    {
        // initialing UserState
        User = new UserState();

        TeamManager = new TeamManager();
        CoachManager = new CoachManager();
        PlayerManager = new PlayerManager();

        User.Coach = CoachManager.CreateCoach("User", 1);
        User.Team = TeamManager.CreateTeam("User's Team");

        BuildFilledTeam();

        PlayerManager.CreatePlayers(10, 1);

    }

    private void BuildFilledTeam()
    {
        Team team = TeamManager.CreateTeam();
        team.Coach = CoachManager.CreateCoach("Unknown", 1); // might cause Confusion

        List<Player> players = PlayerManager.CreatePlayers(3,1,2);
        team.ScoutPlayers(players);
    }
}
