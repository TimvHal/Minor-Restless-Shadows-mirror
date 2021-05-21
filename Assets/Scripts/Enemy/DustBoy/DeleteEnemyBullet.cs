using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("deleteAfterFewSeconds");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 curr = transform.localScale;
        curr.x = curr.x * 0.999f;
        curr.y = curr.y * 0.999f;
        transform.localScale = curr;
    }

    IEnumerator deleteAfterFewSeconds()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != 9 || other.gameObject.layer == 0) Destroy(gameObject);
    }
}