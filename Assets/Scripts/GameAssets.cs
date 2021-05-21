using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    public int score;

    public SoundAudioClip[] SoundAudioClips;
    public Buff[] BuffStats;
    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound Sound;
        public AudioClip AudioClip;
    }
    [Serializable]
    public class Buff
    {
        public BuffManager.BuffType type;
        public float currentValue;
        public float maxValue;
        public float increaseBy;
        public string description;
    }

    [Serializable]
    public class BuffSprite
    {
        public BuffManager.BuffType type;
        public Sprite sprite;
    }
    public BuffSprite[] BuffSprites;
    
    [Serializable]
    public class Cheats
    {
        public CheatManager.Cheat cheat;
        public bool active;
    }

    public Cheats[] Cheatses;

    public float MusicVolume;
    public float SFXVolume;
}
