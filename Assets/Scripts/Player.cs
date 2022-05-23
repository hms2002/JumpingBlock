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

    // 범용적으로 쓰이는 애니메이터, 스프라이트 변수 선언
    Animator anim;
    SpriteRenderer sprite;


// 플레이어 구속 영역
    bool canMove;
    bool canJump;
    bool canBuild;

    IEnumerator IStartSetting()
    {
        canMove = false;
        canJump = false;
        canBuild = false;
        yield return new WaitForSeconds(3);
        canMove = true;
        canJump = true;
        canBuild = true;
    }

// 플레이어 이동 영역
    #region PlayerMove

    // #.플레이어 이동 관련 변수
    Rigidbody2D rigid;

    public int moveSpeed = 8;

    float xAxis;

    // #. 플레이어 이동 관련 함수
    
    public void move()
    {
        if (canMove == false) return;

        xAxis = Input.GetAxisRaw(moveKeyName);
        if(xAxis != 0)
        {
            if (xAxis > 0)
                sprite.flipX = true;
            else
                sprite.flipX = false;
            
            anim.SetBool("isHorizontalZero", false);
        }
        else
        {
            anim.SetBool("isHorizontalZero", true);
        }

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
    float playerPosX;
    float playerPosY;
    public bool checkCanJump()
    {
        if (canJump == false) return false;
        // 점프 전 조사 할 영역 설정
        playerPosX = transform.position.x + playerBoxColider.offset.x;
        playerPosY = transform.position.y + playerBoxColider.offset.y;
        // 플레이어의 콜라이더 바닥 y좌표값를 저장
        float playerUnderPosY = playerPosY - playerBoxColider.size.y / 2f;

        fromJumpPoint = new Vector2(playerPosX - playerBoxColider.size.x / 2f, playerUnderPosY);
        toJumpPoint = new Vector2(playerPosX + playerBoxColider.size.x / 2f, playerUnderPosY - 0.1f);
        // raycast2D를 통한 바닥 검사
        Collider2D hit = Physics2D.OverlapArea(fromJumpPoint, toJumpPoint, mask);


        if (hit)
        {
            anim.SetBool("isGrounded", true);
            return true;
        }
        else
        {
            anim.SetBool("isGrounded", false);
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(new Vector3(playerPosX, playerPosY - (playerBoxColider.size.y / 2) - (0.05f)), new Vector3(playerBoxColider.size.x, 0.1f));
    }
    public void jump()
    {
        if (checkCanJump() && Input.GetKeyDown(jumpKeyCode))
        {

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

    // 플레이어 크기 가져올 boxCollider
    BoxCollider2D playerBoxColider;

    // 설치 할 블럭의 크기 조사
    Vector2 fromBuildPoint;
    Vector2 toBuildPoint;
    // 설치할 수 있는 블럭 개수
    int blockCount;

    float boxLength = 1f;
    float boxHeight = 1f;

    // 블럭 설치 관련 함수들
    public bool checkCanBuild()
    {
        if (canBuild == false) return false;
        // 설치 전 조사 할 영역 설정
        // + 추후 블럭 크기에 따라 달리지도록 수정해야 함
        float playerPosX = transform.position.x + playerBoxColider.offset.x;
        float playerPosY = transform.position.y + playerBoxColider.offset.y;
        // 플레이어의 콜라이더 바닥 y좌표값를 저장
        float playerUnderPosY = playerPosY - playerBoxColider.size.y / 2f;


        fromBuildPoint = new Vector2(playerPosX - boxLength / 2f, playerUnderPosY - 0.1f);
        toBuildPoint = new Vector2(playerPosX + boxLength / 2f, playerUnderPosY - boxHeight);

        Debug.Log("From : " + fromBuildPoint);
        Debug.Log("To : " + toBuildPoint);

        Collider2D hit = Physics2D.OverlapArea(fromBuildPoint, toBuildPoint);

        if (hit)
        {
            Debug.Log(hit.name);
            return false;
        }
        else return true;
    }

    public void build()
    {
        if (Input.GetKeyDown(buildationKeyCode))
        {
            if (!checkCanBuild())
            {
                Debug.Log("불가능!");
                return;
            }
            Vector2 position = new Vector2(transform.position.x, transform.position.y - 1);
            Instantiate(block, position, transform.rotation);
        }
    }

    #endregion

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        playerBoxColider = GetComponent<BoxCollider2D>();

        block = Resources.Load<GameObject>("Prefabs/BlockDefault");

        rigid = GetComponent<Rigidbody2D>();

        StartCoroutine("IStartSetting");

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
}