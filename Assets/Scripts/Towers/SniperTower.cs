using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : NoProjectileTower
{
    public override void Shoot()
    {
        target.Hit(damage);
    }

    protected override void Update()
    {
        if(!enabledTower) return;
        
        if (CanShoot() && EnemiesManager.Instance.AtLeastOneBalloonAlive())
        {
            UpdateTarget();
            RotationLookAtTarget();
            Shoot();
            LastShoot = Time.time;
        }
    }

    protected override void UpdateTarget()
    {
        target = EnemiesManager.Instance.GetFirstBalloonInRange(transform.position, Mathf.Infinity);
    }

    protected override void UpdateRangeIndicator()
    {
        rangeIndicator.transform.localScale = Vector2.one * range * 2;
    }
}
