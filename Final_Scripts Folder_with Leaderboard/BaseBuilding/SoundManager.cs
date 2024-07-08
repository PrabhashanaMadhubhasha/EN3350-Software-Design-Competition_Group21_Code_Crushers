using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    [Header("SFX")]
    public AudioSource dropItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickItemSound;
    public AudioSource grassWalkSound;
    public AudioSource grassSprintSound;
    public AudioSource bloodSound;

    public AudioSource meleeWeaponDrawSound;
    public AudioSource meleeWeaponHitSound;

    public AudioSource enemyDeathSound;

    public AudioSource treeFallingSound;

    public AudioSource gunParticleEmitSound;
    public AudioSource gunParticleHitSound;

    [Header("Music")]
    public AudioSource startingZoneBGMusic;
    public AudioSource birdMusic_1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Play the corresponding sound
    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }
}
