using System.Collections.Generic;
using UnityEngine;

public class CoachManager// : MonoBehaviour
{
    private List<Coach> _coaches;

    public CoachManager()
    {
        _coaches = new List<Coach>();
    }
    public void CreateCoach(int coachNum = 1, int level = 1, List<Enums.EStrategy> strategies = null)
    {
        for (int i = 0; i < coachNum; i++)
        {
            string name = GetRandomName(NameLists.CoachNames);

            Coach coach = new Coach(name, level, strategies);
            _coaches.Add(coach);
        }
    }
    public Coach CreateCoach(string name = "Unknown", int level = 1)
    {
        if(name == "Unknown")
        {
            name = GetRandomName(NameLists.CoachNames);
        }

        Coach coach = new Coach(name, level);

        _coaches.Add(coach);

        return coach;
    }
    private string GetRandomName(string[] nameList)
    {
        int randomIndex = Random.Range(0, nameList.Length);
        return nameList[randomIndex];
    }
}