using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Battery : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        HandleBatteryLife();
    }

    void HandleBatteryLife()
    {
        CheckBatteryLife();
        DepleteBattery();
        RechargeBattery();
    }

    void CheckBatteryLife()
    {
        if (transform.lossyScale.y <= 0)
        {
            ShutdownFlashlight();
        }
    }

    void DepleteBattery()
    {
        if (Flashlight.IsActive() && !(transform.localScale.x <= 0))
        {
            var delay = BuffManager.GetBuff(BuffManager.BuffType.battery).currentValue;
            transform.localScale -= new Vector3(0, 0.01f / delay, 0) * Time.deltaTime;
        }
    }

    void RechargeBattery()
    {
        if (!Flashlight.IsActive() && !(transform.localScale.y >= 0.595f))
        {
            transform.localScale += new Vector3(0, 0.1f, 0) * Time.deltaTime;
            return;
        }
    }

    void ShutdownFlashlight()
    {
        Flashlight.DisableFlashlight();
    }
}
    
