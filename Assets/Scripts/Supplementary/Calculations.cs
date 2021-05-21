using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Polygons;

public static class Calculations
{
    public static float TranslateTo360(float angle)
    {
        if (angle < 0) angle += 360;
        return angle;
    }
    public static float TranslateTo180(float angle)
    {
        if (angle > 180) angle -= 360;
        return angle;
    }

    public static float ReverseAngle(float angle)
    {
        angle += 180;
        if (angle > 360) angle -= 360;
        return angle;
    }

    public static Vector3 GetCirlePosition(Vector3 input, float radius, float angle)
    {
        float x = Convert.ToSingle(radius * Math.Cos(angle));
        float y = Convert.ToSingle(radius * Math.Sin(angle));

        return input + new Vector3(x, y, 0);
    }
}
