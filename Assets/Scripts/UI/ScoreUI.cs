using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private static ScoreUI _i;
    public static ScoreUI instance
    {
        get
        {
            return _i;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        _i = this;
        DontDestroyOnLoad(gameObject);
    }

    private TextMeshPro text;
    private void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.sortingOrder = 10000;
        UpdateValue(0);
    }

    public void UpdateValue(int incr)
    {
        if (GameAssets.i.score + incr < 0) return;
        GameAssets.i.score += incr;
        var display = GameAssets.i.score.ToString();
        if (GameAssets.i.score <= 9) display = "0" + display;
        text.text = "Score: " + display;
    }
}