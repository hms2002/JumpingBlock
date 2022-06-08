using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // 씬 관리
    public enum BuildIdx
    {
        MainMenu,
        InGameScene,
        SelectCharacterScene,
        SelectBackGroundScene
    }

    private static BuildIdx _sceneNum;
    public static BuildIdx sceneNum { get { return _sceneNum; } }


    // 싱글톤
    private static GameManager _instance;
    public static GameManager instance { get { return _instance; } }


    FadeManager fadeManager;

    // 시간 흐름
    Text timeText;
    float time = 60;
    float boomDropCoolTime;
    float boomDropCurTime;

    // 아이템 스폰 시간
    float itemSpawnCoolTime = 2.5f;
    float itemSpawnCurTime = 2.5f;

    // 게임 종료 확인
    GameObject gameOverUI;
    Text winnerText;

    public bool isGameStart;
    bool isGameOver;
    bool isFirstTime;


    public int TimeScale = 1;
    private void Awake()
    {
        // 싱글톤 처리
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
        // 씬 이동해도 유지
        DontDestroyOnLoad(gameObject);

        // 씬 이동할 때 발동할 함수 델리게이트에 저장
        SceneManager.sceneLoaded += OnSceneLoaded;

        boomDropCoolTime = Random.Range(0.5f, 1f);
        boomDropCurTime = boomDropCoolTime;
    }

    private Player.CharacterType _playerCharactorType_A = Player.CharacterType.EndIdx;
    public Player.CharacterType playerCharactorType_A { 
        get{ return _playerCharactorType_A; } 
        set 
        { 
            _playerCharactorType_A = value;
            if (playerCharactorType_B != Player.CharacterType.EndIdx) SceneManager.LoadScene((int)BuildIdx.SelectBackGroundScene);
        } 
    }
    private Player.CharacterType _playerCharactorType_B = Player.CharacterType.EndIdx;
    public Player.CharacterType playerCharactorType_B
    {
        get { return _playerCharactorType_B; }
        set
        {
            _playerCharactorType_B = value;
            if (playerCharactorType_A != Player.CharacterType.EndIdx) SceneManager.LoadScene((int)BuildIdx.SelectBackGroundScene);
        }
    }

    private BackGroundType _backGroundType = BackGroundType.BrokenCity;
    public BackGroundType backGroundType
    {
        get { return _backGroundType; }
        set
        {
            _backGroundType = value;
            SceneManager.LoadScene((int)BuildIdx.InGameScene);
        }
    }

    Button StartButton;
    Button OptionButton;
    Button ExitButton;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("씬 로드 : " + scene.buildIndex + " 로드 모드 : " + mode);
        _sceneNum = (BuildIdx)scene.buildIndex;
        switch (sceneNum)
        {
            case BuildIdx.MainMenu:
                _playerCharactorType_A = Player.CharacterType.EndIdx;
                _playerCharactorType_B = Player.CharacterType.EndIdx;
                _backGroundType = BackGroundType.BrokenCity;
                fadeManager = FindObjectOfType<FadeManager>();
                StartButton = ButtonData.instance.startButton;
                OptionButton = ButtonData.instance.optionButton;
                ExitButton = ButtonData.instance.ExitButton;
                StartButton.onClick.AddListener(onClickStartButton);
                break;
            case BuildIdx.InGameScene:
                isGameOver = false;
                isFirstTime = true;
                isGameStart = false;
                timeText = InGameUIDatabase.instance.timeText;
                gameOverUI = InGameUIDatabase.instance.gameOverUI;
                winnerText = InGameUIDatabase.instance.winnerText;

                if (playerCharactorType_A == Player.CharacterType.EndIdx)
                    playerCharactorType_A = Player.CharacterType.Boy;
                if (playerCharactorType_B == Player.CharacterType.EndIdx)
                    playerCharactorType_B = Player.CharacterType.Girl;

                Player.playerA.characterType = playerCharactorType_A;
                Player.playerB.characterType = playerCharactorType_B;

                InGameUIDatabase.instance.backGround.sprite = InGameUIDatabase.instance.backGroundImg[(int)backGroundType];
                break;
            case BuildIdx.SelectCharacterScene:
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
                        if(!isFirstTime)
                        {
                            boomDropCurTime -= Time.deltaTime;
                            if (boomDropCurTime <= 0)
                            {
                                GameObject boom = BoomManager.instance.MakeExplosion(new Vector2(Random.Range(-7, 7), Random.Range(-4, 4)));
                                //boom.transform.position = new Vector2(Random.Range(-7, 7), Random.Range(-4, 4));
                                //boom.GetComponent<BoomExplosionEffect>().Explode();
                                boomDropCoolTime = Random.Range(0.5f, 1f);
                                boomDropCurTime = boomDropCoolTime;
                            }
                        }
                        if(time <= 0)
                        {
                            if(isFirstTime)
                            {
                                SoundManager.instance.PlayFaster();
                                isFirstTime = false;
                                time = 30;
                            }
                            else
                            {
                                GameOver();
                            }
                        }

                        if(backGroundType == BackGroundType.BrokenCity || backGroundType == BackGroundType.BrokenPark)
                        {
                            itemSpawnCurTime -= Time.deltaTime;
                            if(itemSpawnCurTime <= 0)
                            {
                                itemSpawnCurTime = itemSpawnCoolTime;
                               ItemDatabase.instance.DropItem(Random.Range(0, 2), new Vector3(Random.Range(-7, 8), Random.Range(-3, 4)));
                            }
                        }
                    }
                }
                else
                {
                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        SceneManager.LoadScene((int)BuildIdx.MainMenu);
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
       
        SceneManager.LoadScene((int)BuildIdx.SelectCharacterScene);
    }

    public void onClickOptionButton()
    {

    }

    public void onClickExitGameButton()
    {

    }

    #endregion MainMenu

}
