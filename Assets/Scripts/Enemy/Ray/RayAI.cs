using System;
using System.Collections;
using Enemy.Ray;
using Transitions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class RayAI : MonoBehaviour
{
    private RayState _state;
    private GameObject _player;
    private SpriteRenderer _renderer;
    [SerializeField] private Animator rayAC;
    
    public GameObject _bouncyBall;
    public GameObject _bouncyCluster;
    public GameObject _cardboardAnimal;
    public GameObject _balloon;
    public GameObject _jugglingPin;

    private Vector3 _rayPos;
    private Vector3 _leftUpperCannon;
    private Vector3 _rightUpperCannon;
    private Vector3 _rightLowerCannon;
    private Vector3 _leftLowerCannon;

    private Random _rand;

    private GameObject _healthbar;

    private RayState[] _states =
    {
        RayState.Idle, RayState.BouncyBall, RayState.BouncyCluster, 
        RayState.CardboardAnimal, RayState.Balloon, RayState.JugglingPin
    };

    private float _health;
    private bool _dead;

    [SerializeField] private Sprite rayOverlay;

    // Start is called before the first frame update
    void Start()
    {
        _state = RayState.Idle;
        _player = GameObject.FindGameObjectWithTag("Player");
        _renderer = GetComponent<SpriteRenderer>();
        _rayPos = transform.position;

        _leftUpperCannon = _rayPos + new Vector3(-4f, -4.3f, 0);
        _rightUpperCannon = _rayPos + new Vector3(4f, -4.3f, 0);
        _rightLowerCannon = _rayPos + new Vector3(6f, -6.3f, 0);
        _leftLowerCannon = _rayPos + new Vector3(-6f, -6.3f, 0);

        _rand = new Random();
    
        _healthbar = GameObject.FindWithTag("BossHealthBar");
        _health = 150;
        _dead = false;

        ShowHealthBar();
        
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        switch (_state)
        {
            case RayState.Idle:
                break;
            case RayState.BouncyBall:
                ShootBouncyBalls();
                break;
            case RayState.BouncyCluster:
                LobBouncyCluster();
                break;
            case RayState.CardboardAnimal:
                SpawnCardboardAnimals();
                break;            
            case RayState.Balloon:
                SpawnBalloons();
                break;            
            case RayState.JugglingPin:
                SpawnJugglingPin();
                break;
            case RayState.Death:
                StartCoroutine(Perish());
                break;
        }
        
        yield return new WaitForSeconds(_rand.Next(2, 5));
        if(_state != RayState.Death) ChangeState(); StartCoroutine(Attack());
    }

    void ShootBouncyBalls()
    {
        bool toggleArm = true;
        for (int i = 1; i < 3; i++)
        {
            var clone1 = Instantiate(_bouncyBall, toggleArm ? _leftUpperCannon : _rightUpperCannon, 
                Quaternion.identity);
            var clone2 = Instantiate(_bouncyBall, toggleArm ? _leftLowerCannon : _rightLowerCannon, 
                Quaternion.identity);
            toggleArm = !toggleArm;
        }
    }

    void LobBouncyCluster()
    {
        Instantiate(_bouncyCluster, transform.position, Quaternion.identity);
    }

    void SpawnCardboardAnimals()
    {
        for (int index = 0; index < 3; index++)
        {
            Instantiate(_cardboardAnimal, _rayPos + new Vector3(-10, -2.5f - (index * 3), 0), 
                Quaternion.identity);
        }
    }

    void SpawnBalloons()
    {
        var amount = _rand.Next(5, 11);
        for (int index = 0; index < amount; index++)
        {
            SpawnRandomBalloon();
        }
    }

    void SpawnRandomBalloon()
    {
        var x = _rand.Next(-4, 5);
        Instantiate(_balloon, transform.position + new Vector3(x, 10, 0), Quaternion.identity);
    }

    void SpawnJugglingPin()
    {
        Vector3[] cannons = { _leftLowerCannon, _leftUpperCannon, _rightLowerCannon, _rightUpperCannon };
        var index = _rand.Next(4);
        Instantiate(_jugglingPin, cannons[index], Quaternion.identity);
    }

    void ChangeState()
    {
        while (true)
        {
            var index = _rand.Next(0, 6);

            if (_state == _states[index]) continue;

            _state = _states[index];
            break;
        }
    }

    void SetState(RayState state)
    {
        _state = state;
    }
    
    void TakeDamage(float damageDealt)
    {
        if (_dead) return;
        StartCoroutine(BlinkRed());
        if (_health <= 0)
        {
            _dead = true; 
            SetState(RayState.Death);
            return;
        }
        UpdateBossHealthbar(damageDealt);
    }

    IEnumerator BlinkRed()
    {
        _renderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _renderer.color = Color.white;
    }

    IEnumerator Perish()
    {
        const string deathTrigger = "Die";
        rayAC.SetTrigger(deathTrigger);
        GameState.Instance.SetEnd(true);
        SoundManager.StopSound(SoundManager.Sound.RayTheme);
        yield return new WaitForSeconds(2f);
        FindObjectOfType<LevelLoader>().LoadMainMenu();
    }

    private void UpdateBossHealthbar(float damageInflicted)
    {
        if (_healthbar.transform.localScale.x - damageInflicted * (253f / 150f) < 0)
        {
            _healthbar.transform.localScale = new Vector3(0, 35, 20);
            return;
        }
        _healthbar.transform.localScale -= new Vector3(damageInflicted * (253f / 150f), 0, 0);
        _health -= damageInflicted;
    }
    
    private void ShowHealthBar()
    {
        GameObject.FindWithTag("BossHealthBar").GetComponent<SpriteRenderer>().color = Color.red;
        GameObject.Find("HealthOverlay").GetComponent<Image>().sprite = rayOverlay;
        GameObject.Find("HealthOverlay").GetComponent<Image>().color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == 10) TakeDamage(BuffManager.GetBuff(BuffManager.BuffType.damage).currentValue);
    }
}
