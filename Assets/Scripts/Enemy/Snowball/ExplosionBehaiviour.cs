using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Calculations;

public class ExplosionBehaiviour : MonoBehaviour
{
    CountDown cd = new CountDown(2f);
    private Rigidbody2D _rigidbody;
    public GameObject fireBall;
    private float increment = 0.2f;
    public int quantity = 10;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.rotation = -90f;
        _rigidbody.velocity = Vector2.down * 2;
        StartCoroutine(Explode());
        SoundManager.PlaySound(SoundManager.Sound.SnowballFireball, 0.8f);
    }

    private void Update()
    {
        Vector2 curr = transform.localScale;
        curr.x = curr.x * 0.999f;
        curr.y = curr.y * 0.999f;
        transform.localScale = curr;
    }

    // Update is called once per frame

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        
    }

    private void OnDestroy()
    {
        for (int i = 0; i < quantity; i++)
        {
            Vector3 position = GetCirlePosition(transform.position, 0.1f, i * (360 / quantity));
            var clone = Instantiate(fireBall, transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(clone.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            Rigidbody2D r = clone.GetComponent<Rigidbody2D>();
            clone.transform.rotation =Quaternion.Euler(0, 0, i* (360 / quantity));
            var offset = position - transform.position;
            var direction = Vector2.ClampMagnitude(offset, 1.0f);
            r.velocity = direction * 15;
        }
    }
}
