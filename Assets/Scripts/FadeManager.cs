using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    const int FADE_IMG_IDX = 0;

    Image fadeImg;

    public bool onFade = false;

    private void Start()
    {
        fadeImg = transform.GetChild(FADE_IMG_IDX).GetComponent<Image>();
    }

    public void fadeIn()
    {
        StartCoroutine("IFadeIn");
    }

    public void fadeOut()
    {
        StartCoroutine("IFadeOut");
    }

    // ������°�
    IEnumerator IFadeOut()
    {
        onFade = true;
        while(fadeImg.color.a > 0)
        {
            Debug.Log("D");
            Color tempColor = fadeImg.color;
            tempColor.a -= 0.01f;
            fadeImg.color = tempColor;
            yield return new WaitForSeconds(0.01f);
        }
        onFade = false;
    }

    // ��ο����°�
    IEnumerator IFadeIn()
    {
        onFade = true;
        while (fadeImg.color.a < 1)
        {
            Color tempColor = fadeImg.color;
            tempColor.a += 0.01f;
            fadeImg.color = tempColor;
            yield return new WaitForSeconds(0.01f);
        }
        onFade = false;
    }
}
