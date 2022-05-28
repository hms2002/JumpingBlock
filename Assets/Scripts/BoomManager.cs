using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomManager : MonoBehaviour
{
    public static BoomManager instance;


    const int MAX_BOOM_COUNT = 50;


    public GameObject src_BoomEffect;

    GameObject[] BoomEffects = new GameObject[50];

    void Start()
    {
        instance = this;

        for (int i = 0; i < MAX_BOOM_COUNT; i++)
        {
            BoomEffects[i] = Instantiate(src_BoomEffect);
            BoomEffects[i].SetActive(false);
        }
    }

    public GameObject MakeBoom()
    {
        for (int i = 0; i < MAX_BOOM_COUNT; i++)
        {
            if (BoomEffects[i].activeSelf == false)
            {
                BoomEffects[i].SetActive(true);
                return BoomEffects[i];
            }
            //Debug.Log(defaultBlock[i].activeSelf);
        }
        //Debug.Log("??");
        return null;
    }
}
