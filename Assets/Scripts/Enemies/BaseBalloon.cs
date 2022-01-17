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

    public void UpdateStats(BasicBalloonScriptable basicBalloonStats)
    {
        basicBalloonBaseData = basicBalloonStats;
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = basicBalloonBaseData.sprite;
        }

        hpActual = basicBalloonBaseData.hp;
        transform.localScale = Vector2.one * basicBalloonBaseData.size;
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
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Hit();
        }
    }
    
    virtual public void Hit(int damage = 1)
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
        if (EnemiesManager.Instance.BalloonPopEffect && GameManager.Instance.particleParent)
        {
            ParticleSystem temp = Instantiate(EnemiesManager.Instance.BalloonPopEffect, transform.position,
                Quaternion.identity, GameManager.Instance.particleParent);
        }
        if (!EnemiesManager.Instance.BalloonPopEffect)
        {
            Debug.LogError("Missing particle system prefab : " + name);
        }
        if (!GameManager.Instance.particleParent)
        {
            Debug.LogError("No particle parent reference in gameManager");
        }
    }
    
    protected virtual void LayerPop(int damage = 0)
    {
        PlayPopEffect();

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
        Destroy(gameObject);
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
}
