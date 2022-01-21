using System;
using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class EnemiesManager : MySingleton<EnemiesManager>
{
    public override bool DoDestroyOnLoad { get; }

    private List<BaseBalloon> allBalloons = new List<BaseBalloon>();
    
    [Header("Level path")]
    public Spline path;
    
    [Header("Parent transform")]
    public Transform enemiesParent;

    [Header("Waves")]
    [SerializeField] private AllWaves allWaves;
    [SerializeField] private int waveNumber=0;
    private bool coroutineSendWaveEnded = true;

    [Header("Prefab")]
    public BaseBalloon balloonPrefab;
    
    [Header("Particles")]
    [SerializeField] public ParticleSystem BalloonPopEffect;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StopAllCoroutines();
            coroutineSendWaveEnded = true;
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            CallNextWave();
        }
    }

    public void CallNextWave()
    {
        if(!IsWaveFinished()) return;
        
        StartCoroutine(SendWaveCoroutine(allWaves.allWaves[waveNumber]));
        waveNumber++;
        if (waveNumber >= allWaves.allWaves.Count) waveNumber = allWaves.allWaves.Count - 1;
    }

    private IEnumerator SendWaveCoroutine(AllWaves.SingleWave wave)
    {
        coroutineSendWaveEnded = false;
        
        foreach (AllWaves.EnemyGroup enemyGroup in wave.allgroup)
        {
            yield return StartCoroutine(SendEnemyGroupCoroutine(enemyGroup));
        }
        
        coroutineSendWaveEnded = true;
    }

    public bool IsWaveFinished()
    {
        return coroutineSendWaveEnded && allBalloons.Count == 0;
    }
    
    private IEnumerator SendEnemyGroupCoroutine(AllWaves.EnemyGroup group)
    {
        yield return new WaitForSeconds(group.firstInterval);
        
        for (int i = 0; i < group.qte; i++)
        {
            EnemieSpawnAtStart(group.enemyType);
            yield return new WaitForSeconds(group.loopInterval);
        }
    }

    //To spawn new balloon
    public void EnemieSpawnAtStart(BasicBalloonScriptable scriptable)
    {
        BaseBalloon bb = Instantiate(balloonPrefab, enemiesParent);
        
        bb.UpdateStats(scriptable);
        allBalloons.Add(bb);
    }
    
    //To spawn balloon on balloon death
    public void EnemieSpawnFromRelease(BasicBalloonScriptable scriptable, BaseBalloon parent ,float offset, int damage)
    {
        BaseBalloon bb = Instantiate(balloonPrefab, enemiesParent);
        
        bb.UpdateStats(scriptable);
        bb.followSpline.dist = parent.followSpline.dist - offset;
        bb.ProjectilesHit = new List<BaseProjectile>(parent.ProjectilesHit);
        
        allBalloons.Add(bb);
        
        bb.Hit(damage);
    }

    public void BalloonDead(BaseBalloon bs)
    {
        allBalloons.Remove(bs);
    }

    public bool AtLeastOneBalloonAlive()
    {
        return allBalloons.Count > 0;
    }

    public bool AtLeastOneBalloonInRange(Vector3 position, float range)
    {
        foreach (BaseBalloon balloon in allBalloons)
        {
            float distance = Vector3.Distance(balloon.transform.position, position);
            float outOfRange = range + balloon.GetWorlHitBoxRadius();
            if( distance <= outOfRange) return true;
        }

        return false;
    }

    public List<BaseBalloon> GetAllBalloon()
    {
        return allBalloons;
    }

    public BaseBalloon GetFirstBalloonInRange(Vector3 position, float range)
    {
        BaseBalloon target = null;
        foreach (BaseBalloon balloon in allBalloons)
        {
            float distance = Vector3.Distance(balloon.transform.position, position);
            float outOfRange = range + balloon.GetWorlHitBoxRadius();
            
            if(distance > outOfRange) continue;

            if (!target) target = balloon;
            
            if (balloon.followSpline.dist > target.followSpline.dist)
            {
                target = balloon;
            }
        }

        return target;
    }
    public BaseBalloon GetLastBalloonInRange(Vector3 position, float range)
    {
        BaseBalloon target = allBalloons[0];
        foreach (BaseBalloon balloon in allBalloons)
        {
            if(Vector3.Distance(balloon.transform.position, position) > range + balloon.GetWorlHitBoxRadius()) continue;
            
            if (balloon.followSpline.dist < target.followSpline.dist)
            {
                target = balloon;
            }
        }

        return target;
    }
    public BaseBalloon GetClosestBalloonInRange(Vector3 position, float range)
    {
        BaseBalloon target = allBalloons[0];
        float targetDistance = Vector3.Distance(target.transform.position, position);
        
        foreach (BaseBalloon balloon in allBalloons)
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
