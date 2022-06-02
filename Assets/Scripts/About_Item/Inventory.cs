using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    Item item;
    Player.PlayerType playerType;
    Sprite defaultImg;
    Image itemImage;

    private void Start()
    {
        playerType = GetComponent<Player>().playerType;
        switch(playerType)
        {
            case Player.PlayerType.PlayerA:
                itemImage = InGameUIDatabase.instance.inventoryA;
                break;
            case Player.PlayerType.PlayerB:
                itemImage = InGameUIDatabase.instance.inventoryB;
                break;
        }
        defaultImg = itemImage.sprite;
    }

    bool AddItem(Item _item)
    {
        item = _item;
        itemImage.sprite = item.itemImages;

        return true;
    }
    
    void DeleteItem()
    {
        item = null;
    }

    const int addBlockCount = 10;
    public void useItem()
    {
        if (item == null) return;
        switch(item.itemType)
        {
            case ItemType.Boom:
                BoomManager.instance.MakeBoom(transform.position);
                DeleteItem();
                break;
            case ItemType.AddBlock:
                GetComponent<Player>().AddBlock(addBlockCount);
                DeleteItem();
                break;
        }
        itemImage.sprite = defaultImg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item") && item == null)
        {
            FieldItem item = collision.GetComponent<FieldItem>();
            if (AddItem(item.GetItem()))
                item.DestroyItem();
        }
    }
}
