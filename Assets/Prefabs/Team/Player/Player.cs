using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string Name { get; set; } // The name of the Coach
    public int level { get; set; }
    public List<Enums.EStrategy> Strategies { get; set; }

    public List<Enums.ETrait> Traits;

    public int Attack;
    public int Defense;

    public Dictionary<Enums.ESummon, int> Proficiency { get; set; } // The player's proficiency for each summon
}
