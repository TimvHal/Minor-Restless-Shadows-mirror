using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Nurse : MonoBehaviour
{
    public GameObject player;
    public float health = 15;
    public EnemySpawnController dc;
    public GameObject orb;
    public GameObject coin;
    private Animator anim;
    private bool died;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (died)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (health <= 0)
        {
            StartPerish();
        }

        if (Vector2.Distance(player.transform.position, transform.position) < 10)
        {
            var velocity = new Vector2((float) ((transform.position.x - player.transform.position.x) * 0.4f), 
                (float) ((transform.position.y - player.transform.position.y) * 0.2f));
            rb.velocity = -velocity;
        
            anim.SetBool("isWalking", true);
            anim.SetFloat("x", -velocity.x);
            anim.SetFloat("y", -velocity.y);
        }
        else
        {
            anim.SetBool("isWalking", false);
            rb.velocity = Vector2.zero;
        }
    }
    private bool damaged;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        {
            damaged = true;
            health -= BuffManager.GetBuff(BuffManager.BuffType.damage).currentValue;
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(resetDamageIndication());
        }
        if (other.gameObject.layer == 11)
            SoundManager.PlaySound(SoundManager.Sound.NurseAttack);
    }

    IEnumerator resetDamageIndication()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        damaged = false;
    }


    IEnumerator walksound()
    {
       yield return new WaitForSeconds(new Random().Next(10));
       SoundManager.PlaySound(SoundManager.Sound.NurseWalk);
       StartCoroutine(walksound());
    }

    void StartPerish()
    {
        died = true;
        anim.SetBool("died", true);
    }

    void Perish()
    {
        if (new Random().Next(100) < 5)Instantiate(orb, transform.position, Quaternion.identity);
        ScoreUI.instance.UpdateValue(1);
        dc._currentEnemies -= 1;
        if (dc._currentEnemies <= 0)
        {
            dc.ClearEvent.Invoke();
        }
        SoundManager.PlaySound(SoundManager.Sound.NurseDeath);
        Destroy(gameObject);
    }

    public void ResetAttack()
    {
        anim.SetBool("attack", false);
    }

    public void AttackIfInRange()
    {
        anim.SetBool("attack", Vector2.Distance(player.transform.position, transform.position) < 1.5f);
    }
}
