using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : BaseTower
{
    public override void Shoot()
    {
        target.Hit(damage);
    }

    protected override void Update()
    {
        if (CanShoot() && TargetInRange())
        {
            UpdateTarget();
            RotationLookAtTarget();
            Shoot();
            LastShoot = Time.time;
        }
    }

    protected override bool TargetInRange()
    {
        return EnemiesManager.Instance.GetAllBalloon().Count != 0;
    }

    protected override void UpdateTarget()
    {
        List<BaseBalloon> allBalloon = EnemiesManager.Instance.GetAllBalloon();

        BaseBalloon t = allBalloon[0];

        foreach (BaseBalloon balloon in allBalloon)
        {
            if (balloon.FollowSpline.dist > t.FollowSpline.dist)
            {
                t = balloon;
            }
        }

        target = t;
    }

    protected override void UpdateRangeIndicator()
    {
        rangeIndicator.transform.localScale = Vector2.one * range * 2;
    }
}
