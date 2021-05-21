using System;
using System.Collections;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Enemy.Plaga.Ammunition
{
    public class AcidNeedle : MonoBehaviour
    {
        private GameObject _player;

        private float _speed;
        private Rigidbody2D _rb;
        private Vector3 _target;

        public GameObject _acidPuddle;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _speed = 10f;
            _rb = GetComponent<Rigidbody2D>();

            //Set bullet trajectory.
            _target = _player.transform.position;
            var offset = _target - transform.position;
            var direction = Vector2.ClampMagnitude(offset, 1.0f);

            _rb.velocity += direction * _speed;

            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            StartCoroutine(Explode(3));
        }

        private void Update()
        {
            if (Vector2.SqrMagnitude(_rb.position - new Vector2(_target.x, _target.y)) <= 0.1)
            {
                StartCoroutine(Explode(0));
            }
        }

        private IEnumerator Explode(int timeInSeconds)
        {
            yield return new WaitForSeconds(timeInSeconds);
            Destroy(gameObject);
            Instantiate(_acidPuddle, transform.position, Quaternion.identity);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            StartCoroutine(Explode(0));
        }
    }
}