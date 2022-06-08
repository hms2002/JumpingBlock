using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip MainMenuBGM;
    public AudioClip FightBGM;

    public AudioClip BoomEffectSound1;
    public AudioClip BoomEffectSound2;
    public AudioClip BoomEffectSound3;

    public AudioClip BuildBlockEffectSound;

    public AudioClip SelectEffectSound;

    public AudioClip[] Player_SkillEffect_Sound = new AudioClip[4];

    AudioSource audioSource;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0.02f;

        // 씬 이동해도 유지
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        switch(scene.buildIndex)
        {
            case (int)GameManager.BuildIdx.MainMenu:
                audioSource.clip = MainMenuBGM;
                audioSource.Play();
                break;
            case (int)GameManager.BuildIdx.InGameScene:
                audioSource.clip = FightBGM;
                audioSource.Play();
                break;
        }
    }

    public void PlayFaster()
    {
        audioSource.pitch = 1.5f;
    }

    public void PlaySelectSound()
    {
        audioSource.PlayOneShot(SelectEffectSound, 2f);
    }

    public void PlayBoomSound()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioSource.PlayOneShot(BoomEffectSound1);
                break;
            case 1:
                audioSource.PlayOneShot(BoomEffectSound2);
                break;
            case 2:
                audioSource.PlayOneShot(BoomEffectSound3);
                break;
        }

    }
    public void PlayBuildBlockEffectSound()
    {
        audioSource.PlayOneShot(BuildBlockEffectSound);
    }
    public void PlayPlayerSkillEffectSound(int idx)
    {
        audioSource.PlayOneShot(Player_SkillEffect_Sound[idx], 2);
    }
}
