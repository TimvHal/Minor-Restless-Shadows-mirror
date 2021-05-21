using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheatManager 
{
    public enum Cheat
    {
        God
    }

    public static void ToggleCheat(Cheat C)
    {
        GameAssets.Cheats buff = GetCheat(C);
        if (buff == null) return;
        for (int i = 0; i < GameAssets.i.Cheatses.Length; i++)
        {
            if (GameAssets.i.Cheatses[i].cheat == C)
            {
                GameAssets.i.Cheatses[i].active = !GameAssets.i.Cheatses[i].active;
            }
        }
    }

    public static GameAssets.Cheats GetCheat(Cheat cheat)
    {
        foreach (var che in GameAssets.i.Cheatses)
        {
            if (cheat == che.cheat) return che;
        }

        return null;
    }
    
}
