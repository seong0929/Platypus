using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player //: MonoBehaviour
{
    
    public string Name { get; set; } // The name of the Player
    public int Level { get; set; }

    public Team Team { get; set; }

    public List<Enums.EStrategy> Strategies { get; set; }
    public List<Enums.ETrait> Traits;
    public int Attack;
    public int Defense;
    public Dictionary<Enums.ESummon, int> Proficiency { get; set; } // The player's proficiency for each summon


    public Player(string name = "Unknown", int level = 1, Team team = null)
    {
        Name = name;
        Level = level;
        Team = team;

        SetSkillByLevel(level);
    }

    private void SetSkillByLevel(int level)
    {
        Attack = level;
        Defense = level;

        Proficiency = new Dictionary<Enums.ESummon, int>();
        Proficiency.Add(Enums.ESummon.SenorZorro, level);
        /*
         * Strategies
         * Traits
         */
    }

}
