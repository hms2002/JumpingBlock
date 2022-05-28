using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    Item item;
    Player.PlayerType playerType;
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
    }

    bool AddItem(Item _item)
    {
        item = _item;
        itemImage.sprite = item.itemImages;

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            FieldItem item = collision.GetComponent<FieldItem>();
            if (AddItem(item.GetItem()))
                item.DestroyItem();
        }
    }
}
