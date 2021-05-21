using UnityEngine;

namespace Enemy.Ray.Ammunition
{
    public class BouncyBall: MonoBehaviour
    {
        private GameObject _player;

        private float _speed;
        private Rigidbody2D _rb;
        private Vector3 _playerPos;

        private int _collisionCounter;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _speed = 10f;
            _rb = GetComponent<Rigidbody2D>();
            _collisionCounter = 0;

            //Set bullet trajectory.
            _playerPos = _player.transform.position;

            var offset = _playerPos - transform.position;
            var direction = Vector2.ClampMagnitude(offset, 1.0f);

            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            
            _rb.velocity += direction * _speed;
            
            //Destroy gameobject after three seconds.
            Destroy(gameObject, 5f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _collisionCounter++;
            if (_collisionCounter > 4) Destroy(gameObject);  
            if(other.gameObject.layer == 11 || other.gameObject.layer == 13) Destroy(gameObject);
        }
    }
}