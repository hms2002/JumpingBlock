using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackGroundType
{ 
    BrokenKoreaCastle,
    BrokenCity,
    BrokenPark
}


public class BackGroundSelectManager : MonoBehaviour
{
    public Transform cursorTransform;
    int transformIdx = 0;

    public Transform[] backGroundSelectPos;

    const int MIN_BACKGROUND_IDX = 0;
    const int MAX_BACKGROUND_IDX = 2;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.J))
        {
            if(transformIdx != MIN_BACKGROUND_IDX)
            {
                cursorTransform.position = backGroundSelectPos[--transformIdx].position;
            }
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.L))
        {
            if (transformIdx != MAX_BACKGROUND_IDX)
            {
                cursorTransform.position = backGroundSelectPos[++transformIdx].position;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightShift))
        {
            GameManager.instance.backGroundType = (BackGroundType)transformIdx;
        }
    }
}
