using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    const int SIZE = 3;
    // �ڽ� ������Ʈ �ε��� �� �����ϱ�
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
            // ����Ű�� �޴��� �ٲٰ�, ��׶��� �̹����� �ٲ��ֱ�
            menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(false);
            selected = (SelectedMenu)(((int)selected - 1 + SIZE) % SIZE);
            menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.K))
        {
            // ����Ű�� �޴��� �ٲٰ�, ��׶��� �̹����� �ٲ��ֱ�
            menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(false);
            selected = (SelectedMenu)(((int)selected + 1 + SIZE) % SIZE);
            menues[(int)selected].transform.GetChild(BACKGROUND_IDX).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightShift))
        {
            // ����Ű�� �޴����� ��ư ����
            Button pointedBtn = menues[(int)selected].transform.GetChild(BTN_IDX).GetComponent<Button>();
            Debug.Log(pointedBtn.gameObject.name);
            pointedBtn.onClick.Invoke();
        }
    }
}