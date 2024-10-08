using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    private AudioSource audioSource;
    public bool sound = true;

    private void Awake()
    {
        makeSingleton();
        audioSource = GetComponent<AudioSource>();
    }

    private void makeSingleton() // Sahne değiştiğinde skorun sıfırlanmaması için
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }


    public void SoundOnOff() // Ses açıp kapatan fonksiyon
    {
        sound = !sound;
    }

    public void playAudio(AudioClip clip,float volume) // Ses eklemizi sağlayan fonksiyon
    {
        if (sound)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

}
