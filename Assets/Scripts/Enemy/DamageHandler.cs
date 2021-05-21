using System;
using UnityEngine;

namespace Enemy
{
    public class DamageHandler: MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == 10)
            {
                var playerDamage = BuffManager.GetBuff(BuffManager.BuffType.damage);
                var damageDealt = playerDamage.currentValue;
                GetComponent<PlagaAI>().TakeDamage(damageDealt);
            }
        }
    }
}