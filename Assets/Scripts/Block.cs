using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    float lifeTime;

    bool playerOnBlock;

    Vector2 fromBlockPoint;
    Vector2 toBlockPoint;

    int layerMask;
    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Player");
        
        lifeTime = 1f;

        
        // 플레이어 있는지 확인
        playerOnBlock = false;
        
        // 플레이어 있는지 확인 할 영역
        fromBlockPoint = new Vector2(transform.position.x - 0.5f, transform.position.y + 0.5f);
        toBlockPoint = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.6f);
    
    }

    private void Update()
    {
        if(playerOnBlock)
        {
            Collider2D hit = Physics2D.OverlapArea(fromBlockPoint, toBlockPoint, layerMask);
            if (hit)
            {
                lifeTime -= Time.deltaTime;
                if (lifeTime <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                playerOnBlock = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerOnBlock = true;
    }
}