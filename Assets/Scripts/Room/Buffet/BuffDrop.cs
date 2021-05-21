using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDrop : MonoBehaviour
{
    public BuffManager.BuffType type;
    public Rigidbody2D rb;
    private GameAssets.Buff value;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = BuffManager.GetSprite(type);
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * 100);
        StartCoroutine(droptime());
    }

    IEnumerator droptime()
    {
        yield return new WaitForSeconds(0.6f);
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 13)
        {
            BuffManager.IncreaseBuffStrength(type);
            SoundManager.PlaySound(SoundManager.Sound.BuffPickUp);
            BannerSpawning.instance.Spawn(type);
            if (type == BuffManager.BuffType.health) OnHealthUp();
            Destroy(gameObject);
        }
    }

    private void OnHealthUp()
    {
        var playerHealth = FindObjectOfType<PlayerDamage>();
        var HealthRep = FindObjectOfType<Healthrepresentation>();
        playerHealth._health = BuffManager.GetBuff(BuffManager.BuffType.health).currentValue;
        HealthRep.resetAll();
    }
}
