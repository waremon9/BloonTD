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
    
    public override bool BalloonHit(NewBalloonLogic balloon)
    {
        if (base.BalloonHit(balloon)) return true;
        
        if (balloon.IsResistant(projectileType))
        {
            EndOfLife();
            return true;
        }
        
        balloon.Hit(damage);
        
        piercePower--;
        if (piercePower < 0)
        {
            EndOfLife();
        }

        return false;
    }
}
