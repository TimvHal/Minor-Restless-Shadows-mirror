using System;
using UnityEngine;

namespace Enemy.Plaga.Ammunition
{
    public class AcidBullet : MonoBehaviour
    {
        private GameObject _player;

        private float _speed;
        private Rigidbody2D _rb;
        private Vector3 _playerPos;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _speed = 10f;
            _rb = GetComponent<Rigidbody2D>();

            //Set bullet trajectory.
            _playerPos = _player.transform.position;

            var offset = _playerPos - transform.position;
            var direction = Vector2.ClampMagnitude(offset, 1.0f);

            _rb.velocity += direction * _speed;

            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
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