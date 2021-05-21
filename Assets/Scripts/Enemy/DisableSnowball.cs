using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableSnowball : MonoBehaviour
{
    public DoorDeletion dc;
    public GameObject snowball;
    private GameObject _bossHealthBar;
    private GameObject _bossHealthBarOverlay;
    private SpriteRenderer _bossHealthImage;
    private Image _bossOverlayImage;
    private Color _color;
    private Color _overlayColor;

    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private ParticleSystem _particleSystem;

    private bool _active;


    private void Start()
    {
        snowball = GameObject.FindWithTag("Snowball");
        _bossHealthBar = GameObject.FindWithTag("BossHealthBar");
        _bossHealthBarOverlay = GameObject.Find("HealthOverlay");
        snowball.SetActive(false);
        
        _bossHealthImage = _bossHealthBar.GetComponent<SpriteRenderer>();
        _bossOverlayImage = _bossHealthBarOverlay.GetComponent<Image>();
        
        _color = _bossHealthImage.color;
        _overlayColor = _bossOverlayImage.color;
        
        _color.a = 0;
        _bossHealthImage.color = _color;
        _overlayColor.a = 0;
        _bossOverlayImage.color = _overlayColor;
        
        _active = false;
        _canvas = GameObject.Find("SnowballBossSplashScreen").GetComponent<Canvas>();
        _canvasGroup = _canvas.GetComponentInChildren<CanvasGroup>();
        _particleSystem = _canvas.GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (dc != null)
        {
            if (dc.gameObject.GetComponent<DoorAnimator>().ctg.playerIsInside)
            {
                dc._currentEnemy = 1;
                if (snowball != null && !_active) StartCoroutine(SpawnBoss());

            }
        } 
    }

    IEnumerator SpawnBoss()
    {
        SoundManager.StopSound(SoundManager.Sound.OverWorldBase);
        _active = true;
        _canvas.enabled = true;
        _particleSystem.Play();
        
        StartCoroutine(FadeInSplashScreen());
        yield return new WaitForSeconds(5f);
        _particleSystem.Stop();
        StartCoroutine(FadeOutSplashScreen());
        yield return new WaitForSeconds(0.25f);
        SoundManager.PlaySound(SoundManager.Sound.SnowballTheme, GameAssets.i.MusicVolume, true);
        _particleSystem.Clear();
        _canvas.enabled = false;

        _color.a = 1;
        _overlayColor.a = 1;
        _bossHealthImage.color = _color;
        _bossOverlayImage.color = _overlayColor;
        snowball.SetActive(true);
    }
    
    IEnumerator FadeInSplashScreen()
    {
        _canvasGroup.alpha += 0.1f;
        yield return new WaitForSeconds(0.025f);
        if (_canvasGroup.alpha < 1f) StartCoroutine(FadeInSplashScreen());
    }

    IEnumerator FadeOutSplashScreen()
    {
        _canvasGroup.alpha -= 0.1f;
        yield return new WaitForSeconds(0.025f);
        if (_canvasGroup.alpha > 0f) StartCoroutine(FadeOutSplashScreen());
    }
}
