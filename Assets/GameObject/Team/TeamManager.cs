using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager //: MonoBehaviour
{
    public List<Team> Teams;
    public Team FAs;
    public GameManager GameManager;

    public TeamManager()
    {
        Teams = new List<Team>();
        FAs = new Team("FA");
        Teams.Add(FAs);
    }
    
    private void Awake()
    {
        
    }

    public string GenerateRandomName(string[] nameList)
    {
        int randomIndex = UnityEngine.Random.Range(0, nameList.Length);
        string name = nameList[randomIndex];

        return name;
    }

//Making random teams
    public void CreateTeams(int num =1, Enums.ELeague league = Enums.ELeague.Amature)
    {
        for(int i =0; i < num; i++)
        {
            string name = GenerateRandomName(NameLists.TeamNames);
            Team team = new Team(name, league);

            Teams.Add(team);
        }
    }
    
    public Team CreateTeam(string name = "Unknown", Enums.ELeague league = Enums.ELeague.Amature)
    {
        if(name == "Unknown")
        {
            name = GenerateRandomName(NameLists.TeamNames);
        }

        Team team = new Team(name, league);

        Teams.Add(team);
        return team;
    }

    public void AddTeam(Team team)
    {
        Teams.Add(team);
    }

    //Create Filled Team
    public Team CreateFilledTeam()
    {

        Team team = CreateTeam();

        // Get CoachManager from GameManager and assign a coach to the team
        CoachManager coachManager = GameManager.CoachManager;
        if(coachManager == null) // If the coachManager is not initialized return null
        {
            Debug.LogError("CoachManager is not initialized!");
            return null;
        }
        team.Coach = coachManager.CreateCoach("Unknown", 1);

        // Get PlayerManager from GameManager and assign 3 players to the team
        PlayerManager playerManager = GameManager.PlayerManager;
        if (playerManager == null) // If the playerManager is not initialized return null
        {
            Debug.LogError("PlayerManager is not initialized!");
            return null;
        }
        List<Player> players = playerManager.CreatePlayers(3, 1, 2);
        team.ScoutPlayers(players);

        return team;
    }
}