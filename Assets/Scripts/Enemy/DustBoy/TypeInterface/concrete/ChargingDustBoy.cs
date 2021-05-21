using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Calculations;

public class ChargingDustBoy : DustBoy
{
    public MovementStage Stage = MovementStage.Start;
    public double speed = 0;
    private CountDown _chargingCountdown = new CountDown(3f);

    public override Vector3 Movement(Vector3 _player, Vector3 _self)
    {
        Vector3 result = Vector3.zero;
        switch (Stage)
        {
            case (MovementStage.Start):
                speed += 0.1;
                if (speed >= 0.5)
                {
                    speed = 1;
                    Stage = MovementStage.Charging;
                }
                break;
            case (MovementStage.Charging):
                _chargingCountdown.Start(Charge);
                break;
            case (MovementStage.Stopping):
                if (speed <= 0) Stage = MovementStage.Start;
                speed -= 0.005;
                break;
        }
        result =new Vector2((float) ((_self.x - _player.x) * speed), (float) ((_self.y - _player.y) * speed));

        return -result;
    }

    public override Vector3 LightMovement(Vector3 _player, Vector3 _self)
    {
        return Movement(_player, _self) * 0.25f;
    }

    void Charge()
    {
        Stage = MovementStage.Stopping;
        speed = 1;
    }
    public override bool Shoot(Vector3 _player, Vector3 _self)
    {
        return false;
    }
}
