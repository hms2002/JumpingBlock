using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomManager : MonoBehaviour
{
    public static BoomManager instance;


    const int MAX_Explosion_COUNT = 10;
    const int MAX_BOOM_COUNT = 10;


    public GameObject src_ExplosionEffect;

    GameObject[] ExplosionEffects = new GameObject[10];


    public GameObject src_Boom;

    GameObject[] Booms = new GameObject[10];

    void Start()
    {
        instance = this;

        for (int i = 0; i < MAX_Explosion_COUNT; i++)
        {
            ExplosionEffects[i] = Instantiate(src_ExplosionEffect);
            ExplosionEffects[i].SetActive(false);
        }
        for (int i = 0; i < MAX_BOOM_COUNT; i++)
        {
            Booms[i] = Instantiate(src_Boom);
            Booms[i].SetActive(false);
        }
    }

    public GameObject MakeBoom(Vector2 pos)
    {
        for (int i = 0; i < MAX_BOOM_COUNT; i++)
        {
            if (Booms[i].activeSelf == false)
            {
                Booms[i].SetActive(true);
                Booms[i].transform.position = pos;
                return Booms[i];
            }
        }
        return null;
    }

    public GameObject MakeExplosion(Vector2 pos)
    {
        for (int i = 0; i < MAX_Explosion_COUNT; i++)
        {
            if (ExplosionEffects[i].activeSelf == false)
            {
                ExplosionEffects[i].SetActive(true);
                ExplosionEffects[i].transform.position = pos;
                ExplosionEffects[i].GetComponent<BoomExplosionEffect>().Explode();
                return ExplosionEffects[i];
            }
            //Debug.Log(defaultBlock[i].activeSelf);
        }
        //Debug.Log("??");
        return null;
    }

    //public GameObject MakeExplosion(Vector2 pos)
    //{
    //    for (int i = 0; i < MAX_Explosion_COUNT; i++)
    //    {
    //        if (ExplosionEffects[i].activeSelf == false)
    //        {
    //            ExplosionEffects[i].SetActive(true);
    //            ExplosionEffects[i].transform.position = pos;
    //            return ExplosionEffects[i];
    //        }
    //        //Debug.Log(defaultBlock[i].activeSelf);
    //    }
    //    //Debug.Log("??");
    //    return null;
    //}
}
