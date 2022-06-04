using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLock : MonoBehaviour
{
    bool selected = false;

    public Player.CharacterType characterType;

    public bool Select(CharactorSelectManager.CursorType type)
    {
        if (selected) return false;
        selected = true;

        GetComponent<SpriteRenderer>().color = Color.black;

        switch (type)
        {
            case CharactorSelectManager.CursorType.playerA:
                GameManager.instance.playerCharactorType_A = characterType;
                break;
            case CharactorSelectManager.CursorType.playerB:
                GameManager.instance.playerCharactorType_B = characterType;
                break;
        }

        return true;
    }
}
