using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownControl : MonoBehaviour
{
    Text text;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        count = 3;
        text.text = count.ToString();
    }

    public void countDown()
    {
        count--;
        if (count <= 0)
            Destroy(transform.parent.gameObject);
        else
            text.text = count.ToString();
            

    }
}
