using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SplineMesh;
using UnityEngine;

public class BaseBalloon : MonoBehaviour
{
    public BasicBalloonScriptable basicBalloonBaseData;
    public FollowSpline followSpline;
    public float hitBoxRadius;
    public int hpActual;

    public List<BaseProjectile> ProjectilesHit;
    
    public ObjectPool poolRef;

    public void UpdateStats(BasicBalloonScriptable basicBalloonStats)
    {
        basicBalloonBaseData = basicBalloonStats;
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = basicBalloonBaseData.sprite;
        }
        

        int remainingDamage = hpActual * -1;
        
        hpActual = basicBalloonBaseData.hp;
        transform.localScale = Vector2.one * basicBalloonBaseData.size;
        
        if(remainingDamage>=1) Hit(remainingDamage);
    }
    
    protected virtual void Start()
    {
        if (!followSpline)
        {
            if(!TryGetComponent(out followSpline)) Debug.Log("Balloon has no 'FollowSpline' component. " + name);
        }

        if (TryGetComponent(out CircleCollider2D collider))
        {
            hitBoxRadius = collider.radius * transform.localScale.x;
            collider.enabled = false;
        }

        poolRef = EnemiesManager.Instance.poolPopParticle;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Hit(1);
        }
    }
    
    virtual public void Hit(int damage)
    {
        if(damage == 0) return;
        
        TakeDamage(damage);
        if(hpActual<=0) LayerPop(hpActual * -1);
    }

    protected virtual void TakeDamage(int damage)
    {
        hpActual -= damage;
    }
    
    public bool IsResistant(ProjectileType type)
    {
        return basicBalloonBaseData.resistance.Contains(type);
    }

    protected void PlayPopEffect()
    {
        if(poolRef == null) poolRef = EnemiesManager.Instance.poolPopParticle;
        GameObject poolObject = poolRef.GetFromPool();
        ParticleSystem partSys = poolObject.GetComponent<ParticleSystem>();
        partSys.transform.position = transform.position;
        partSys.Play();
        EnemiesManager.Instance.particleNumber++;
    }
    
    protected virtual void LayerPop(int damage)
    {
        PlayPopEffect();
        
        GameManager.Instance.AddMoney();
        
        if (basicBalloonBaseData.IsLastLayer())
        {
            Death();
            return;
        }
        
        
        if (basicBalloonBaseData.MoreThanOneOnRelease())
        {
            bool firstSkipped = false;
            int nb = 1;
            foreach (ReleaseOnDeath data in basicBalloonBaseData.releaseOnDeath)
            {
                for (int i = 0; i < data.qte; i++)
                {
                    if (!firstSkipped)
                    {
                        firstSkipped = true;
                        continue;
                    }
                    
                    EnemiesManager.Instance.EnemieSpawnFromRelease(data.basicBalloon,this, nb * 0.3f, damage);
                    nb++;
                }
            }
        }
        
        UpdateStats(basicBalloonBaseData.releaseOnDeath[0].basicBalloon);
    }

    protected void Death()
    {
        EnemiesManager.Instance.BalloonDead(this);
    }

    public float GetWorlHitBoxRadius()
    {
        return hitBoxRadius * transform.localScale.x;   
    }

    public bool NewProjectileHit(BaseProjectile proj)
    {
        if (ProjectilesHit.Contains(proj))
        {
            return false;
        }
        else
        {
            ProjectilesHit.Add(proj);
            return true;
        }
        
    }

    public int GetBalloonRBE()
    {
        return RBERecursive(basicBalloonBaseData);
    }

    public static int GetBalloonRBE(BasicBalloonScriptable balloonData)
    {
        return RBERecursive(balloonData);
    }

    private static int RBERecursive(BasicBalloonScriptable b)
    {
        int RBE = b.hp;

        foreach (ReleaseOnDeath bDeath in b.releaseOnDeath)
        {
            RBE += RBERecursive(bDeath.basicBalloon) * bDeath.qte;
        }

        return RBE;
    }
}
