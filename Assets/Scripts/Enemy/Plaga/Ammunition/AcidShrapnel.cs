using System;
using UnityEngine;

namespace Enemy.Plaga.Ammunition
{
    public class AcidShrapnel: MonoBehaviour
    {
        private float _speed;
        private Rigidbody2D _rb;
        private Vector2 _direction;

        private void Start()
        {
            _speed = 10f;
            _rb = GetComponent<Rigidbody2D>();

            _direction = Vector2.up * _speed;
            _direction = Quaternion.Euler(0, 0, _rb.rotation) * _direction;
            _rb.velocity += _direction;
            
            //Destroy gameobject after three seconds.
            Destroy(gameObject, 5f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Destroy(gameObject);
        }
    }
}