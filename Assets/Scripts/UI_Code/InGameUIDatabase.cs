using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIDatabase : MonoBehaviour
{
    private static InGameUIDatabase _instance;
    public static InGameUIDatabase instance { get { return _instance; } }

    public Text timeText;
    public GameObject gameOverUI;
    public Text winnerText;

    public Image inventoryA;
    public Image inventoryB;

    public Image skillA;
    public Image skillB;

    public Sprite boySkillSprite;
    public Sprite girlSkillSprite;

    private void Awake()
    {
        if(instance == null)
        {
            _instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
}