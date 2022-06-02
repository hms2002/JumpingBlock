using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Boom,
    AddBlock
}

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public List<Item> itemDB = new List<Item>();

    public GameObject fieldItemPrefab;

    public void DropItem(int objType, Vector3 objPos)
    {
        GameObject go = Instantiate(fieldItemPrefab, objPos, Quaternion.identity);
        go.GetComponent<FieldItem>().SetItem(itemDB[objType]);
        //go.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-20, 20), Random.Range(10, 30)));
    }
}
