using System;
using System.Collections;
using DefaultNamespace.Plaga;
using Transitions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class PlagaAI : MonoBehaviour
{
    private PlagaState _state;
    private GameObject _player;
    private SpriteRenderer _renderer;
    [SerializeField] private Animator plagaAC;
    
    public GameObject _strayAcidBullet;
    public GameObject _acidBullet;
    public GameObject _acidNeedle;
    public GameObject _acidCluster;
    public GameObject _acidStream;

    private Vector3 _plagaPos;
    private Vector3 _leftUpperArm;
    private Vector3 _rightUpperArm;
    private Vector3 _rightLowerArm;
    private Vector3 _leftLowerArm;

    private Random _rand;

    private GameObject _healthbar;
    private Image _overlayImage;
    [SerializeField] private Sprite plagaOverlay;
    private SpriteRenderer _healthRenderer;
    private bool _dead;

    private PlagaState[] _states =
    {
        PlagaState.Idle, PlagaState.Lobacidcluster, PlagaState.Shootacid,
        PlagaState.Shootacidstream, PlagaState.Shootneedle
    };

    private float _health;

    // Start is called before the first frame update
    void Start()
    {
        _state = PlagaState.Idle;
        _player = GameObject.FindGameObjectWithTag("Player");
        _renderer = GetComponent<SpriteRenderer>();
        _plagaPos = transform.position;

        _leftUpperArm = _plagaPos + new Vector3(-0.9f, 0.8f, 0);
        _rightUpperArm = _plagaPos + new Vector3(0.9f, 0.7f, 0);
        _rightLowerArm = _plagaPos + new Vector3(0.6f, -0.4f, 0);
        _leftLowerArm = _plagaPos + new Vector3(-0.5f, -0.3f, 0);

        _rand = new Random();
    
        _healthbar = GameObject.FindWithTag("BossHealthBar");
        _healthRenderer = _healthbar.GetComponent<SpriteRenderer>();
        _overlayImage = GameObject.Find("HealthOverlay").GetComponent<Image>();
        SoundManager.PlaySound(SoundManager.Sound.PlagaTheme, GameAssets.i.MusicVolume, true);
        _health = 100;

        _dead = false;

        ShowHealthBar();

        StartCoroutine(Attack());
    }

    private void FixedUpdate()
    {
        UpdateSprite();
    }

    IEnumerator Attack()
    {
        switch (_state)
        {
            case PlagaState.Idle:
                break;
            case PlagaState.Shootacid:
                StartCoroutine(ShootAcid());
                break;
            case PlagaState.Lobacidcluster:
                LobAcidCluster();
                break;
            case PlagaState.Shootacidstream:
                ShootAcidStream();
                break;
            case PlagaState.Shootneedle:
                ShootNeedle();
                break;
            case PlagaState.Death:
                StartCoroutine(Perish());
                break;
        }

        
        yield return new WaitForSeconds(_rand.Next(1, 4));
        if(_state != PlagaState.Death) ChangeState(); StartCoroutine(Attack());
    }

    IEnumerator ShootAcid()
    {
        bool toggleArm = true;
        for (int i = 1; i < 3; i++)
        {
            var clone1 = Instantiate(_strayAcidBullet, toggleArm ? _leftUpperArm : _rightUpperArm, Quaternion.identity);
            var clone2 = Instantiate(_strayAcidBullet, toggleArm ? _leftLowerArm : _rightLowerArm, Quaternion.identity);
            clone1.GetComponent<Rigidbody2D>().rotation = i * -5 - 10;
            clone2.GetComponent<Rigidbody2D>().rotation = i * 5 + 10;
            toggleArm = !toggleArm;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 2; i++)
        {
            Instantiate(_acidBullet, toggleArm ? _leftUpperArm : _rightUpperArm, Quaternion.identity);
            Instantiate(_acidBullet, toggleArm ? _leftLowerArm : _rightLowerArm, Quaternion.identity);
            toggleArm = !toggleArm;
        }
    }

    void LobAcidCluster()
    {
        StartCoroutine(LobClusters());
    }

    void ShootAcidStream()
    {
        var spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Instantiate(_acidStream, spawnLocation, Quaternion.identity);
    }

    void ShootNeedle()
    {
        Instantiate(_acidNeedle, _leftLowerArm, Quaternion.identity);
    }

    void ChangeState()
    {
        while (true)
        {
            var index = _rand.Next(0, 5);

            if (_state == _states[index]) continue;

            _state = _states[index];
            break;
        }
    }

    IEnumerator LobClusters()
    {
        Instantiate(_acidCluster, _rightUpperArm, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_acidCluster, _leftUpperArm, Quaternion.identity);
    }

    public void TakeDamage(float damageDealt)
    {
        if (_dead) return;
        StartCoroutine(BlinkRed());
        if (_health <= 0)
        {
            SetState(PlagaState.Death);
            _dead = true;
            return;
        }
        DepleteBossHealthbar(damageDealt);
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
        plagaAC.SetTrigger(deathTrigger);
        GameState.Instance.SetEnd(true);
        SoundManager.StopSound(SoundManager.Sound.PlagaTheme);
        yield return new WaitForSeconds(2f);
        FindObjectOfType<LevelLoader>().LoadNextLevel();
        SoundManager.PlaySound(SoundManager.Sound.OverWorldBase);
    }

    void UpdateSprite()
    {
        var offset = _player.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        if (angle < 90 && angle > -90)
        {
            _renderer.flipX = true;
            return;
        }
        _renderer.flipX = false;
    }
    
    private void DepleteBossHealthbar(float damageInflicted)
    {
        _health -= damageInflicted;
        if (_healthbar.transform.localScale.x - damageInflicted * (253f / 100f) < 0)
        {
            _healthbar.transform.localScale = new Vector3(0, 35, 20);
            return;
        }
        _healthbar.transform.localScale -= new Vector3(damageInflicted * (253f/100f), 0f, 0f);
    }

    private void ShowHealthBar()
    {
        GameObject.FindWithTag("BossHealthBar").GetComponent<SpriteRenderer>().color = Color.red;
        GameObject.Find("HealthOverlay").GetComponent<Image>().sprite = plagaOverlay;
        GameObject.Find("HealthOverlay").GetComponent<Image>().color = Color.white;
    }

    void SetState(PlagaState state)
    {
        _state = state;
    }
}
