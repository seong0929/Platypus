using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public string Name { get; set; } // The name of the team
    public Enums.ELeague League { get; set; } // The league the team belongs to -- might need furthur disccusion --- needs ENUM
    public int Reputation { get; set; } // The reputation of the team 


    // Money
    public int Money { get; set; } // The amount of money the team has
    // Crystal
    public int Crystal { get; set; } // The number of magic crystals the team has
    // Sponsor
    public Enums.ESponsor Sponsor { get; set; } // The sponsor of the team



    // Coach 
    public Coach Coach { get; set; } // The coach of the team

    // Players
    public List<Player> Players { get; set; } // The list of players in the team

    // Roster
    public List<Player> Roster { get; set; } // The current roster for the team

    // Equipment
    public List<Enums.EEquipment> Equipment { get; set; } // The equipment owned by the team
}