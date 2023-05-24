using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team //: MonoBehaviour
{
    //PlayerLimit
    const int MAX_PLAYER = 10;
    const int MAX_ROSTER = 6;

    public string Name { get; set; }
    public Enums.ELeague League { get; set; }
    public int Reputation { get; set; }
    public int Money { get; set; }
    public int Crystal { get; set; }
    public Enums.ESponsor Sponsor { get; set; }
    public Coach Coach { get; set; }
    public List<Player> Players { get; set; }
    public List<Player> Roster { get; set; }
    public List<Enums.EEquipment> Equipment { get; set; }

    public Team(string name = "Unknown", Enums.ELeague league = Enums.ELeague.Amature)
    {
        Name = name;
        League = league;
        Players = new List<Player>();
        Roster = new List<Player>();
    }

    public void ScoutPlayer(Player player)
    {
        if (Players.Count >= MAX_PLAYER)
        {
            Debug.LogWarning("Team is already full! The player cannot be scouted.");
            return;
        }
        if (player == null)
        {
            Debug.LogWarning("The player is invalid. The player cannot be scouted.");
            return;
        }

        player.Team?.ReleasePlayer(player);
        Players.Add(player);
        player.Team = this;
    }

    public void ScoutPlayers(List<Player> players)
    {
        if (Players.Count + players.Count > MAX_PLAYER)
        {
            Debug.LogWarning("Too many players to fit in! The players cannot be scouted.");
            return;
        }

        foreach (Player player in players)
        {
            player.Team?.ReleasePlayer(player);
            Players.Add(player);
            player.Team = this;
        }
    }

    public void ReleasePlayer(Player player)
    {
        if (!Players.Contains(player))
        {
            Debug.LogWarning("No such player in this team. The player cannot be released.");
            return;
        }

        player.Team = null;
        Players.Remove(player);
    }

    public void AddPlayerToRoster(Player player)
    {
        if (!Players.Contains(player))
        {
            Debug.LogWarning("No such player in this team. The player cannot be on the roster.");
            return;
        }
        if (Roster.Count >= MAX_ROSTER)
        {
            Debug.LogWarning("Roster is already full!");
            return;
        }

        Roster.Add(player);
    }
    public void RemovePlayerFromRoster(Player player)
    {
        if (!Roster.Contains(player))
        {
            Debug.LogWarning("No such player in the roster.");
            return;
        }

        Roster.Remove(player);
    }

    public bool IsRosterValid(int minNum)
    {
        return Roster.Count >= minNum;
    }

}