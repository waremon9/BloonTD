using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class BasicBalloonScriptable : ScriptableObject
{
    [Header("Graphic")]
    public Sprite sprite;
    public float size;
    [Header("Stats")]
    public float speed;
    public int hp = 1;
    public ProjectileType[] resistance;
    [Header("LayerPop")]
    public ReleaseOnDeath[] releaseOnDeath;
    
    public bool MoreThanOneOnRelease()
   {
       return  releaseOnDeath.Length > 0 && releaseOnDeath[0].qte > 1 || releaseOnDeath.Length > 1;
   }

    public bool IsLastLayer()
    {
        return releaseOnDeath.Length == 0;
    }
}

[Serializable]
public struct ReleaseOnDeath
{
    public int qte;
    public BasicBalloonScriptable basicBalloon;

    
}
