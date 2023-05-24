using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coach : MonoBehaviour
{
    public string Name { get; set; } // The name of the Coach
    public int Level { get; set; }
    public List<Enums.EStrategy> Strategies { get; set; }

    public Coach(string name = "Unknown", int level = 1, List<Enums.EStrategy> strategies = null )
    {
        Name = name;
        Level = level;
        Strategies = strategies ?? new List<Enums.EStrategy>();
    }

}