using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffStats : MonoBehaviour
{
    public Text Hitbox;
    public Text Damage;
    public Text Multishot;
    public Text Firerate;
    public Text Magazine;
    public Text Velocity;

    private void FixedUpdate()
    {
        Hitbox.text = "Hitbox: " + BuffManager.GetBuff(BuffManager.BuffType.hitbox).currentValue.ToString();
        Damage.text = "Damage: " + BuffManager.GetBuff(BuffManager.BuffType.damage).currentValue.ToString();
        Multishot.text = "Multishot: " +  BuffManager.GetBuff(BuffManager.BuffType.multishot).currentValue.ToString();
        Velocity.text = "Velocity: " +  BuffManager.GetBuff(BuffManager.BuffType.velocity).currentValue.ToString();
        Firerate.text = "Fire Rate: " +  BuffManager.GetBuff(BuffManager.BuffType.firerate).currentValue.ToString();
        Magazine.text = "Magazine: " +  BuffManager.GetBuff(BuffManager.BuffType.magazineSize).currentValue.ToString();
    }
}
