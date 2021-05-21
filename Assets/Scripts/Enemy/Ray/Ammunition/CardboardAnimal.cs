using System;
using UnityEngine;
using Utils;

namespace Enemy.Ray.Ammunition
{
    public class CardboardAnimal: MonoBehaviour
    {
        [SerializeField] private Sprite elephant;
        [SerializeField] private Sprite lion;

        private Rigidbody2D _rb;
        private SpriteRenderer _renderer;
        
        private bool _flipped;
        private Vector3 _startPos;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _flipped = false;
            
            PickAnimal();
            PickDirection();
            GiveVelocity();
            if (_flipped) _renderer.flipX = true;
            Destroy(gameObject, 10f);
        }

        void PickAnimal()
        {
            var randInt = Randomizer.RandInt(2);
            switch (randInt)
            {
                case 0:
                    _renderer.sprite = lion;
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    transform.position += new Vector3(0, -0.15f, 0);
                    break;
                case 1:
                    _renderer.sprite = elephant;
                    break;
            }
        }

        void PickDirection()
        {
            var randInt = Randomizer.RandInt(2);
            switch (randInt)
            {
                case 0:
                    _flipped = true;
                    break;
                case 1:
                    transform.position += new Vector3(20, 0, 0);
                    break; 
            }
        }

        void GiveVelocity()
        {
            var speed = Randomizer.RandInt(2, 5);
            var direction = new Vector2(1, 0);
            var velocity =  direction * speed;
            
            if (_flipped)
            {
                _rb.velocity = velocity;
                return;
            }
            _rb.velocity = velocity * -1;
        }
    }
}