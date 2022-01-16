using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceProjectile : BaseProjectile
{
    private int piercePower;

    public void SetPierce(int p)
    {
        piercePower = p;
    }
    
    public override void BalloonHit(BaseBalloon balloon)
    {
        base.BalloonHit(balloon);
        
        if (balloon.IsResistant(projectileType))
        {
            EndOfLife();
            return;
        }
        
        balloon.Hit(damage);
        
        piercePower--;
        if (piercePower < 0)
        {
            EndOfLife();
        }
    }
}
