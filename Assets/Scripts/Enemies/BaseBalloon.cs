using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SplineMesh;
using UnityEngine;

public class BaseBalloon : MonoBehaviour
{
    public BalloonScriptable balloonBaseData;
    public FollowSpline followSpline;
    public float hitBoxRadius;
    public int hpActual;

    public List<BaseProjectile> ProjectilesHit;

    public void UpdateStats(BalloonScriptable balloonStats)
    {
        balloonBaseData = balloonStats;
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = balloonBaseData.sprite;
        }

        hpActual = balloonBaseData.hp;
        transform.localScale = Vector2.one * balloonBaseData.size;
    }
    
    private void Start()
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
        return balloonBaseData.resistance.Contains(type);
    }

    private void PlayPopEffect()
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
    
    protected void LayerPop(int damage = 0)
    {
        PlayPopEffect();

        if (balloonBaseData.IsLastLayer())
        {
            Death();
            return;
        }
        
        
        if (balloonBaseData.MoreThanOneOnRelease())
        {
            bool firstSkipped = false;
            int nb = 1;
            foreach (ReleaseOnDeath data in balloonBaseData.releaseOnDeath)
            {
                for (int i = 0; i < data.qte; i++)
                {
                    if (!firstSkipped)
                    {
                        firstSkipped = true;
                        continue;
                    }
                    
                    EnemiesManager.Instance.EnemieSpawnFromRelease(data.Balloon,this, nb * 0.3f, damage);
                    nb++;
                }
            }
        }
        
        UpdateStats(balloonBaseData.releaseOnDeath[0].Balloon);
    }

    private void Death()
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
