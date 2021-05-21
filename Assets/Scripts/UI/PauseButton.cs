using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject _c;
    public bool TimeTOggle = false;
    public float scale;

    private void Start()
    {
        TimeTOggle = Time.timeScale == 0;
        _c.SetActive(false);;
        scale = Time.timeScale;
    }

    void Toggletime()
    {;
        _c.SetActive(TimeTOggle);
        if (TimeTOggle) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TimeTOggle = Time.timeScale == 0;
        TimeTOggle = !TimeTOggle;
        SoundManager.PlaySound(SoundManager.Sound.buttonPress);
        Toggletime();
    }
}
