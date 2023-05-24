using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    //PlayerLimit
    const int MAX_PLAYER = 10;
    const int MAX_ROSTER = 6;

    public string Name { get; set; } // The name of the team
    public Enums.ELeague League { get; set; } // The league the team belongs to -- might need furthur disccusion --- needs ENUM
    public int Reputation { get; set; } // The reputation of the team 


    // Money
    public int Money { get; set; } // The amount of money the team has
    // Crystal
    public int Crystal { get; set; } // The number of magic crystals the team has
    // Sponsor
    public Enums.ESponsor Sponsor { get; set; } // The sponsor of the team

    public Team(string name = "Unknown", Enums.ELeague league = Enums.ELeague.Amature)
    {
        Name = name;
        League = league;
    }

    // Coach 
    public Coach Coach { get; set; } // The coach of the team

    // Players
    public List<Player> Players { get; set; } // The list of players in the team

    // Roster
    public List<Player> Roster { get; set; } // The current roster for the team

    // Equipment
    public List<Enums.EEquipment> Equipment { get; set; } // The equipment owned by the team
    public void ScoutPlayer(Player player)
    {
        if (MAX_PLAYER <= Players.Count)
        {
            Debug.LogWarning("Team is already Fulled! the player can not be scouted");
            return;
        }
        if (player = null)
        {
            Debug.LogWarning("The player is invalid, can not be scouted");
            return;
        }

        Players.Add(player);
        player.Team = this;
        return;
    }
    public void ReleasePlayer(Player player)
    {
        if (Players.Contains(player)!)
        {
            Debug.LogWarning("No Such player in this team. The player can not be released");
            return;
        }

        Players.Remove(player);
        player.Team = null;
        return;
    }

    public void AddPlayerOnRoaster(Player player)
    {
        if (Players.Contains(player)!)
        {
            Debug.LogWarning("No Such player in this team. The player can not be one the roaster");
            return;
        }
        if (MAX_ROSTER <= Players.Count)
        {
            Debug.LogWarning("Roaster is already Fulled!");
            return;
        }

        Players.Add(player);
        return;
    }
    public void RemovePlayerOnRoster(Player player)
    {
        if (Roster.Contains(player)!)
        {
            Debug.LogWarning("No Such player in the Roaster");
            return;
        }

        Roster.Remove(player);
        return;
    }

    public bool IsRosterValid(int minNum)
    {
        if (Roster.Count < minNum)
        {
            return false;
        }
        return true;
    }

}