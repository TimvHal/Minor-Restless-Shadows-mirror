using System;
using System.Collections.Generic;
using Transitions;
using UnityEngine;
using Utils;
using Image = UnityEngine.UI.Image;

public class HealthController : MonoBehaviour
{
    private float Health = 20;
    public float stage = 4;
    private float division;

    public GameObject bossbar;
    public GameObject bossbarOverlay;
    private RectTransform BBT;
    private SnowballStateMachine stateScript;
    private SpriteRenderer _renderer;
    private Image _overlayImage;
    private Color _color;
    private Color _overlayColor;
    public bool died;
    
    // Start is called before the first frame update
    void Start()
    {
        bossbar = GameObject.FindWithTag("BossHealthBar");
        bossbarOverlay = GameObject.Find("HealthOverlay");
        division = Health / (stage);
        stateScript = GetComponent<SnowballStateMachine>();

        _renderer = bossbar.GetComponent<SpriteRenderer>();
        _overlayImage = bossbarOverlay.GetComponent<Image>();
        
        _color = _renderer.color;
        _overlayColor = _overlayImage.color;

    }
    

    // Update is called once per frame
    void Update()
    {
        if (Health % division == 0)
        {
            if (Math.Round(Health / division, 0) == stage){
                stage -= 1;
                stateScript.attackQuantity += 1;
            }
        }

        if (Health <= 0)
        {
            StartPerish();
        }
    }

    private void DepleteBossHealthbar(float damageInflicted)
    {
        if (died || bossbar.transform.localScale.x < 0) return;
        bossbar.transform.localScale -= new Vector3(damageInflicted * (312f/20f), 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        {
            var damageInflicted = BuffManager.GetBuff(BuffManager.BuffType.damage).currentValue;
            Health -= damageInflicted;
            DepleteBossHealthbar(damageInflicted);
            
            Destroy(other.gameObject);
        }
    }

    private void StartPerish()
    {
        GetComponent<Animator>().SetBool("Death", true);
        foreach (var dum in stateScript.dummies)
        {
            if (dum != null)
                dum.GetComponent<Animator>().SetBool("death", true);
        }
        died = true;
    }

    private void Perish()
    {
        SoundManager.StopSound(SoundManager.Sound.SnowballTheme);
        GameState.Instance.SetEnd(true);
        FindObjectOfType<LevelLoader>().GetComponent<LevelLoader>().LoadNextLevel();
         _color.a = 0;
        _overlayColor.a = 0;
        _renderer.color = _color;
        _overlayImage.color = _overlayColor;
    }
}
