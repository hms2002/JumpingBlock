using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

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
        audioSource.volume = 0.05f;

        // 씬 이동해도 유지
        DontDestroyOnLoad(gameObject);
    }

    public void PlayFaster()
    {
        audioSource.pitch = 1.5f;
    }
}
