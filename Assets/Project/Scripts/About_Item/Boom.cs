using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    SpriteRenderer boomRenderer;

    float lifeTime = 2;
    float curTime = 2;

    private void Start()
    {
        boomRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        curTime = lifeTime;
        if(boomRenderer != null)
            boomRenderer.color = Color.white;
    }

    private void Update()
    {
        curTime -= Time.deltaTime;
        Color color = boomRenderer.color;
        color.g = color.b = curTime / lifeTime;
        boomRenderer.color = color;

        if(curTime <= 0)
        {
            BoomManager.instance.MakeExplosion(transform.position);
            gameObject.SetActive(false);
        }
    }
}
