using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Player> Players;

    public PlayerManager()
    {
        Players = new List<Player>();
    }

    public string GenerateRandomName()
    {
        string firstName = GetRandomName(NameLists.PlayerFirstNames);
        string lastName = GetRandomName(NameLists.PlayerLastNames);

        return firstName + " " + lastName;
    }

    private string GetRandomName(string[] nameList)
    {
        int randomIndex = UnityEngine.Random.Range(0, nameList.Length);
        return nameList[randomIndex];
    }

    public void CreatePlayers(int num = 1, int level = 1)
    {
        for (int i = 0; i < num; i++)
        {
            string name = GenerateRandomName();
            Player player = new Player(name, level);

            Players.Add(player);
        }
    }

    public List<Player> CreatePlayers(int num = 1, int levelMin = 1, int levelMax =1)
    {
        List<Player> players = new List<Player>();

        for (int i = 0; i < num; i++)
        {
            string name = GenerateRandomName();
            int level = UnityEngine.Random.Range(levelMin, levelMax);

            Player player = new Player(name, level);

            Players.Add(player);
            players.Add(player);
        }

        return players;
    }
}
