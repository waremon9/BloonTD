using System;
using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EnemiesManager : MySingleton<EnemiesManager>
{
    public override bool DoDestroyOnLoad { get; }

    private List<BaseBalloon> allBalloons = new List<BaseBalloon>();
    
    [Header("Level path")]
    public Spline path;
    
    [Header("Waves")]
    [SerializeField] private RoundText roundText;
    [SerializeField] private AllWaves allWaves;
    public int waveNumber=0;

    [Header("balloon")]
    public BaseBalloon balloonPrefab;
    public ObjectPool poolBalloon;

    [Header("Particles")]
    [SerializeField] public ParticleSystem BalloonPopEffect;
    public ObjectPool poolPopParticle;

    [Header("Event")]
    public UnityEvent OnEndOfGame;

    private GameManager GM;

    [HideInInspector] public int particleNumber = 0;
    
    private void Start()
    {
        GM = GameManager.Instance;
        poolBalloon.Initialize(balloonPrefab.gameObject);
        poolPopParticle.Initialize(BalloonPopEffect.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StopAllCoroutines();
            GM.gameState = GameState.NoWave;
            UIManager.Instance.UpdateWaveButtonIcon();
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            CallNextWave();
        }
    }

    public void CallNextWave()
    {
        if(GM.gameState == GameState.WaveComing) return;
        
        StartCoroutine(SendWaveCoroutine(allWaves.allWaves[waveNumber]));
        waveNumber++;

        roundText.StartCoroutine(roundText.AnimateRoundText());
        
        if (waveNumber >= allWaves.allWaves.Count) waveNumber = allWaves.allWaves.Count - 1;
    }

    public void AutoLaunchNextWaveCheck()
    {
        if (GM.gameSpeed == GameSpeed.AutoLaunch)
        {
            CallNextWave();
        }
    }

    private IEnumerator SendWaveCoroutine(AllWaves.SingleWave wave)
    {
        GM.gameState = GameState.WaveComing;
        
        foreach (AllWaves.EnemyGroup enemyGroup in wave.allgroup)
        {
            yield return StartCoroutine(SendEnemyGroupCoroutine(enemyGroup));
        }
        
        yield return new WaitUntil(() => allBalloons.Count == 0);
        
        GM.gameState = GameState.NoWave;
        OnEndOfGame.Invoke();
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
        GameObject balloon = poolBalloon.GetFromPool();
        BaseBalloon baseBalloon = balloon.GetComponent<BaseBalloon>();;

        baseBalloon.UpdateStats(scriptable);
        baseBalloon.followSpline.dist = 0;
        baseBalloon.followSpline.UpdatePosition();
        baseBalloon.ProjectilesHit.Clear();
        allBalloons.Add(baseBalloon);
    }
    
    //To spawn balloon on balloon death
    public void EnemieSpawnFromRelease(BasicBalloonScriptable scriptable, BaseBalloon parent ,float offset, int damage)
    {
        GameObject balloon = poolBalloon.GetFromPool();
        BaseBalloon baseBalloon = balloon.GetComponent<BaseBalloon>();
        
        baseBalloon.UpdateStats(scriptable);
        baseBalloon.followSpline.dist = parent.followSpline.dist - offset;
        baseBalloon.followSpline.UpdatePosition();
        baseBalloon.ProjectilesHit = new List<BaseProjectile>(parent.ProjectilesHit);
        
        allBalloons.Add(baseBalloon);
        
        baseBalloon.Hit(damage);
    }

    public void BalloonDead(BaseBalloon bs)
    {
        allBalloons.Remove(bs);
        
        poolBalloon.ReturnToPool(bs.gameObject);
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

    public void ParticlePopBackToPool(GameObject obj)
    {
        poolPopParticle.ReturnToPool(obj);
        particleNumber--;
    }
}
