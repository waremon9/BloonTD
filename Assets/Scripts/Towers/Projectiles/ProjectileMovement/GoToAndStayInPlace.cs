using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SplineMesh;
using UnityEngine;

public class GoToAndStayInPlace : BaseProjectileMovement
{
    private bool DoOnce = true;

    private Vector3 destination;
    
    protected override void Movement()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            projectile.EndOfLife();
        }
        
        if(!DoOnce) return;
        DoOnce = false;

        StartCoroutine(GoToDestinationCoroutine());
    }

    public void SetDestination(Vector3 dest)
    {
        destination = dest;
    }

    private IEnumerator GoToDestinationCoroutine()
    {
        Vector3 start = transform.position;
        float alpha = 0;

        while (alpha<1)
        {
            alpha += Time.deltaTime * (1/speed);
            transform.position = Vector3.Lerp(start, destination, alpha);
            yield return null;
        }
    }
}
