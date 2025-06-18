using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public record PlayerData
{
    public string Name;
    public long Score;
    public string  Email;
    public string  Number;
    

    public PlayerData(string name, long score, string number = default,string email = default)
    {
        Name = name;
        Score = score;
        Email = email;
        Number = number;
    }
}

