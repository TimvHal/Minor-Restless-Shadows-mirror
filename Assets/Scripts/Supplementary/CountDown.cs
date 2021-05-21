using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown
{
    public float max;
    private float targetTime;
    
    public CountDown(float endTime)
    {
        max = endTime;
    }

    public void Start(Action resultingAction)
    {
        targetTime -= Time.deltaTime;
 
        if (targetTime <= 0.0f)
        {
            targetTime = max;
            resultingAction();
        }
    }
    public void Start(Action<SoundManager.Sound> resultingAction, SoundManager.Sound parameter)
    {
        targetTime -= Time.deltaTime;
 
        if (targetTime <= 0.0f)
        {
            targetTime = max;
            resultingAction(parameter);
        }
    }

    public void Reset()
    {
        targetTime = max;
    }
}
