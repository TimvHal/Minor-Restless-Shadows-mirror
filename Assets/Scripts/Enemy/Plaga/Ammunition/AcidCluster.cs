using System;
using System.Collections;
using UnityEngine;

namespace Enemy.Plaga.Ammunition
{
    public class AcidCluster : MonoBehaviour
    {
        private GameObject _player;

        private float _speed;
        private Rigidbody2D _rb;
        private Vector2 _target;

        public GameObject _acidShrapnel;

        private void Start()
        {
            _player = GameObject.Find("Player");
            _speed = 20f;
            _rb = GetComponent<Rigidbody2D>();

            StartCoroutine(LobCluster());
        }

        private void Update()
        {
            if (_rb.position.y < _target.y)
            {
                Destroy(gameObject);
                SpawnShrapnel();
            }
        }

        IEnumerator LobCluster()
        {
            _rb.velocity = new Vector2(0, 1) * _speed;
            
            yield return new WaitForSeconds(2f);

            _target = _player.transform.position;
            _rb.position = new Vector2(_target.x, _rb.position.y);
            _rb.velocity = new Vector2(0, -1) * _speed;
        }

        private void SpawnShrapnel()
        {
            for (int i = 0; i < 8; i++)
            {
                var clone = Instantiate(_acidShrapnel, transform.position, Quaternion.identity);
                clone.GetComponent<Rigidbody2D>().rotation = i * (360 / 8);
            }
        }
    }
}