using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager //: MonoBehaviour
{
    public List<Team> Teams;
    public Team FAs;

    public TeamManager()
    {
        Teams = new List<Team>();
        FAs = new Team("FA");
        Teams.Add(FAs);
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
}