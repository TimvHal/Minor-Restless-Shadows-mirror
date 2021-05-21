using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Calculations;
using Random = System.Random;

public class ShootingDustBoy : DustBoy
{
    public double speed = 1;
    public float angle = 0;
    private float RotateSpeed = 1;

    public MovementStage stage = MovementStage.Start;
    public override Vector3 Movement(Vector3 _player, Vector3 _self)
    {
        Vector2 rotation = GetCirlePosition(_player, 6, angle);
        angle += RotateSpeed *(new Random().Next(10) / 5) * Time.deltaTime;
        if (angle > 360) angle -= 360;
        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * 5;
        return _player + (Vector3)offset;
    }

    public override Vector3 LightMovement(Vector3 _player, Vector3 _self)
    {
        angle += RotateSpeed *(new Random().Next(10) / 5) * Time.deltaTime;
        if (angle > 360) angle -= 360;
        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * 10;
        return _player + (Vector3)offset;
    }

    public override bool Shoot(Vector3 _player, Vector3 _self)
    {
        return true;
    }
}