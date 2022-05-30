using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomExplosionEffect : MonoBehaviour
{
    Animator anim;
    float lifeTime = 0.8f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        transform.position = new Vector2(Random.Range(-7, 7), Random.Range(-4, 4));
        lifeTime = 0.3f;
        anim.SetTrigger("Boom"); 
        Collider2D[] colloders = Physics2D.OverlapCircleAll(transform.position, 2);
        foreach (Collider2D collider in colloders)
        {
            if (collider.CompareTag("Block"))
            {
                Debug.Log(collider.gameObject.name);
                collider.gameObject.SetActive(false);
            }
        }
    }
    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if(lifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
