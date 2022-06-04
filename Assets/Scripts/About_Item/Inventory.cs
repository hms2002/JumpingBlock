using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    const int MAX_QUEUE_IDX = 4;
    int front = 0;
    int rear = 0;
    bool isEmpty() 
    {
        if (front == rear) return true;
        else return false; 
    }
    bool isFull()
    {
        if((rear + MAX_QUEUE_IDX - front) % MAX_QUEUE_IDX == MAX_QUEUE_IDX - 1)
        {
            return true;
        }
        return false;
    }
    void pushItem(Item item)
    {
        rear = (rear + 1) % MAX_QUEUE_IDX;
        itemQueue[rear] = item;
    }

    Item item;
    Item[] itemQueue = new Item[MAX_QUEUE_IDX];


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
        pushItem(_item);

        item = itemQueue[(front + 1) % MAX_QUEUE_IDX];
        itemImage.sprite = itemQueue[(front + 1) % MAX_QUEUE_IDX].itemImages;

        return true;
    }
    Item Pop()
    {
        if (isEmpty()) return null;
        front = (front + 1) % MAX_QUEUE_IDX;
        return itemQueue[front];
    }
    
    void DeleteItem()
    {
        item = null;
        front = (front + 1) % MAX_QUEUE_IDX;
    }

    const int addBlockCount = 10;
    public void useItem()
    {
        if (isEmpty() == true) return;
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

        if (isEmpty() == true)
        {
            itemImage.sprite = defaultImg;
            return;
        }

        item = itemQueue[(front + 1) % MAX_QUEUE_IDX];
        itemImage.sprite = itemQueue[(front + 1) % MAX_QUEUE_IDX].itemImages;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item") && isFull() == false)
        {
            FieldItem item = collision.GetComponent<FieldItem>();
            if (AddItem(item.GetItem()))
                item.DestroyItem();
        }
    }
}
