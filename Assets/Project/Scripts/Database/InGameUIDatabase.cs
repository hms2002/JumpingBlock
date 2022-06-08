using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InGameUIDatabase : MonoBehaviour
{
    private static InGameUIDatabase _instance;
    public static InGameUIDatabase instance { get { return _instance; } }

    public Text timeText;
    public GameObject gameOverUI;
    public Text winnerText;

    public Image[] playerCharactorImage = new Image[2];

    public Image inventoryA_1;
    public Image inventoryA_2;
    public Image inventoryA_3;
    public Image inventoryB_1;
    public Image inventoryB_2;
    public Image inventoryB_3;

    public Image skillA;
    public Image skillB;

    public Sprite boySkillSprite;
    public Sprite girlSkillSprite;
    public Sprite girlTwoSkillSprite;
    public Sprite boyTwoSkillSprite;

    public SpriteRenderer backGround;
    public Sprite[] backGroundImg;

    public Sprite[] charactorImage;

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

//[CanEditMultipleObjects]
//[CustomEditor(typeof(InGameUIDatabase))]
//class InGameUIDatabaseEditor : UnityEditor.Editor
//{
//    InGameUIDatabase inGameUIDatabase;

//    private void OnEnable()
//    {
//        inGameUIDatabase = target as InGameUIDatabase;
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        base.OnInspectorGUI();
//        EditorGUILayout.LabelField("°ø°Ý·Â", inGameUIDatabase.timeText.ToString());
//    }
//}