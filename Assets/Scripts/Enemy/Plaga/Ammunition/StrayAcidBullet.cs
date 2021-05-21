using System;
using Unity.Mathematics;
using UnityEngine;

namespace Enemy.Plaga.Ammunition
{
    public class StrayAcidBullet: MonoBehaviour
    {
        private GameObject _player;

        private float _speed;
        private Rigidbody2D _rb;
        private Vector3 _playerPos;
        private Vector3 _direction;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _speed = 8f;
            _rb = GetComponent<Rigidbody2D>();

            //Set bullet trajectory.
            _playerPos = _player.transform.position;

            var offset = _playerPos - transform.position;
            var direction = Vector2.ClampMagnitude(offset, 1.0f);
            direction = Quaternion.Euler(0, 0, _rb.rotation) * direction;
            
            _rb.velocity += direction.normalized * _speed;
            
            _rb.rotation = 0;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            
            //Destroy gameobject after three seconds.
            Destroy(gameObject, 5f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Destroy(gameObject);
        }
    }
}