using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DamageRegistration : MonoBehaviour
{
    public bool colliding;
    private CountDown reset = new CountDown(0.5f);

    void Update()
    {
        if (colliding) reset.Start(Reset);
    }

    private void Reset()
    {
        colliding = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var layer = other.gameObject.layer;
        colliding = (layer == 8 || layer == 9 || layer == 15 || layer == 16 || layer == 17 || layer == 18);
        reset.Reset();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        var layer = other.gameObject.layer;
        colliding = (layer == 19);
        reset.Reset();
    }

    public void Collide()
    {
        colliding = true;
        reset.Reset();
    }
}
