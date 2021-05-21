using System.Collections;
using UnityEngine;

namespace Enemy.Ray.Ammunition
{
    public class BouncyCluster: MonoBehaviour
    {
        private GameObject _player;

        private float _speed;
        private Rigidbody2D _rb;
        private Vector2 _target;
        
        public GameObject _bouncyShrapnel;
        private int _shrapnelWave;

        private Vector3 _explosionPos;

        private void Start()
        {
            _player = GameObject.Find("Player");
            _speed = 20f;
            _rb = GetComponent<Rigidbody2D>();
            _shrapnelWave = 1;

            StartCoroutine(LobCluster());
        }

        private void Update()
        {
            if (_rb.position.y < _target.y && _shrapnelWave == 1)
            {
                _explosionPos = transform.position;
                StartCoroutine(Explode());
                var color = GetComponent<SpriteRenderer>().color;
                color.a = 0;
                GetComponent<SpriteRenderer>().color = color;
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

        IEnumerator Explode()
        {
            SpawnShrapnel();
            yield return new WaitForSeconds(0.3f);
            SpawnShrapnel();
            yield return new WaitForSeconds(0.3f);
            SpawnShrapnel();
            yield return new WaitForSeconds(0.3f);
            Destroy(gameObject);
        }

        void SpawnShrapnel()
        {
            for (var index = 0; index < 3; index++)
            {
                var clone = Instantiate(_bouncyShrapnel, _explosionPos, Quaternion.identity);
                clone.GetComponent<Rigidbody2D>().rotation = index * 360 / 3 + (_shrapnelWave * 10);
            }

            _shrapnelWave++;
        }
    }
}