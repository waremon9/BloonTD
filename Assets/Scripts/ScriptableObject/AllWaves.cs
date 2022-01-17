using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave/AllWaves")]
public class AllWaves : ScriptableObject
{
    [Serializable]
    public struct EnemyGroup
    {
        public BasicBalloonScriptable enemyType;
        public  int qte;
        public float firstInterval;
        public float loopInterval;
    }
    
    [Serializable]
    public struct SingleWave
    {
        public List<EnemyGroup> allgroup;
    }
    
    public List<SingleWave> allWaves;
}