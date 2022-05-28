using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    GameObject explosionEffect;
    float lifeTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
