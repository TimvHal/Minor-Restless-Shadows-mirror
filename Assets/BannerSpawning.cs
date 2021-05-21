using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BannerSpawning : MonoBehaviour
{
    public static BannerSpawning instance;
    
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    private Image Image;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Image = GetComponent<Image>();
        ToggleExisteence(false);
    }

    public void Spawn(BuffManager.BuffType type)
    {
        GameAssets.Buff buff = BuffManager.GetBuff(type);
        Title.text = type.ToString().First().ToString().ToUpper() + type.ToString().Substring(1);
        Description.text = buff.description;
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        ToggleExisteence(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(fadeout());
    }

    IEnumerator fadeout()
    {
        var col = Image.color;
        col.a *= 0.95f;
        Image.color = col;
        var tcol = Title.color;
        tcol.a = col.a;
        Title.color = tcol;
        var dcol = Description.color;
        dcol.a = col.a;
        Description.color = dcol;
        yield return new WaitForFixedUpdate();
        if (col.a >= 0.5) StartCoroutine(fadeout());
        else ToggleExisteence(false);
    }

    void ToggleExisteence(bool enable)
    {
        var col = Image.color;
        col.a = enable ? 1 : 0;
        var tcol = Title.color;
        tcol.a = col.a;
        Title.color = tcol;
        var dcol = Description.color;
        dcol.a = col.a;
        Description.color = dcol;
        
        Image.color = col;
        Title.enabled = enable;
        Description.enabled = enable;
    }

}
