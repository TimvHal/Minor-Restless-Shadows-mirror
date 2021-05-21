using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOrb : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 13 || other.gameObject.layer == 11)
        {
            
            var script = player.GetComponent<PlayerDamage>();
            var max = BuffManager.GetBuff(BuffManager.BuffType.health).currentValue;
            if (script._health + 1 < max)script._health += 1;
            else if (script._health < max) script._health = max;
            GameObject.FindGameObjectWithTag("HealthRepresentation").GetComponent<Healthrepresentation>()
                .ForceUpdate(script._health);
            Destroy(gameObject);
        }

    }
}
