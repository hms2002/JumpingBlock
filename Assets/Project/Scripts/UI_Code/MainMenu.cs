using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    const int SIZE = 3;
    // 자식 오브젝트 인덱스 값 저장하기
    const int BTN_IDX = 0;
    const int BACKGROUND_IDX = 1;
    const int MENU_IMAGE_IDX = 2;


    enum SelectedMenu
    {
        Start,
        Option,
        Exit
    }
    SelectedMenu selected;

    public GameObject menu;
    public GameObject[] menues;
    Image[] menuImages = new Image[3];
    GameObject[] menuBackGroundImages = new GameObject[3];
    GameObject backGround;

    bool menuAwake = false;
    public OpenningJumpImage[] openningJumpImages;

    public Image title;
    public Image white;
    public RectTransform high;

    private void Start()
    {
        selected = SelectedMenu.Start;
        menuAwake = false;

        StartCoroutine("ShowOpenning");

        for(int i = 0; i < 3; i++)
        {
            menuImages[i] = menues[i].transform.GetChild(MENU_IMAGE_IDX).GetComponent<Image>();
            menuBackGroundImages[i] = menues[i].transform.GetChild(BACKGROUND_IDX).gameObject;
        }
    }

    private void Update()
    {
        if(!menuAwake)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.I))
        {
            //// 가리키는 메뉴바 바꾸고, 백그라운드 이미지도 바꿔주기
            //menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(false);
            selected = (SelectedMenu)(((int)selected - 1 + SIZE) % SIZE);
            UpdateMenuView();
            //menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.K))
        {
            //// 가리키는 메뉴바 바꾸고, 백그라운드 이미지도 바꿔주기
            //menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(false);
            selected = (SelectedMenu)(((int)selected + 1 + SIZE) % SIZE);
            UpdateMenuView();
            //menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightShift))
        {
            // 가리키는 메뉴바의 버튼 실행
            Button pointedBtn = menues[(int)selected].transform.GetChild(BTN_IDX).GetComponent<Button>();
            Debug.Log(pointedBtn.gameObject.name);
            pointedBtn.onClick.Invoke();
            // 사운드
            SoundManager.instance.PlaySelectSound();
        }
    }

    void UpdateMenuView()
    {
        foreach(Image image in menuImages)
        {
            image.color = Color.black;
        }
        foreach(GameObject backGround in menuBackGroundImages)
        {
            backGround.SetActive(false);
        }
        menuImages[(int)selected].color = Color.white;
        menuBackGroundImages[(int)selected].SetActive(true);
        //menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(false);
    }

    IEnumerator ShowOpenning()
    {
        foreach(OpenningJumpImage openningJump in openningJumpImages)
        {
            openningJump.JumpIn();
            yield return new WaitForSeconds(0.7f);
        }

        RectTransform rect = title.GetComponent<RectTransform>();
        Vector2 vector2 = rect.position;
        Color color;

        for (float i = 0; i <= 1; i += 0.02f)
        {
            rect.position = Vector2.Lerp(vector2, high.position, i);
            
            color = title.color;
            color.a = i;
            title.color = color;

            color = white.color;
            color.a = i;
            white.color = color;

            yield return new WaitForSeconds(0.01f);
        }

        for (float i = 0; i <= 1; i += 0.1f)
        {
            color = white.color;
            color.a = 1 - i;
            white.color = color;

            yield return new WaitForSeconds(0.01f);
        }

        menu.SetActive(true);
        menuAwake = true;
    }
}