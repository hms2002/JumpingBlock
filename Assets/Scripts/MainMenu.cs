using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    const int SIZE = 3;
    // 자식 오브젝트 인덱스 값 저장하기
    const int BTN_IDX = 0;
    const int BACKGROUND_IDX= 1;


    enum SelectedMenu
    {
        Start,
        Option,
        Exit
    }
    SelectedMenu selected;

    public GameObject[] menues;
    GameObject backGround;

    private void Start()
    {
        selected = SelectedMenu.Start;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.I))
        {
            // 가리키는 메뉴바 바꾸고, 백그라운드 이미지도 바꿔주기
            menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(false);
            selected = (SelectedMenu)(((int)selected - 1 + SIZE) % SIZE);
            menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.K))
        {
            // 가리키는 메뉴바 바꾸고, 백그라운드 이미지도 바꿔주기
            menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(false);
            selected = (SelectedMenu)(((int)selected + 1 + SIZE) % SIZE);
            menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightShift))
        {
            // 가리키는 메뉴바의 버튼 실행
            Button pointedBtn = menues[(int)selected].transform.GetChild(BTN_IDX).GetComponent<Button>();
            Debug.Log(pointedBtn.gameObject.name);
            pointedBtn.onClick.Invoke();
        }
    }
}