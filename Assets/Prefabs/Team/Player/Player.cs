using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public string Name { get; set; } // The name of the Player
    public int Level { get; set; }
    public List<Enums.EStrategy> Strategies { get; set; }

    public List<Enums.ETrait> Traits;

    public int Attack;
    public int Defense;

    public Dictionary<Enums.ESummon, int> Proficiency { get; set; } // The player's proficiency for each summon

    public Player(string name, int level)
    {
        Name = name;
        Level = level;

        Attack = 1;
        Defense = 1;

    }
}
