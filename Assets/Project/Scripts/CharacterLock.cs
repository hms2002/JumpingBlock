using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterLock : MonoBehaviour
{
    bool selected = false;

    public Player.CharacterType characterType;

    Image image;
    public Image smallCharactorImage;
    public Sprite[] jumpSpriteSet;
    public Sprite[] idleSpriteSet;

    public RectTransform pointTransform;
    public RectTransform objTransform;
    public Text nameText;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Pointed()
    {
        image.color = Color.white;
    }
    public void UnPointed()
    {
        image.color = Color.black;
    }

    public bool Select(CharactorSelectManager.CursorType type)
    {
        if (selected) return false;
        selected = true;

        switch(type)
        {
            case CharactorSelectManager.CursorType.playerA:
                nameText.color = Color.blue;
                break;
            case CharactorSelectManager.CursorType.playerB:
                nameText.color = Color.red;
                break;
        }

        StartCoroutine("ISelectAction", type);

        return true;
    }

    IEnumerator ISelectAction(CharactorSelectManager.CursorType type)
    {
        for(int i = 0; i < 4; i++)
        {
            image.color = Color.white;
            yield return new WaitForSeconds(0.05f);
            image.color = Color.black;
            yield return new WaitForSeconds(0.05f);
        }

        for (float i = 0; i <= 1; i += 0.01f)
        {//objTransform.transform.position
            Vector3 pos = Vector3.Lerp(objTransform.transform.position, pointTransform.position, i);
            objTransform.transform.position = new Vector3(objTransform.transform.position.x, pos.y, pos.z);
            yield return new WaitForSeconds(0.01f);
        }


        foreach(Sprite sprite in jumpSpriteSet)
        {
            yield return new WaitForSeconds(0.1f);
            smallCharactorImage.sprite = sprite;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.05f);
        switch(type)
        {
            case CharactorSelectManager.CursorType.playerA:
                GameManager.instance.playerCharactorType_A = characterType;
                break;
            case CharactorSelectManager.CursorType.playerB:
                GameManager.instance.playerCharactorType_B = characterType;
                break;
        }

        while(true)
        {
            foreach (Sprite sprite in idleSpriteSet)
            {
                yield return new WaitForSeconds(0.05f);
                smallCharactorImage.sprite = sprite;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
