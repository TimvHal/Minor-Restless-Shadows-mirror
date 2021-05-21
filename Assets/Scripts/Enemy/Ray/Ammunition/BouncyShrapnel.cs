using UnityEngine;

namespace Enemy.Ray.Ammunition
{
    public class BouncyShrapnel : MonoBehaviour
    {
        private float _speed;
        private Rigidbody2D _rb;
        private Vector2 _direction;

        private int _collisionCounter;

        private void Start()
        {
            _speed = 10f;
            _rb = GetComponent<Rigidbody2D>();

            _direction = Vector2.up * _speed;
            _direction = Quaternion.Euler(0, 0, _rb.rotation) * _direction;
            _rb.velocity += _direction;

            _collisionCounter = 0;

            //Start time for 10 seconds before deletion.
            Destroy(gameObject, 10f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _collisionCounter++;
            if (_collisionCounter > 3) Destroy(gameObject);
            if (other.gameObject.layer == 13 || other.gameObject.layer == 11) Destroy(gameObject);
        }
    }
}