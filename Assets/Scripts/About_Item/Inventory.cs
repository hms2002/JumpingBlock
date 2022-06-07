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

    int queueCount()
    {
        return (rear + MAX_QUEUE_IDX - front) % MAX_QUEUE_IDX;
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
    Image itemImage1;
    Image itemImage2;
    Image itemImage3;

    private void Start()
    {
        playerType = GetComponent<Player>().playerType;
        switch(playerType)
        {
            case Player.PlayerType.PlayerA:
                itemImage1 = InGameUIDatabase.instance.inventoryA_1;
                itemImage2 = InGameUIDatabase.instance.inventoryA_2;
                itemImage3 = InGameUIDatabase.instance.inventoryA_3;
                break;
            case Player.PlayerType.PlayerB:
                itemImage1 = InGameUIDatabase.instance.inventoryB_1;
                itemImage2 = InGameUIDatabase.instance.inventoryB_2;
                itemImage3 = InGameUIDatabase.instance.inventoryB_3;
                break;
        }
        defaultImg = itemImage1.sprite;
    }

    void updateImage()
    {
        switch (queueCount())
        {
            case 0:
                itemImage1.sprite = defaultImg;
                itemImage2.sprite = defaultImg;
                itemImage3.sprite = defaultImg;
                break;
            case 1:
                itemImage1.sprite = itemQueue[(front + 1) % MAX_QUEUE_IDX].itemImages;
                itemImage2.sprite = defaultImg;
                itemImage3.sprite = defaultImg;
                break;
            case 2:
                itemImage1.sprite = itemQueue[(front + 1) % MAX_QUEUE_IDX].itemImages;
                itemImage2.sprite = itemQueue[(front + 2) % MAX_QUEUE_IDX].itemImages;
                itemImage3.sprite = defaultImg;
                break;
            case 3:
                itemImage1.sprite = itemQueue[(front + 1) % MAX_QUEUE_IDX].itemImages;
                itemImage2.sprite = itemQueue[(front + 2) % MAX_QUEUE_IDX].itemImages;
                itemImage3.sprite = itemQueue[(front + 3) % MAX_QUEUE_IDX].itemImages;
                break;
        }
    }

    bool AddItem(Item _item)
    {
        pushItem(_item);

        item = itemQueue[(front + 1) % MAX_QUEUE_IDX];

        updateImage();

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

        item = itemQueue[(front + 1) % MAX_QUEUE_IDX];
        updateImage();
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
