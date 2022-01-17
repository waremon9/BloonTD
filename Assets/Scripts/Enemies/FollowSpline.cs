using System;
using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class FollowSpline : MonoBehaviour
{
    [HideInInspector] public Spline spline;
    
    [HideInInspector] public float dist = 0;

    private NewBalloonLogic balloon;

    private void Awake()
    {
        if (!TryGetComponent(out balloon))
        {
            Debug.Log("no balloon with the follow spline : "+name);
        }

        spline = EnemiesManager.Instance.path;
        
        UpdatePosition();
    }

    void Update()
    {
        if (!spline)
        {
            Debug.LogWarning("no spline : " + name);
            return;
        }
        
        if(!balloon) return;
        
        UpdateDistance();
        UpdatePosition();
    }

    public void UpdateDistance()
    {
        //stop when reach the end
        if (dist < spline.Length)
        {

            dist += balloon.balloonBaseData.speed * Time.deltaTime;

            if (dist > spline.Length)
            {
                dist = spline.Length;
            }
        }
        else
        {
            EnemiesManager.Instance.BalloonDead(GetComponent<NewBalloonLogic>());
            
            Destroy(gameObject);//reached the end of the spline
        }
    }

    public void UpdatePosition()
    {
        CurveSample sample;
        if (dist < 0)
        {
            sample = spline.GetSampleAtDistance(0);
        }
        else
        {
            sample = spline.GetSampleAtDistance(dist);
        }
        
        transform.position = sample.location + spline.transform.localPosition;
    }
}
