using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBalloon : HpBalloon
{
    [SerializeField] private BalloonScriptable balloonLeak;
    
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        int remainingDamage;
        if (hp >= 0) remainingDamage = 0;
        else remainingDamage = hp * -1;
        EnemiesManager.Instance.EnemieSpawnFromRelease(balloonLeak, this ,0, remainingDamage);
    }
}