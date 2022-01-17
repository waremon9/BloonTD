using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBalloon : BaseBalloon
{
    public BalloonScriptable balloonLeak;
    
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        int remainingDamage = hpActual > 0 ? 0 : hpActual * -1;
        int qteToLeak = hpActual > 0 ? damage : damage + hpActual;

        for (int i = 0; i < qteToLeak; i++)
        {
            EnemiesManager.Instance.EnemieSpawnFromRelease(balloonLeak, this ,0, remainingDamage);
        }
    }
}
