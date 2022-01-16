using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MySingleton<ProjectileManager>
{
    public override bool DoDestroyOnLoad { get; }

    public Transform projectileParent;

    private List<BaseProjectile> AllProjectile = new List<BaseProjectile>();

    private void Update()
    {
        CheckCollisionProjectileBalloon();
    }

    public void AddProjectile(BaseProjectile proj)
    {
        AllProjectile.Add(proj);
    }

    public void RemoveProjectile(BaseProjectile proj)
    {
        AllProjectile.Remove(proj);
    }

    private void CheckCollisionProjectileBalloon()
    {
        foreach (BaseProjectile projectile in AllProjectile)
        {
            foreach (BaseBalloon balloon in EnemiesManager.Instance.GetAllBalloon())
            {
                if (Vector2.Distance(balloon.transform.position, projectile.transform.position) <=
                    balloon.hitBoxRadius + projectile.hitBoxRadius)
                {
                    projectile.BalloonHit(balloon);
                    return;
                }
            }
        }
    }
}
