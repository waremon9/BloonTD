using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MySingleton<PlayerManager>
{
    public override bool DoDestroyOnLoad { get; }

    private Player actualPlayer;
    public string playerName = "Waremon";
    
    private void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            actualPlayer = new Player(data);
        }
        else
        {
            actualPlayer = new Player(playerName);
        }

        Debug.Log(actualPlayer.ToString());
    }
}

public class Player
{
    public string name;
    public int level;
    public int experience;
    public int balloonPop;

    public Player(string _name)
    {
        name = _name;
        level = 0;
        experience = 0;
        balloonPop = 0;
    }
    
    public Player(PlayerData data)
    {
        name = data.name;
        level = data.level;
        experience = data.experience;
        balloonPop = data.balloonPop;
    }

    public string ToString()
    {
        return "Player " + name + " is level " + level + " with " + experience + " exp.\n" +
               "He popped a total of " + balloonPop + " balloons.";
    }
}
