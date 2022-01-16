using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MySingleton<GameManager>
{
    public override bool DoDestroyOnLoad { get; }

    public Transform particleParent;

    
    void OnGUI () {
        GUI.contentColor = Color.black;
        GUILayout.Label(Time.deltaTime.ToString());
    }
}
