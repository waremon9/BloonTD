using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(Player p)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.btd";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(p);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.btd";
        Debug.Log(path);
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            
            return data;
        }
        else
        {
            Debug.LogError("SaveFile not found in " + path);
            return null;
        }
    }
}


[Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public int experience;
    public int balloonPop;

    public PlayerData(Player p)
    {
        name = p.name;
        level = p.level;
        experience = p.experience;
        balloonPop = p.balloonPop;
    }
}