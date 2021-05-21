using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBox : MonoBehaviour
{
    public ChestSelection eventHandler;
    public GameObject Buff;
    public Sprite open;
    public bool inactive = false;
    private SpriteRenderer r;
    private void Start()
    {
        r = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 13)
        {
            if (r == null || open == null) return;
            r.sprite = open;
            StartCoroutine(spawnBuff());
        }
    }

    IEnumerator spawnBuff()
    {
        yield return new WaitForSeconds(0.5f);
        if (!inactive)
        {

            var spawnLocation = transform.position;
            var clone = Instantiate(Buff, spawnLocation, Quaternion.identity);
            clone.GetComponent<BuffDrop>().type = BuffManager.GetRandom();
            Physics2D.IgnoreCollision(clone.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            inactive = true;
            if (eventHandler != null) eventHandler._unityEvent.Invoke();
        }
    }

}
