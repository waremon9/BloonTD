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
        //CheckCollisionProjectileBalloon();
    }

    private int hit = 0;
    private void FixedUpdate()
    {
        hit = 0;
        CheckCollisionProjectileBalloon();
        Debug.Log(hit);
    }

    public void AddProjectile(BaseProjectile proj)
    {
        AllProjectile.Add(proj);
    }

    public void RemoveProjectile(BaseProjectile proj)
    {
        AllProjectile.Remove(proj);
    }

    public void DestroyAllProjectiles()
    {
        foreach (BaseProjectile projectile in AllProjectile)
        {
            Destroy(projectile.gameObject);
        }
        AllProjectile.Clear();
    }

    private void CheckCollisionProjectileBalloon()
    {
        List<BaseBalloon> listB = EnemiesManager.Instance.GetAllBalloon();

        for (int i = AllProjectile.Count-1; i >= 0; i--)
        {
            BaseProjectile projectile = AllProjectile[i];

            for (int j = listB.Count - 1; j >= 0; j--)
            {
                BaseBalloon balloon = listB[j];
                    
                if (Vector2.Distance(balloon.transform.position, projectile.transform.position) <=
                    balloon.hitBoxRadius + projectile.hitBoxRadius)
                {
                    hit++;
                    if (projectile.GetType() == typeof(TackPile))
                    {
                        projectile.BalloonHit(balloon);
                        if(!projectile) break;
                    }
                    else
                    {
                        if(!balloon.NewProjectileHit(projectile)) continue;
                        
                        projectile.BalloonHit(balloon);
                        break;
                    }
                }
            }
        }
    }
}
