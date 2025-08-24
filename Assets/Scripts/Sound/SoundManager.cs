using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip clip;
}
public enum Sounds
{
    BackgroundMusic,
    buttonClick,
    PlayerMove,
    PlayerJump,
    PlayerDeath,
    ItemPickup,
    LevelComplete,
    GameOver
}
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    public SoundType[] soundTypes;
    public AudioSource soundEffect;
    public AudioSource soundMusic;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {
        PlaySound(Sounds.BackgroundMusic);
    }
    public void PlaySound(Sounds sound)
    {
        SoundType item = Array.Find(soundTypes, item => item.soundType == sound);
        if (item == null)
        {
            Debug.LogWarning("Sound not found: " + sound);
            return;
        }
        AudioClip clip = item.clip;
        if (clip != null)
        {
            if (sound == Sounds.BackgroundMusic)
            {
                soundMusic.clip = clip;
                soundMusic.Play();
            }
            else
            {
                soundEffect.PlayOneShot(clip);
            }
        }
        else
        {
            Debug.LogWarning("AudioClip is null for sound: " + sound);
        }
    }

}



