using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummonMonitor : MonoBehaviour
{

    public List<Summon> Summons = new List<Summon>();
    public static UnityEvent<List<Summon>> RegisterSummon = new UnityEvent<List<Summon>>();

    private void Awake()
    {
        InitEvents();
    }

    private void AddSummon(List<Summon> summons)
    {
        Summons.AddRange(summons);
    }
    private void InitEvents()
    {
        RegisterSummon.AddListener((summons) => AddSummon(summons));
    }
}
