using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSkillEffect : MonoBehaviour
{
    int power = 1000;

    private void Start()
    {
        Destroy(gameObject, 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if(player.characterType != Player.CharacterType.Girl)
            {
                player.PlayerConfine(0.3f);
                player.GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x * power, 80, 0));
            }
        }
    }
}
