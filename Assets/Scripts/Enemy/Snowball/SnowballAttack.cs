using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnowballAttack : MonoBehaviour
{
    public AttackTypes current;
    public GameObject player;
    
    public GameObject fireBall;
    public GameObject explodingFireBall;

    public Vector2 spawnPos;
    public Transform worldPosition;
    public enum AttackTypes
    {
        Cooldown,
        FireBall,
        Exploding,
        AOE
    }

    public Action GetAttack()
    {
        switch (current)
        {
            case AttackTypes.Exploding:
                return Exploding;
            case AttackTypes.FireBall:
                return ShootFireBall;
        }

        return Cooldown;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetAttack()
    {
        current = randomAttack();
    }

    public AttackTypes randomAttack()
    {
        var oneOrtwo = Random.Range(0f, 100f);
        if (oneOrtwo > 50) 
            return AttackTypes.Exploding;
        else if (oneOrtwo < 100)
            return AttackTypes.FireBall;

        return randomAttack();
    }
    
    void Cooldown()
    {
        SetAttack();
    }

    void ShootFireBall()
    {
        for (int i = -1; i < 2; i++)
        {
            Vector3 position = transform.position;
            position.x += i;
            var clone = Instantiate(fireBall, position, Quaternion.identity);
            Physics2D.IgnoreCollision(clone.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
            float angle = (float) ((float) Math.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * 180 / Math.PI);
            angle += (i * 15);
            Rigidbody2D r = clone.GetComponent<Rigidbody2D>();
            clone.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            r.AddForce(Quaternion.Euler(0,0,angle) * Vector2.right * 150);
        }

        current = AttackTypes.Cooldown;
    }

    void Exploding()
    {
        Instantiate(explodingFireBall, worldPosition.position + transform.localPosition, Quaternion.identity);
        current = AttackTypes.Cooldown;
    }

    private void OnDestroy()
    {
        SoundManager.PlaySound(SoundManager.Sound.GenericDeath);
    }
}
