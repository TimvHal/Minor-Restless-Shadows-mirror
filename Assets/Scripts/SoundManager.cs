using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        PlayerShoot,
        PlayerReload,
        DustBoyAttack,
        DustBoyWalk,
        StartGame,
        buttonPress,
        tileMusic,
        OverWorldBase,
        OverworldBattle,
        OverworldStart,
        RayAnimalRail,
        RayThrow,
        RayBounceBall,
        NurseDeath,
        NurseWalk,
        NurseAttack,
        DustBoyDeath,
        PlagaAcidLob,
        PlagaAcidSpray,
        PlagaAcidBulletSpray,
        GenericThrow,
        GenericDeath,
        SnowballFireball,
        GenericHit,
        BuffPickUp,
        GameOver,
        PlagaTheme,
        RayTheme,
        SnowballTheme
    }

    public static GameObject soudGameObject;
    public static Dictionary<Sound, AudioSource> currentlyActive = new Dictionary<Sound, AudioSource>();
    
    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void PlaySound(Sound sound)
    {
        PlaySound(sound, 1, false);
    }
    public static void PlaySound(Sound sound, float volume)
    {
        PlaySound(sound, volume, false);
    }
    public static void PlaySound(Sound sound, bool loop)
    {
        PlaySound(sound, 1, loop);
    }
    public static void PlaySound(Sound sound, float volume, bool loop)
    {
        if (inList(sound)) currentlyActive.Remove(sound);
        if (soudGameObject == null) soudGameObject = new GameObject("Sound");
        AudioSource audioSource = soudGameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        
        audioSource.clip = getAudioClip(sound);
        audioSource.loop = loop;
        audioSource.Play();
        currentlyActive.Add(sound, audioSource);
    }
    
    public static void StopSound(Sound sound)
    {
        foreach (var soundPair in currentlyActive)
        {
            if (soundPair.Key == sound) soundPair.Value.Stop();
        }
    }

    private static bool inList(Sound sound)
    {
        foreach (var soundPair in currentlyActive)
        {
            if (soundPair.Key == sound) return true;
        }

        return false;
    }
    private static AudioClip getAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.SoundAudioClips)
        {
            if (soundAudioClip.Sound == sound)
            {
                return soundAudioClip.AudioClip;
            }
        }
        Debug.LogError("Sound" + sound + "Does not exist" );
        return null;
    }

}
