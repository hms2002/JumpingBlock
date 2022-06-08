using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;


    const int MAX_BLOCK_COUNT = 50;
    const int MAX_METAL_BLOCK_COUNT = 10;

    public enum BlockType
    {
        DefaultBlock,
        MetalBlock
    }

    public GameObject src_DefaultBlock;
    BoxCollider2D defaultBlockCollider;
    public float defaultBlockSizeX;
    public float defaultBlockSizeY;


    public GameObject src_MetalBlock;
    BoxCollider2D metalBlockCollider;
    public float metalBlockSizeX;
    public float metalBlockSizeY;

    GameObject[] defaultBlock = new GameObject[50];
    GameObject[] metalBlock = new GameObject[MAX_METAL_BLOCK_COUNT];

    void Start()
    {
        instance = this;

        if(GameManager.instance.backGroundType == BackGroundType.BrokenCity)
        {
            src_DefaultBlock.transform.localScale = src_DefaultBlock.transform.localScale * 0.8f;
            src_MetalBlock.transform.localScale = src_MetalBlock.transform.localScale * 0.8f;
        }

        defaultBlockCollider = src_DefaultBlock.GetComponent<BoxCollider2D>();
        defaultBlockSizeX = defaultBlockCollider.size.x;
        defaultBlockSizeY = defaultBlockCollider.size.y;


        metalBlockCollider = src_MetalBlock.GetComponent<BoxCollider2D>();
        metalBlockSizeX = metalBlockCollider.size.x;
        metalBlockSizeY = metalBlockCollider.size.y;

        for (int i = 0; i < MAX_BLOCK_COUNT; i++)
        {
            defaultBlock[i] = Instantiate(src_DefaultBlock);
            defaultBlock[i].SetActive(false);
        }
        for (int i = 0; i < MAX_METAL_BLOCK_COUNT; i++)
        {
            metalBlock[i] = Instantiate(src_MetalBlock);
            metalBlock[i].SetActive(false);
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

    public GameObject MakeObj(BlockType block, Vector2 position)
    {
        Vector2 fromBuildPoint;
        Vector2 toBuildPoint;
        int buildLayerMask;

        switch (block)
        {
            case BlockType.DefaultBlock:

                fromBuildPoint = position + new Vector2(-defaultBlockSizeX/2f, defaultBlockSizeY/2f);
                toBuildPoint = position + new Vector2(defaultBlockSizeX/2f, -defaultBlockSizeY/2f);

                //Debug.Log("From : " + fromBuildPoint);
                //Debug.Log("To : " + toBuildPoint);
                buildLayerMask = ((1 << LayerMask.NameToLayer("Effect")) | (1 << LayerMask.NameToLayer("Item")));
                buildLayerMask = ~buildLayerMask;

                Collider2D hit = Physics2D.OverlapArea(fromBuildPoint, toBuildPoint, buildLayerMask);

                if (hit)
                {
                    Debug.Log(hit.name);
                    return null;
                }

                for (int i = 0; i < MAX_BLOCK_COUNT; i++)
                {
                    if (defaultBlock[i].activeSelf == false)
                    {
                        defaultBlock[i].transform.position = position;
                        defaultBlock[i].SetActive(true);

                        return defaultBlock[i];
                    }
                    //Debug.Log(defaultBlock[i].activeSelf);
                }

                Debug.LogError("생성할 블럭이 부족합니다!!");

                break;
            case BlockType.MetalBlock:

                fromBuildPoint = position + new Vector2(-metalBlockSizeX/2f, metalBlockSizeY/2f);
                toBuildPoint = position + new Vector2(metalBlockSizeX/2f, -metalBlockSizeY/2f);

                //Debug.Log("From : " + fromBuildPoint);
                //Debug.Log("To : " + toBuildPoint);
                buildLayerMask = ((1 << LayerMask.NameToLayer("Effect")) | (1 << LayerMask.NameToLayer("Item")));
                buildLayerMask = ~buildLayerMask;

                hit = Physics2D.OverlapArea(fromBuildPoint, toBuildPoint, buildLayerMask);

                if (hit)
                {
                    Debug.Log(hit.name);
                    return null;
                }

                for (int i = 0; i < MAX_METAL_BLOCK_COUNT; i++)
                {
                    if (metalBlock[i].activeSelf == false)
                    {
                        metalBlock[i].transform.position = position;
                        metalBlock[i].SetActive(true);

                        return metalBlock[i];
                    }
                    //Debug.Log(defaultBlock[i].activeSelf);
                }

                Debug.LogError("생성할 블럭이 부족합니다!!");
                break;
        }
        //Debug.Log("??");
        return null;
    }
}
