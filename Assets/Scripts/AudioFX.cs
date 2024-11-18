using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFX : MonoBehaviour
{
    [SerializeField] private AudioClip[] fxs;
    [SerializeField] private AudioSource audioSource;

    // 0 buttonclick
    // 1 collectcoin
    // 2 gameloop
    // 3 normalclick
    // 4 wronganswer
    // Reproduce un efecto de sonido seg�n el �ndice
    public void PlaySound(int soundIndex)
    {
        if (soundIndex >= 0 && soundIndex < fxs.Length)
        {
            audioSource.clip = fxs[soundIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("�ndice de sonido fuera de rango: " + soundIndex);
        }
    }
}
