using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UserState User;
    public TeamManager TeamManager;
    public MatchManager MatchManager;
    private CoachManager CoachManager;
    private PlayerManager PlayerManager;
    //    private MatchManager MatchManager;

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

            NewGame();
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

        for (int i = 0; i<3; i++)
        {
            BuildFilledTeam();
        }

        PlayerManager.CreatePlayers(10, 1, TeamManager.FAs);
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
    public void MakeMatch()
    {
        // get from schedule or something
        // ---- START: FOR THE TEST --- //
        Team Opponent;
        Opponent = TeamManager.Teams[2];
        // ---- END: FOR THE TEST --- //

        MatchManager = new MatchManager(2, User.Team, Opponent);

        // ---- START: FOR THE TEST --- //
        List<Player> opponentSelected = new List<Player>();
        opponentSelected.Add(Opponent.Players[0]);
        opponentSelected.Add(Opponent.Players[1]);
        MatchManager.GroupB.SelectedPlayers = opponentSelected;
        // ---- END: FOR THE TEST --- //
    }
}
