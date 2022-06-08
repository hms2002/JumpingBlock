using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string name;
    public Sprite itemImages;
    //public List<ItemEffects> effects = new List<ItemEffects>();
    public bool alreadyTouch = false;

}
