using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // #.플레이어 선택 enum
    public enum PlayerType
    {
        PlayerA,
        PlayerB
    }
    public PlayerType playerType;

    // #.선택에 따라 조작키 바꾸는 변수
    string moveKeyName;
    KeyCode jumpKeyCode;
    KeyCode buildationKeyCode;

// 플레이어 이동 영역
    #region PlayerMove

    // #.플레이어 이동 관련 변수
    Rigidbody2D rigid;

    public int moveSpeed = 8;
    float xAxis;

    // #. 플레이어 이동 관련 함수
    public void move()
    {
        xAxis = Input.GetAxisRaw(moveKeyName);

        Vector2 vectorSpeed = new Vector2(xAxis * moveSpeed, rigid.velocity.y);

        rigid.velocity = vectorSpeed;
    }
    #endregion
// 플레이어 점프 영역
    #region PlayerJump
    // #.플레이어 점프 관련 변수
    // 점프 가능한지 확인 변수
    public LayerMask mask;
    Vector2 fromJumpPoint;
    Vector2 toJumpPoint;

    // 점프 기능 관련 변수
    public float jumpPower = 50;

    // #.플레이어 점프 관련 함수
    public bool canJump()
    {
        fromJumpPoint = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
        toJumpPoint = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.6f);
        // raycast2D를 통한 바닥 검사
        Collider2D hit = Physics2D.OverlapArea(fromJumpPoint, toJumpPoint, mask);

        if (hit)
            return true;
        else
            return false;
    }
    public void jump()
    {
        if (Input.GetKeyDown(jumpKeyCode))
        {
            if (!canJump()) return;

            // 두 번 이어서 점프 했을 때 똑같은 높이로 점프하도록 y속도 초기화
            rigid.velocity = new Vector2(rigid.velocity.x, 0);

            // addForce를 이용한 점프
            Vector2 jumpVector = new Vector2(0, jumpPower * 10);
            rigid.AddForce(jumpVector);
        }
    }
    #endregion
// 플레이어 블럭 설치 영역
    #region PlayerBuildation
    // 설치 할 블럭 프리펩을 보관할 변수
    GameObject block;

    // 블럭 설치가 가능한 영역인지 확인
    GameObject checkBox;
    // 설치 할 블럭의 크기 조사
    Vector2 fromBuildPoint;
    Vector2 toBuildPoint;
    // 설치할 수 있는 블럭 개수
    int blockCount;

    #endregion

    void Start()
    {
        block = Resources.Load<GameObject>("Prefabs/BlockDefault");

        rigid = GetComponent<Rigidbody2D>();
        
        // 플레어어 타입에 맞게 키 설정 해주기
        switch (playerType)
        {
            case PlayerType.PlayerA:
                moveKeyName = "HorizontalA";
                jumpKeyCode = KeyCode.W;
                buildationKeyCode = KeyCode.Space;
                break;
            case PlayerType.PlayerB:
                moveKeyName = "HorizontalB";
                jumpKeyCode = KeyCode.I;
                buildationKeyCode = KeyCode.RightShift;
                break;
        }
    }

    void Update()
    {
        move();
        jump();
        build();
    }

    public bool canBuild()
    {
        // 설치 전 조사 할 영역 설정
        // + 추후 블럭 크기에 따라 달리지도록 수정해야 함
        fromBuildPoint = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.55f);
        toBuildPoint = new Vector2(transform.position.x + 0.5f, transform.position.y - 1.5f);

        Collider2D hit = Physics2D.OverlapArea(fromBuildPoint, toBuildPoint);

        if (hit) return false;
        else return true;
    }

    public void build()
    {
        if(Input.GetKeyDown(buildationKeyCode))
        {
            if(!canBuild())
            {
                Debug.Log("불가능!");
                return;
            }
            Vector2 position = new Vector2(transform.position.x, transform.position.y - 1);
            Instantiate(block, position, transform.rotation);
        }
    }
}