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
    public GameObject[] backGroundMaskObject;

    const int MIN_BACKGROUND_IDX = 0;
    const int MAX_BACKGROUND_IDX = 2;

    bool isSelected = false;

    private void Update()
    {
        if (isSelected) return;

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(transformIdx != MIN_BACKGROUND_IDX)
            {
                cursorTransform.position = backGroundSelectPos[--transformIdx].position;
            }
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (transformIdx != MAX_BACKGROUND_IDX)
            {
                cursorTransform.position = backGroundSelectPos[++transformIdx].position;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            StartCoroutine("SelectBackGroundAction");
            // »ç¿îµå
            SoundManager.instance.PlaySelectSound();
        }
    }

    IEnumerator SelectBackGroundAction()
    {
        for(int i = 0; i < 6; i++)
        {
            backGroundMaskObject[transformIdx].SetActive(true);
            yield return new WaitForSeconds(0.02f);
            backGroundMaskObject[transformIdx].SetActive(false);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.02f);
        GameManager.instance.backGroundType = (BackGroundType)transformIdx;
    }
}
