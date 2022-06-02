using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // �� ����
    public enum BuildIdx
    {
        MainMenu,
        InGameScene
    }

    private static BuildIdx _sceneNum;
    public static BuildIdx sceneNum { get { return _sceneNum; } }


    // �̱���
    private static GameManager _instance;
    public static GameManager instance { get { return _instance; } }


    FadeManager fadeManager;

    // �ð� �帧
    Text timeText;
    float time = 60;
    float boomDropCoolTime;
    float boolDropCurTime;

    // ������ ���� �ð�
    float itemSpawnCoolTime = 5;
    float itemSpawnCurTime = 5;

    // ���� ���� Ȯ��
    GameObject gameOverUI;
    Text winnerText;

    public bool isGameStart;
    bool isGameOver;
    bool isFirstTimeOver;


    public int TimeScale = 1;
    private void Awake()
    {
        // �̱��� ó��
        GameManager gameManager = instance;
        if(gameManager == null)
        {
            _instance = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
            return;
        }
        // �� �̵��ص� ����
        DontDestroyOnLoad(gameObject);

        // �� �̵��� �� �ߵ��� �Լ� ��������Ʈ�� ����
        SceneManager.sceneLoaded += OnSceneLoaded;

        boomDropCoolTime = Random.Range(0.5f, 1f);
        boolDropCurTime = boomDropCoolTime;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("�� �ε� : " + scene.buildIndex + " �ε� ��� : " + mode);
        _sceneNum = (BuildIdx)scene.buildIndex;
        switch (sceneNum)
        {
            case BuildIdx.InGameScene:
                isGameOver = false;
                isFirstTimeOver = true;
                isGameStart = false;
                timeText = InGameUIDatabase.instance.timeText;
                gameOverUI = InGameUIDatabase.instance.gameOverUI;
                winnerText = InGameUIDatabase.instance.winnerText;
                break;
        }
    }

    private void Start()
    {
        fadeManager = FindObjectOfType<FadeManager>();
    }

    private void Update()
    {
        switch(sceneNum)
        {
            case BuildIdx.InGameScene:
                if(!isGameOver)
                {
                    if(isGameStart)
                    {
                        time -= Time.deltaTime * TimeScale;
                        timeText.text = "Time : " + (int)time;
                        if(!isFirstTimeOver)
                        {
                            boolDropCurTime -= Time.deltaTime;
                            if (boolDropCurTime <= 0)
                            {
                                GameObject boom = BoomManager.instance.MakeExplosion(new Vector2(Random.Range(-7, 7), Random.Range(-4, 4)));
                                //boom.transform.position = new Vector2(Random.Range(-7, 7), Random.Range(-4, 4));
                                //boom.GetComponent<BoomExplosionEffect>().Explode();
                                boomDropCoolTime = Random.Range(0.5f, 1f);
                                boolDropCurTime = boomDropCoolTime;
                            }
                        }
                        if(time <= 0)
                        {
                            if(isFirstTimeOver)
                            {
                                isFirstTimeOver = false;
                                time = 30;
                            }
                            else
                            {
                                GameOver();
                            }
                        }

                        itemSpawnCurTime -= Time.deltaTime;
                        if(itemSpawnCurTime <= 0)
                        {
                            itemSpawnCurTime = itemSpawnCoolTime;
                           ItemDatabase.instance.DropItem(Random.Range(0, 2), new Vector3(Random.Range(-7, 8), Random.Range(-3, 4)));
                        }
                    }
                }
                break;
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        else
        {
            isGameOver = true;
            gameOverUI.SetActive(true);
            Player winner = Player.winner;
            if(winner == null)
            {
                winnerText.text = "No Winner!";
            }
            else
            {
                switch(winner.playerType)
                {
                    case Player.PlayerType.PlayerA:
                        winnerText.text = "PlayerA Win!";
                        break;
                    case Player.PlayerType.PlayerB:
                        winnerText.text = "PlayerB Win!";
                        break;
                }
            }
        }    
    }

    #region MainMenu
    public void onClickStartButton()
    {
        StartCoroutine("IStartGame");
    }
    IEnumerator IStartGame()
    {
        while (fadeManager.onFade == true)
        {
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);
       
        SceneManager.LoadScene((int)BuildIdx.InGameScene);
    }

    public void onClickOptionButton()
    {

    }

    public void onClickExitGameButton()
    {

    }

    #endregion MainMenu

}
