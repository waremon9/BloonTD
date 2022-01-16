using System;
using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class EnemiesManager : MySingleton<EnemiesManager>
{
    public override bool DoDestroyOnLoad { get; }

    [SerializeField] private Spline path;

    public BaseBalloon e;
    public Transform enemiesParent;

    [SerializeField] private AllWaves allWaves;
    [SerializeField] private int waveNumber=0;
    private bool waveEnded = true;

    [SerializeField] public ParticleSystem BalloonPopEffect;

    private List<BaseBalloon> allBalloons = new List<BaseBalloon>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnemieSpawn(e);
        }
        if (Input.GetKey(KeyCode.T))
        {
            EnemieSpawn(e);
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            StopAllCoroutines();
            waveEnded = true;
        }
        
        if (Input.GetKeyDown(KeyCode.P) && waveEnded)
        {
            StartCoroutine(SendWaveCoroutine(allWaves.allWaves[waveNumber]));
            waveNumber++;
            if (waveNumber >= allWaves.allWaves.Count) waveNumber = allWaves.allWaves.Count - 1;
        }
    }

    private IEnumerator SendWaveCoroutine(AllWaves.SingleWave wave)
    {
        waveEnded = false;
        
        foreach (AllWaves.EnemyGroup enemyGroup in wave.allgroup)
        {
            yield return StartCoroutine(SendEnemyGroupCoroutine(enemyGroup));
        }
        
        waveEnded = true;
    }
    
    private IEnumerator SendEnemyGroupCoroutine(AllWaves.EnemyGroup group)
    {
        yield return new WaitForSeconds(group.firstInterval);
        
        for (int i = 0; i < group.qte; i++)
        {
            EnemieSpawn(group.enemyType);
            yield return new WaitForSeconds(group.loopInterval);
        }
    }

    //To spawn new balloon
    private void EnemieSpawn(BaseBalloon balloon)
    {
        BaseBalloon bs = Instantiate(balloon, enemiesParent);
        bs.Initialize(path, bs.speed);
        
        allBalloons.Add(bs);
    }
    //To spawn balloon on balloon death
    public void EnemieSpawn(BaseBalloon balloon, float rate, int damage)
    {
        BaseBalloon bs = Instantiate(balloon, enemiesParent);
        bs.Initialize(path, bs.speed, rate);
        
        allBalloons.Add(bs);
        
        bs.Hit(damage);
    }

    public void BalloonDead(BaseBalloon bs)
    {
        allBalloons.Remove(bs);
    }

    public List<BaseBalloon> GetAllBalloon()
    {
        return allBalloons;
    }
}
