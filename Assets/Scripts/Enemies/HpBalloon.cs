using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBalloon : BaseBalloon
{
    [SerializeField] protected int hp;

    public override void Hit(int damage = 1)
    {
        TakeDamage(damage);
        if (hp <= 0)
        {
            LayerPop(hp * -1);
        }
    }

    protected virtual void TakeDamage(int damage)
    {
        hp-= damage;
    }
}
