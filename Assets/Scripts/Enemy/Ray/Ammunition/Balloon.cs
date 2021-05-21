using System;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace Enemy.Ray.Ammunition
{
    public class Balloon: MonoBehaviour
    {       
        private GameObject _player;

        private float _speed;
        private Rigidbody2D _rb;
        private Vector3 _playerPos;
        private Vector3 _target;
        private SpriteRenderer _renderer;

        [SerializeField] private GameObject reticle;
        [SerializeField] private Sprite _blueBalloon;        
        [SerializeField] private Sprite _greenBalloon;
        [SerializeField] private Sprite _yellowBalloon;

        private GameObject _reticle;
        
        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _speed = Randomizer.RandInt(5, 11);
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            
            _rb.velocity += new Vector2(0, -1) * _speed;

            PickColor();
            SpawnReticle();
        }

        private void FixedUpdate()
        {
            if (transform.position.y < _target.y)
            {
                Explode();
            }
        }

        void PickColor()
        {
            var index = Randomizer.RandInt(4);
            switch (index)
            {
                case 0:
                    _renderer.sprite = _blueBalloon;
                    break;
                case 1:
                    _renderer.sprite = _greenBalloon;
                    break;
                case 2:
                    _renderer.sprite = _yellowBalloon;
                    break;
                case 3:
                    break;
            }
        }
        void SpawnReticle()
        {
            var randY = Randomizer.RandInt(15, 26);
            _target = transform.position - new Vector3(0, randY, 0);
            _reticle = Instantiate(reticle, _target, Quaternion.identity);
        }

        void Explode()
        {
            //TODO Spawn explosion.
            Destroy(_reticle);
            Destroy(gameObject);
        }
    }
}