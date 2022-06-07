using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CursorInfo
{
    public CharactorSelectManager.CursorType type;
    public int posIdx = 0;
    public Transform _transform;

    public bool selected;
}

public class CharactorSelectManager : MonoBehaviour
{
    public enum CursorType
    {
        playerA,
        playerB
    }

    const int MIN_CHARACTOR_IDX = 0;
    const int MAX_CHARACTOR_IDX = 3;

    const int LEFT = 0;
    const int RIGHT = 1;

    public CursorInfo cursorA;
    public CursorInfo cursorB;

    public Transform[] cursorPos;
    public CharacterLock[] characterLock;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveCursor(cursorA, LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveCursor(cursorA, RIGHT);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SelectCharactor(cursorA);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCursor(cursorB, LEFT);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCursor(cursorB, RIGHT);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.RightShift))
        {
            SelectCharactor(cursorB);
        }
    }

    void MoveCursor(CursorInfo info, int dir)
    {
        if (info.selected) return;

        switch(dir)
        {
            case LEFT:
                if (info.posIdx == MIN_CHARACTOR_IDX)
                    return;
                else
                    info._transform.position = cursorPos[--info.posIdx].position;
                break;
            case RIGHT:
                if (info.posIdx == MAX_CHARACTOR_IDX)
                    return;
                else
                    info._transform.position = cursorPos[++info.posIdx].position;
                break;
        }
    }
    
    public void SelectCharactor(CursorInfo cursor)
    {
        if (cursor.selected) return;
        cursor.selected = characterLock[cursor.posIdx].Select(cursor.type);

        if (cursor.selected) cursor._transform.gameObject.SetActive(false);
        // »ç¿îµå
        SoundManager.instance.PlaySelectSound();
    }
}
