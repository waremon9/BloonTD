using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MySingleton<CheatManager>
{
    public override bool DoDestroyOnLoad { get; }

    private EnemiesManager EM;
    
    [Header("Single spawn")]
    public BasicBalloonScriptable singleEnemy;
    
    [Header("Multi spawn")]
    public BasicBalloonScriptable multiEnemies;
    public int qte;


    private void Start()
    {
        EM = EnemiesManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            StartCoroutine(BalloonSpawnCoroutine());
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EM.EnemieSpawnAtStart(singleEnemy);
        }
        if (Input.GetKey(KeyCode.Keypad0))
        {
            EM.EnemieSpawnAtStart(singleEnemy);
        }
    }

    private IEnumerator BalloonSpawnCoroutine()
    {
        for (int i = 0; i < qte; i++)
        {
            EnemiesManager.Instance.EnemieSpawnAtStart(multiEnemies);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
}
