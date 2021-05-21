using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckPlagaActive : MonoBehaviour
{
    [FormerlySerializedAs("slidingDoor")] [FormerlySerializedAs("sldingDoor")] public DoorAnimator doorAnimator;
    public GameObject plaga;
    public GameObject Healthbar;
    private SpriteRenderer _renderer;
    private Color _color;

    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private ParticleSystem _particleSystem;

    private bool _active;

    private void Start()
    {
        Healthbar = GameObject.FindWithTag("BossHealthBar");
        _renderer = Healthbar.GetComponent<SpriteRenderer>();
        _color = _renderer.color;
        _active = false;
        _canvas = GameObject.Find("PlagaBossSplashScreen").GetComponent<Canvas>();
        _canvasGroup = _canvas.GetComponentInChildren<CanvasGroup>();
        _particleSystem = _canvas.GetComponentInChildren<ParticleSystem>();
    }

    private void BootPlaga()
    {
        StartCoroutine(SpawnBoss());
        doorAnimator.dc._currentEnemies = 1;
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
        _particleSystem.Clear();
        _canvas.enabled = false;
        SoundManager.PlaySound(SoundManager.Sound.PlagaTheme, GameAssets.i.MusicVolume, true);

        _color.a = 1;
        _renderer.color = _color;
        plaga.SetActive(true);
    }

    private void Update()
    {
        if (plaga == null || doorAnimator == null) return;
        if (doorAnimator.ctg.playerIsInside && !_active) BootPlaga();
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
