using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Room;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using static Calculations;
using Random = System.Random;

public class DustBoyController : MonoBehaviour, LightRegistration
{
    public enum EnemyType
    {
        Shooting,
        Charging
    }
    public int maxHealth = 3;
    private float _health;
    private bool _lock;
    public Animator _Animator;
    private DustBoy _concreteDustBoy;
    public GameObject _player;
    private PlayerMovement script;
    private Rigidbody2D _rigidbody2D;
    public GameObject enemyBullet;
    public GameObject orb;
    public GameObject coin;
    private SpriteRenderer _renderer;

    private CountDown _shootingCountdown = new CountDown(3f);
    public EnemySpawnController dc;

    public EnemyType typing;
    public bool inLIght;
    public ParticleSystem[] _Particles;
    private CountDown soundCD = new CountDown(5f + new Random().Next(50));
    private Color origin = Color.white;


    private bool died;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _health = maxHealth;
        script = _player.GetComponent<PlayerMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        switch (typing)
        {
            case EnemyType.Shooting:
                _concreteDustBoy = new ShootingDustBoy();
                break;
            case EnemyType.Charging:
                _concreteDustBoy = new ChargingDustBoy();
                _renderer.color = Color.green;
                origin = Color.green;
                break;
        }
    }

    private void Update()
    {
        soundCD.Start(walk);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (died)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }
        //Vector3.MoveTowards(transform.position, script.positionSnapshot, 3 * Time.deltaTime);
        _shootingCountdown.Start(Shoot);
        Moving();
    }


    void Shoot()
    {
         if (_concreteDustBoy.Shoot(script.positionSnapshot, transform.position) && !_lock)
         {
             Vector3 spawnPos = transform.position;
             for (int i = 0; i < 1; i++)
             {
                 SoundManager.PlaySound(SoundManager.Sound.DustBoyAttack, 0.8f);
                 Vector3 position = spawnPos;
                 position.x += i;
                 var clone = Instantiate(enemyBullet, spawnPos, Quaternion.identity);
                 Physics2D.IgnoreCollision(clone.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                 float angle = (float) ((float) Math.Atan2(_player.transform.position.y - transform.position.y, _player.transform.position.x - transform.position.x) * 180 / Math.PI);
                 angle += (i * 15);
                 Rigidbody2D r = clone.GetComponent<Rigidbody2D>();
                 clone.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                 r.AddForce(Quaternion.Euler(0,0,angle) * Vector2.right * 150);
             }
             _lock = true;
            
        }
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.rotation = 0f;
        _lock = false;
    }

    void Moving()
    {
        Vector2 velocity;
        if (inLIght)
        {
            velocity = _concreteDustBoy.LightMovement(_player.transform.position, transform.position);
        } 
        else velocity = _concreteDustBoy.Movement(_player.transform.position, transform.position);

        if (_concreteDustBoy.GetType() == typeof(ShootingDustBoy))
        {
            Vector2 translatedVelocity = Vector2.MoveTowards(transform.position, velocity, 2 * Time.deltaTime);
            _rigidbody2D.MovePosition(translatedVelocity);
            var offset = translatedVelocity - (Vector2) transform.position;
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            foreach (var part in _Particles)
            {
                part.gameObject.transform.rotation = Quaternion.AngleAxis(ReverseAngle(angle), Vector3.forward);
            }
        }

        else
        {
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            _rigidbody2D.velocity = velocity;
            foreach (var part in _Particles)
            {
                part.gameObject.transform.rotation = Quaternion.AngleAxis(ReverseAngle(angle), Vector3.forward);
            }
        }
        
        _Animator.SetBool("isWalking", true);
        _Animator.SetFloat("x", _rigidbody2D.velocity.x);
        _Animator.SetFloat("y", _rigidbody2D.velocity.y);

    }
    private bool damaged;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        {
            damaged = true;
            _health -= BuffManager.GetBuff(BuffManager.BuffType.damage).currentValue;
            if (_health <= 0)
            {
                StartPerish();
            }
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(resetDamageIndication());
        } else if (other.gameObject.layer == 11)
        {
            SoundManager.PlaySound(SoundManager.Sound.DustBoyAttack);
        }
    }

    IEnumerator resetDamageIndication()
    {
        yield return new WaitForSeconds(0.5f);
        _renderer.color = origin;
        damaged = false;
    }

    private void walk()
    {
        SoundManager.PlaySound(SoundManager.Sound.DustBoyWalk, 0.8f);
    }

    private void StartPerish()
    {
        _Animator.SetBool("died", true);
        died = true;
    }

    private void Perish()
    {
        SoundManager.PlaySound(SoundManager.Sound.DustBoyDeath);
        ScoreUI.instance.UpdateValue(1);
        if (new Random().Next(100) < 5)
            Instantiate(orb, transform.position, Quaternion.identity);
        dc._currentEnemies -= 1;
        if (dc._currentEnemies <= 0)
        {
            dc.ClearEvent.Invoke();
        }
        Destroy(gameObject);
    }

    public void setLIght(bool toggle)
    {
        inLIght = toggle;
        foreach (var part in _Particles)
        {
            part.gameObject.SetActive(!toggle);
        }
        
        Color col = _renderer.color;
        col.a = toggle ? 1 : 0.5f;
        _renderer.color = col;
    }
}
