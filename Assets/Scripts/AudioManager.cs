using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip clipJump;
    public AudioClip clipBigJump;
    public AudioClip clipCoin;
    public AudioClip clipStomp;
    public AudioClip clipFlipDie;
    public AudioClip clipShoot;
    public AudioClip clipPowerUp;
    public AudioClip clipPowerDown;
    public AudioClip clipPowerUpAppear;
    public AudioClip clipBreak;
    public AudioClip clipBump;
    public AudioClip clipDie;
    public AudioClip clipFlagPole;
    public AudioClip clipOneUp;


    public static AudioManager Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(clipJump);
    }

    public void PlayBigJump()
    {
        audioSource.PlayOneShot(clipBigJump);
    }

    public void PlayCoin()
    {
        audioSource.PlayOneShot(clipCoin);
    }

    public void PlayStomp()
    {
        audioSource.PlayOneShot(clipStomp);
    }

    public void PlayFlipDie()
    {
        audioSource.PlayOneShot(clipFlipDie);
    }

    public void PlayShoot()
    {
        audioSource.PlayOneShot(clipShoot);
    }

    public void PlayPowerUp()
    {
        audioSource.PlayOneShot(clipPowerUp);
    }

    public void PlayPowerDown()
    {
        audioSource.PlayOneShot(clipPowerDown);
    }

    public void PlayPowerUpAppear()
    {
        audioSource.PlayOneShot(clipPowerUpAppear);
    }

    public void PlayBreak()
    {
        audioSource.PlayOneShot(clipBreak);
    }

    public void PlayBump()
    {
        audioSource.PlayOneShot(clipBump);
    }

    public void PlayDie()
    {
        audioSource.PlayOneShot(clipDie);
    }

    public void PlayFlagPole()
    {
        audioSource.PlayOneShot(clipFlagPole);
    }
    public void PlayOneUp()
    {
        audioSource.PlayOneShot(clipOneUp);
    }
}
