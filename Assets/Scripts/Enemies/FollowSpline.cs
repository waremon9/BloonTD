using System;
using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class FollowSpline : MonoBehaviour
{
    [HideInInspector] public Spline spline;
    
    [HideInInspector] public float rate = 0;
    private float durationInSecond;

    public void CalculateDurationInSecond(float ballonSpeed)
    {
        durationInSecond = spline.Length / ballonSpeed;
    }

    void Update()
    {
        if (!spline)
        {
            Debug.LogWarning("no spline : " + name);
            return;
        }
        
        UpdateRate();
        UpdatePosition();
    }

    public void UpdateRate()
    {
        //stop when reach the end
        if (rate < spline.nodes.Count - 1) {
            
            //some math so the speed is the same along the entire spline
            float totalPathLength = spline.Length;
            float actualCurveLenght;
            if (rate < 0)
            {
                actualCurveLenght = spline.GetCurves()[0].Length;
            }
            else
            {
                actualCurveLenght = spline.GetCurves()[Mathf.FloorToInt(rate)].Length;
            }
            
            float curvePercentOfTotalPath = totalPathLength / actualCurveLenght;
            float durationForThisCurve = durationInSecond / curvePercentOfTotalPath;
            
            rate += Time.deltaTime / durationForThisCurve;

            if (rate > spline.nodes.Count - 1)
            {
                rate = spline.nodes.Count - 1;
            }
        }
        else
        {
            EnemiesManager.Instance.BalloonDead(GetComponent<BaseBalloon>());
            
            Destroy(gameObject);//reached the end of the spline
        }
    }

    public void UpdatePosition()
    {
        CurveSample sample;
        if (rate < 0)
        {
            sample = spline.GetSample(0);
        }
        else
        {
            sample = spline.GetSample(rate);
        }
        
        transform.position = sample.location + spline.transform.localPosition;
    }
}
