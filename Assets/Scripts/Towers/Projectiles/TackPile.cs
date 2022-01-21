using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackPile : BaseProjectile
{
    public int tackQte;
    public AddRemoveSprite renderersScript;

    public void SetTackQte(int qte)
    {
        tackQte = qte;
        renderersScript.AddSprite(tackQte);
    }
    
    public override void BalloonHit(BaseBalloon balloon)
    {
        if(tackQte<=0) return;
        
        if (balloon.IsResistant(projectileType))
        {
            EndOfLife();
            return;
        }
        
        balloon.Hit(damage);

        tackQte--;
        if (!renderersScript) TryGetComponent(out renderersScript);
        renderersScript.RemoveSprite();

        if (tackQte <= 0)
        {
            EndOfLife();
        }
    }
}
