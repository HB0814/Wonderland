using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("오디오 소스")]
    public AudioSource bgmSource;
    [SerializeField] private List<AudioSource> sfxSources;
    private int currentSfxIndex = 0;

    [Header("무기 오디오 클립")]
    public AudioClip bookAttackSound;
    public AudioClip watchAttackSound;
    public AudioClip teaAttackSound;
    public AudioClip catAttackSound;
    public AudioClip swordAttackSound;
    public AudioClip hatAttackSound;
    public AudioClip cardAttackSound;
    public AudioClip appleAttackSound;
    public AudioClip defaultAttackSound;

    [Header("몬스터 오디오 클립")]
    public AudioClip fixedGuillotione;
    public AudioClip movedGuillotione;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip enemyDeath;
    [SerializeField] private AudioClip enemyExplosion;

    [Header("기타 오디오 클립")]
    [SerializeField] private AudioClip heal;
    [SerializeField] private AudioClip expGem;
    [SerializeField] private AudioClip levelUp;
    [SerializeField] private AudioClip magnet;

    float hitTimer = 0;
    float expTimer = 0;
    float deathTimer = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        hitTimer += Time.deltaTime;
        expTimer += Time.deltaTime;
        deathTimer += Time.deltaTime;
    }

    public void PlaySFX(string effect)
    {
        AudioSource source = sfxSources[currentSfxIndex];

        switch (effect)
        {
            case "heal":
                source.pitch = Random.Range(0.9f, 1.1f);
                source.PlayOneShot(heal);
                break;

            case "expGem":
                if (expTimer > 0.08f)
                {
                    expTimer = 0;
                    source.pitch = Random.Range(0.9f, 1.1f);
                    source.PlayOneShot(expGem);
                }
                break;

            case "levelUp":
                source.PlayOneShot(levelUp);
                break;

            case "enemyDeath":
                if(deathTimer > 0.1f)
                {
                    deathTimer = 0;
                    source.pitch = Random.Range(0.8f, 1.2f);
                    source.PlayOneShot(enemyDeath);
                }
                break;

            case "explosion":
                source.pitch = Random.Range(0.9f, 1.1f);
                source.PlayOneShot(enemyExplosion);
                break;

            case "magnet":
                source.PlayOneShot(magnet);
                break;

            case "fixedGuillotione":
                source.pitch = Random.Range(0.9f, 1.1f);
                source.PlayOneShot(fixedGuillotione);
                break;

            case "movedGuillotione":
                source.pitch = Random.Range(0.9f, 1.1f);
                source.PlayOneShot(movedGuillotione);
                break;
        }

        currentSfxIndex = (currentSfxIndex + 1) % sfxSources.Count;
    }

    public void PlayHitSound(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Pipe || weaponType == WeaponType.Firecracker)
            return;

        if (hitTimer > 0.1f)
        {
            AudioSource source = sfxSources[currentSfxIndex];

            hitTimer = 0;
            source.pitch = Random.Range(0.8f, 1.2f);
            source.PlayOneShot(enemyHit);

            currentSfxIndex = (currentSfxIndex + 1) % sfxSources.Count;
        }
    }

    public void PlayWeaponSound(WeaponType weaponType)
    {
        AudioSource source = sfxSources[currentSfxIndex];

        AudioClip clip = weaponType switch
        {
            WeaponType.Book => bookAttackSound,
            WeaponType.Watch => watchAttackSound,
            WeaponType.Cat => catAttackSound,
            WeaponType.Sword => swordAttackSound,
            WeaponType.Tea=> teaAttackSound,
            WeaponType.Hat=> hatAttackSound,
            WeaponType.Card => cardAttackSound,
            WeaponType.Apple=> appleAttackSound,
            _ => defaultAttackSound
        };

        if (clip != null)
        {
            source.pitch = Random.Range(0.9f, 1.1f);
            source.PlayOneShot(clip);
            currentSfxIndex = (currentSfxIndex + 1) % sfxSources.Count;
        }

    }
}