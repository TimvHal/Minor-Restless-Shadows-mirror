using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class BuffManager
{
    public enum BuffType
    {
        magazineSize,
        multishot,
        damage,
        velocity,
        hitbox,
        firerate,
        health,
        movement,
        battery
    }

    public static GameAssets.Buff GetBuff(BuffType T)
    {
        foreach (var buff in GameAssets.i.BuffStats)
        {
            if (buff.type == T) return buff;
        }

        return null;
    }

    public static void IncreaseBuffStrength(BuffType T)
    {
        GameAssets.Buff buff = GetBuff(T);
        if (buff == null) return;
        if (buff.currentValue < buff.maxValue)
        {
            buff.currentValue += buff.increaseBy;
            for (int i = 0; i < GameAssets.i.BuffStats.Length; i++)
            {
                if (GameAssets.i.BuffStats[i].type == T)
                {
                    GameAssets.i.BuffStats[i] = buff;
                }
            }
        }
    }

    public static BuffType GetRandom()
    {
        BuffType[] values = Enum.GetValues(typeof(BuffType)) as BuffType[];
        Random rand = new Random();
        var toReturn = values[rand.Next(values.Length)];
        var returnvalue = GetBuff(toReturn);
        if (returnvalue.currentValue >= returnvalue.maxValue) return GetRandom();
        return toReturn;
    }

    public static Sprite GetSprite(BuffType type)
    {
        foreach (var types in GameAssets.i.BuffSprites)
        {
            if (types.type == type) return types.sprite;
        }

        return null;
    }
}
