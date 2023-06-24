using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UserState User;
    public TeamManager TeamManager;
    public MatchManager MatchManager;
    private CoachManager _coachManager;
    private PlayerManager _playerManager;
    //    private MatchManager _matchManager;

    // Singleton instance
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
        _coachManager = new CoachManager();
        _playerManager = new PlayerManager();

        User.Coach = _coachManager.CreateCoach("User", 1);
        User.Team = TeamManager.CreateTeam("User's Team");

        for (int i = 0; i<3; i++)
        {
            BuildFilledTeam();
        }

        _playerManager.CreatePlayers(10, 1, TeamManager.FAs);
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
    //    private void BuildFilledTeam()
    private Team BuildFilledTeam()
    {
        Team team = TeamManager.CreateTeam();
        team.Coach = _coachManager.CreateCoach("Unknown", 1); // might cause Confusion

        List<Player> players = _playerManager.CreatePlayers(3,1,2);
        team.ScoutPlayers(players);

        return team;
    }
}
