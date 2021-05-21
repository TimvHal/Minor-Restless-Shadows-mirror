using System;
using Unity.Mathematics;
using UnityEngine;

namespace Enemy.Plaga.Ammunition
{
    public class AcidStream : MonoBehaviour
    {
        private GameObject _player;
        private GameObject _collider;

        private float _speed;

        private void Start()
        {
            _player = GameObject.Find("Player");
            _collider = GameObject.Find("DamageCollider");

            var offset = _player.transform.position - transform.position;
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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