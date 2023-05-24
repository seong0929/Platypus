using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UserState User;
    private TeamManager TeamManager;
    private CoachManager CoachManager;
    private PlayerManager PlayerManager;

    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Other GameManager variables and methods...

    private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NewGame()
    {
        // initialing UserState
        User = new UserState();

        TeamManager = new TeamManager();
        CoachManager = new CoachManager();
        PlayerManager = new PlayerManager();

        User.Coach = CoachManager.CreateCoach("User", 1);
        User.Team = TeamManager.CreateTeam("User's Team");

//        BuildFilledTeam();
        Team Opponent = BuildFilledTeam();

        PlayerManager.CreatePlayers(10, 1);

        // FILL USER'S TEAM with RANDOM Players
        Team UserTeam = User.Team;
        UserTeam.ScoutPlayers(PlayerManager.CreatePlayers(5, 1, 3));
        //FILL User's Roster
        UserTeam.AddPlayerOnRoster(UserTeam.Players[0]);
        UserTeam.AddPlayerOnRoster(UserTeam.Players[1]);

        //Fill Opponent's Roaster
        Opponent.AddPlayerOnRoster(Opponent.Players[0]);
        Opponent.AddPlayerOnRoster(Opponent.Players[1]);
        Opponent.AddPlayerOnRoster(Opponent.Players[2]);

        //Set Selected Players
    }

    //    private void BuildFilledTeam()
    private Team BuildFilledTeam()
    {
        Team team = TeamManager.CreateTeam();
        team.Coach = CoachManager.CreateCoach("Unknown", 1); // might cause Confusion

        List<Player> players = PlayerManager.CreatePlayers(3,1,2);
        team.ScoutPlayers(players);

        return team;
    }
}
