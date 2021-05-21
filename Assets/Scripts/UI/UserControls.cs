using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using static Calculations;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class UserControls : MonoBehaviour
{
    public Stick movementStick;
    public Stick aimingStick;

    public Joystick aiming;
    public static UserControls Instance;

    void Start()
    {
        Instance = this;
        movementStick = new Stick();
        aimingStick = aiming.stick;
    }
}
