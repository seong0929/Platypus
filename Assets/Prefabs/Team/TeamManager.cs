using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    private List<Team> Teams;

    // To Do: put thses somewhere else
    private static string[] TeamNameList = { "Seoul", "NewYork", "London", "Jeju", "Tokyo", "Paris" };

    public string GenerateRandomName()
    {
        int randomIndex = UnityEngine.Random.Range(0, TeamNameList.Length);
        string name = TeamNameList[randomIndex];

        return name;
    }

    public void CreateTeam()
    {
        string name = GenerateRandomName();
        Team team = new Team(name);
        Teams.Add(team);
    }
    public void CreateTeam(int num)
    {
        for (int i = 0; i < num; i++)
        {
            CreateTeam();
        }
    }
}
