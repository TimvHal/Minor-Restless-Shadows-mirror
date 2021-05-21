using System;
using System.Collections;
using UnityEngine;

namespace Enemy.Ray
{
    public class CircusBossTransition : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private  GameObject ray;

        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private ParticleSystem _particleSystem;

        private bool _active;

        private void Start()
        {
            _active = false;
            _canvas = GameObject.Find("RayBossSplashScreen").GetComponent<Canvas>();
            _canvasGroup = _canvas.GetComponentInChildren<CanvasGroup>();
            _particleSystem = _canvas.GetComponentInChildren<ParticleSystem>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 13)
            {
                player.transform.position = new Vector3(0, 30, -1);
                StartCoroutine(SpawnBoss());
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
            SoundManager.PlaySound(SoundManager.Sound.RayTheme, 0.5f, true);
            _particleSystem.Clear();
            _canvas.enabled = false;
            
            ray.SetActive(true);
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
}
