using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;


    const int MAX_BLOCK_COUNT = 50;

    public enum BlockType
    {
        DefaultBlock
    }

    public GameObject src_DefaultBlock;

    GameObject[] defaultBlock = new GameObject[50];

    void Start()
    {
        instance = this;

        for (int i = 0; i < MAX_BLOCK_COUNT; i++)
        {
            defaultBlock[i] = Instantiate(src_DefaultBlock);
            defaultBlock[i].SetActive(false);
        }
    }

    public GameObject MakeObj(BlockType block)
    {
        switch (block)
        {
            case BlockType.DefaultBlock:
                for (int i = 0; i < MAX_BLOCK_COUNT; i++)
                {
                    if (defaultBlock[i].activeSelf == false)
                    {
                        defaultBlock[i].SetActive(true);
                        return defaultBlock[i];
                    }
                    //Debug.Log(defaultBlock[i].activeSelf);
                }
                break;
        }
        //Debug.Log("??");
        return null;
    }
}
