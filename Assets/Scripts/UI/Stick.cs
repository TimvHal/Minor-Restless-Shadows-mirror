using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Calculations;
[Serializable]
public class Stick
{
    public string type;
    public bool touchStart = false;
    public Vector2 pointA;
    public Vector2 pointB;
    public Transform innerCircle;
    public Transform outerCircle;
    public float angle = 180;
    public Vector2 vector;
    public Vector2 touchPos;

    public bool first = false;

    public Stick(string _type, Vector2 _pointA, Vector2 _pointB, Transform _innerCircle, Transform _outerCircle)
    {
        type = _type;
        pointA = _pointA;
        pointB = _pointB;
        innerCircle = _innerCircle;
        outerCircle = _outerCircle;
    }

    public Stick()
    {
        vector = Vector2.zero;
    }

    public void ResetSelf()
    {
        touchStart = false;
        first = false;
        innerCircle.transform.position = outerCircle.position;
    }

    public void SaveDirectionInformation()
    {
        var offset = pointB - pointA;
        var direction = Vector2.ClampMagnitude(offset, 1.0f);
        vector = direction;
        angle = TranslateTo360(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }
    
}