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



//        CoachManager.CreateCoach(coachNum: 1, level: 1);
//        TeamManager.CreateTeams();

    }
}
