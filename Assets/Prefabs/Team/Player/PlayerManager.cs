using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Player> Players;

    // To Do: put thses somewhere else
    private static string[] firstNameList = { "John", "Emma", "Michael", "Sophia", "William", "Olivia" };
    private static string[] lastNameList = { "Smith", "Johnson", "Brown", "Jones", "Davis", "Miller" };

    public string GenerateRandomName()
    {
        string firstName = GetRandomName(firstNameList);
        string lastName = GetRandomName(lastNameList);

        return firstName + " " + lastName;
    }
    private string GetRandomName(string[] nameList)
    {
        int randomIndex = UnityEngine.Random.Range(0, nameList.Length);
        return nameList[randomIndex];
    }

    // To Do: Add more parameter - will be needed for the depenedency
    public void CreatePlayer()
    {
        string name = GenerateRandomName();
        int level = 1;
        Player player = new Player(name, level);
        Players.Add(player);
    }

    public void CreatePlayer(int num)
    {
        for(int i = 0; i < num; i++)
        {
            CreatePlayer();
        }
    }

}
