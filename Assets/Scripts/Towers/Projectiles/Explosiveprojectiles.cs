using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Explosiveprojectiles : BaseProjectile
{
    private float explosionRange;

    [SerializeField] private ParticleSystem explosionEffect;

    public void SetExplosionRange(float range)
    {
        explosionRange = range;
    }
    
    public override void BalloonHit(BaseBalloon balloon)
    {
        EndOfLife();
    }

    public override void EndOfLife()
    {
        ExplosionDamageBalloon();
        base.EndOfLife();
    }

    protected void ExplosionDamageBalloon()
    {
        foreach (BaseBalloon b in EnemiesManager.Instance.GetAllBalloon())
        {
            if (Vector3.Distance(b.transform.position, transform.position) <= explosionRange + b.hitBoxRadius)
            {
                if(!b.IsResistant(projectileType)) b.Hit(damage); 
            }
        }

        if (explosionEffect && GameManager.Instance.particleParent)
        {
            ParticleSystem temp = Instantiate(explosionEffect, transform.position, Quaternion.identity, GameManager.Instance.particleParent);
            temp.transform.localScale = Vector3.one * explosionRange;
        }
        if (!explosionEffect)
        {
            Debug.LogError("Missing particle system prefab : " + name);
        }
        if (!GameManager.Instance.particleParent)
        {
            Debug.LogError("No particle parent reference in gameManager");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
