using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item;
    public SpriteRenderer image;

    public void SetItem(Item _item)
    {
        item.itemType = _item.itemType;
        item.name = _item.name;
        item.itemImages = _item.itemImages;
        //item.effects = _item.effects;

        image.sprite = _item.itemImages;
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
