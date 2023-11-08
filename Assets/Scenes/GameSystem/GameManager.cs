using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // User's State, Team and Coach
    public UserState UserState;
    private Team userTeam;
    private Coach userCoach;

    // Managers of Team, Coach, Player
    public TeamManager TeamManager;
    public CoachManager CoachManager;
    public PlayerManager PlayerManager;

    // Managers of Match and Schedular
    public MatchManager MatchManager;
    public Schedular Schedular;

    public Group GroupA;
    public Group GroupB;

    // Singleton instance
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            NewGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NewGame()
    {
        // initialing Managers
        TeamManager = new TeamManager();
        CoachManager = new CoachManager();
        PlayerManager = new PlayerManager();
        // Set GameManager to other Managers
        TeamManager.GameManager = this;
        //CoachManager.GameManager = this;
        //PlayerManager.GameManager = this;

        // initialing UserState
        UserState = new UserState();
        UserState.Coach = CoachManager.CreateCoach("User", 1);
        UserState.Team = TeamManager.CreateTeam("User's Team");

        userTeam = UserState.Team;
        userCoach = UserState.Coach;

        //Build 3 other teams
        for (int i = 0; i<3; i++)
        {
            TeamManager.CreateFilledTeam();
        }

        //Build 10 FA players
        PlayerManager.CreatePlayers(10, 1, TeamManager.FAs);
    }

    //Build a Match (Temporary patches)
    public void MakeMatch()
    {
        // get from schedule or something
        // ---- START: FOR THE TEST --- //
        Team Opponent;
        Opponent = TeamManager.Teams[2];
        // ---- END: FOR THE TEST --- //


        MatchManager = new MatchManager();
        MatchManager.InitializeMatch(2, userTeam, Opponent);

        // ---- START: FOR THE TEST --- //
        List<Player> opponentSelected = new List<Player>();
        opponentSelected.Add(Opponent.Players[0]);
        opponentSelected.Add(Opponent.Players[1]);
        MatchManager.GroupB.SelectedPlayers = opponentSelected;
        // ---- END: FOR THE TEST --- //
    }

    public Team GetUserTeam()
    {
        return userTeam;
    }
}
