using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DustBoy
{
    public enum MovementStage
    {
        Start,
        Charging,
        Stopping
    }

    public MovementStage Stage;
    public double speed;
    public abstract Vector3 Movement(Vector3 _player, Vector3 _self);

    public abstract Vector3 LightMovement(Vector3 _player, Vector3 _self);

    public abstract bool Shoot(Vector3 _player, Vector3 _self);

}