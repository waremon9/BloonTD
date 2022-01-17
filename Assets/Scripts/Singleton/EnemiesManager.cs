using System;
using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class EnemiesManager : MySingleton<EnemiesManager>
{
    public override bool DoDestroyOnLoad { get; }

    public Spline path;

    public BalloonScriptable e;
    public Transform enemiesParent;

    [SerializeField] private AllWaves allWaves;
    [SerializeField] private int waveNumber=0;
    private bool waveEnded = true;

    [SerializeField] public ParticleSystem BalloonPopEffect;

    private List<NewBalloonLogic> allBalloons = new List<NewBalloonLogic>();

    public NewBalloonLogic balloonPrefab;

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
    private void EnemieSpawn(BalloonScriptable balloon)
    {
        NewBalloonLogic bs = Instantiate(balloonPrefab, enemiesParent);
        bs.UpdateStats(balloon);
        
        allBalloons.Add(bs);
    }
    //To spawn balloon on balloon death
    public void EnemieSpawn(BalloonScriptable balloon, float rate, int damage)
    {
        NewBalloonLogic bs = Instantiate(balloonPrefab, enemiesParent);
        bs.UpdateStats(balloon);
        
        allBalloons.Add(bs);
        
        bs.Hit(damage);
    }

    public void BalloonDead(NewBalloonLogic bs)
    {
        allBalloons.Remove(bs);
    }

    public bool AtLeastOneBalloonAlive()
    {
        return allBalloons.Count > 0;
    }

    public bool AtLeastOneBalloonInRange(Vector3 position, float range)
    {
        foreach (NewBalloonLogic balloon in allBalloons)
        {
            if(Vector3.Distance(balloon.transform.position, position) <= range + balloon.GetWorlHitBoxRadius()) return true;
        }

        return false;
    }

    public List<NewBalloonLogic> GetAllBalloon()
    {
        return allBalloons;
    }

    public NewBalloonLogic GetFirstBalloonInRange(Vector3 position, float range)
    {
        NewBalloonLogic target = allBalloons[0];
        foreach (NewBalloonLogic balloon in allBalloons)
        {
            if(Vector3.Distance(balloon.transform.position, position) > range + balloon.GetWorlHitBoxRadius()) continue;
            
            if (balloon.followSpline.dist > target.followSpline.dist)
            {
                target = balloon;
            }
        }

        return target;
    }
    public NewBalloonLogic GetLastBalloonInRange(Vector3 position, float range)
    {
        NewBalloonLogic target = allBalloons[0];
        foreach (NewBalloonLogic balloon in allBalloons)
        {
            if(Vector3.Distance(balloon.transform.position, position) > range + balloon.GetWorlHitBoxRadius()) continue;
            
            if (balloon.followSpline.dist < target.followSpline.dist)
            {
                target = balloon;
            }
        }

        return target;
    }
    public NewBalloonLogic GetClosestBalloonInRange(Vector3 position, float range)
    {
        NewBalloonLogic target = allBalloons[0];
        float targetDistance = Vector3.Distance(target.transform.position, position);
        
        foreach (NewBalloonLogic balloon in allBalloons)
        {
            float distance = Vector3.Distance(balloon.transform.position, position);
            if(distance <= range + balloon.GetWorlHitBoxRadius() && targetDistance>distance)
            {
                target = balloon;
                targetDistance = distance;
            }
        }

        return target;
    }
}
