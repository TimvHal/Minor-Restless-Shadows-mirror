using System;
using UnityEngine;

namespace Enemy.Plaga.Ammunition
{
    public class AcidPuddle: MonoBehaviour
    {
        public GameObject _collider;
        private void Start()
        {
            _collider = GameObject.Find("DamageCollider");
            Destroy(gameObject, 10f);
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.layer == 13)
            {
                _collider.GetComponent<DamageRegistration>().Collide();
            }
        }
    }
}