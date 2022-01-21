using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackPile : BaseProjectile
{
    private int tackQte;
    public AddRemoveSprite renderersScript;

    public void SetTackQte(int qte)
    {
        tackQte = qte;
        renderersScript.AddSprite(tackQte);
    }
    
    public override void BalloonHit(BaseBalloon balloon)
    {
        if (balloon.IsResistant(projectileType))
        {
            EndOfLife();
            return;
        }
        
        balloon.Hit(damage);

        tackQte--;
        renderersScript.RemoveSprite();

        if (tackQte <= 0)
        {
            EndOfLife();
        }
    }
}
