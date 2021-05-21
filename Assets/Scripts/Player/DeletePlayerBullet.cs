using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayerBullet : MonoBehaviour
{
    public Sprite[] Sprites;

    public GameObject damagenum;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(deleteAfterFewSeconds());
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
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 9)
        {
            var dam = Instantiate(damagenum, transform.position, Quaternion.identity);
            float num;
            dam.GetComponent<damagenumber>().damage = num = BuffManager.GetBuff(BuffManager.BuffType.damage).currentValue;
            SoundManager.PlaySound(SoundManager.Sound.GenericHit, 0.8f);
            var renderer = dam.GetComponent<SpriteRenderer>();
            renderer.sprite = Sprites[getsprite(num)];
        }
        Destroy(gameObject);
    }
    private int getsprite(float damage)
    {
        switch (damage)
        {
            case 1:
                return 0;
                break;
            case 1.5f:
                return 1;
                break;
            case 2:
                return 2;
                break;
            case 2.5f:
                return 3;
                break;
            case 3:
                return 4;
                break;
        }

        return 0;
    }
 

}