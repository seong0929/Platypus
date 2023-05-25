using System;
using System.IO;
using UnityEngine;

[Serializable]
public class UserState
{
    public Coach Coach;
    public string Name;
    public Team Team;

    public UserState()
    {
        Coach = null;
        Name = null;
        Team = null;
    }
}