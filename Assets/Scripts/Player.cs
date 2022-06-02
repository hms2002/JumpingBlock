using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum CharacterType
    {
        Boy,
        Girl
    }
    public CharacterType characterType;

    // #.플레이어 선택 enum
    public enum PlayerType
    {
        PlayerA,
        PlayerB
    }
    public PlayerType playerType;

    // 승자 실글톤
    public static Player playerA = null;
    public static Player playerB = null;
    public static Player winner = null;

    // #.선택에 따라 조작키 바꾸는 변수
    string moveKeyName;
    KeyCode jumpKeyCode;
    KeyCode buildationKeyCode;
    KeyCode useItemKeyCode;
    KeyCode useSkillKeyCode;


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

    public void PlayerConfine(float time)
    {
        StopCoroutine("IPlayerConFine");
        StartCoroutine("IPlayerConFine", time);
    }

    IEnumerator IPlayerConFine(float time)
    {
        canMove = false;
        canJump = false;
        canBuild = false;
        yield return new WaitForSeconds(time);
        canMove = true;
        canJump = true;
        canBuild = true;
    }

    bool flipNormal = true;

// 플레이어 이동 영역
    #region PlayerMove

    // #.플레이어 이동 관련 변수
    Rigidbody2D rigid;

    public int moveSpeed = 8;

    float xAxis;

    // #. 플레이어 이동 관련 함수
    
    public void move()
    {
        if (canMove == false)
        {
            return;
        }

        xAxis = Input.GetAxisRaw(moveKeyName);
        if(xAxis != 0)
        {
            if (xAxis > 0)
                sprite.flipX = flipNormal;
            else
                sprite.flipX = !flipNormal;
            
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
    //GameObject block;

    // 플레이어 크기 가져올 boxCollider
    BoxCollider2D playerBoxColider;

    // 설치 할 블럭의 크기 조사
    Vector2 fromBuildPoint;
    Vector2 toBuildPoint;
    // 설치할 수 있는 블럭 개수
    int blockCount;

    public void AddBlock(int count)
    {
        blockCount += count;
        blockCountText.text = "남은 블럭 갯수 : " + blockCount;
    }

    float boxLength = 1f;
    float boxHeight = 1f;

    // 블럭 갯수 출력 텍스트
    public Text blockCountText;

    // 레이어마스크
    int buildLayerMask;// = (-1) - (1 << LayerMask.NameToLayer("Effect"));

    // 블럭 설치 관련 함수들
    public bool checkCanBuild()
    {
        if (canBuild == false || blockCount <= 0) return false;
        // 설치 전 조사 할 영역 설정
        // + 추후 블럭 크기에 따라 달리지도록 수정해야 함
        float playerPosX = transform.position.x + playerBoxColider.offset.x;
        float playerPosY = transform.position.y + playerBoxColider.offset.y;
        // 플레이어의 콜라이더 바닥 y좌표값를 저장
        float playerUnderPosY = playerPosY - playerBoxColider.size.y / 2f;


        boxLength = BlockManager.instance.defaultBlockSizeX;
        boxHeight = BlockManager.instance.defaultBlockSizeY;

        fromBuildPoint = new Vector2(playerPosX - boxLength / 2f, playerUnderPosY - 0.1f);
        toBuildPoint = new Vector2(playerPosX + boxLength / 2f, playerUnderPosY - boxHeight);

        //Debug.Log("From : " + fromBuildPoint);
        //Debug.Log("To : " + toBuildPoint);

        Collider2D hit = Physics2D.OverlapArea(fromBuildPoint, toBuildPoint, buildLayerMask);

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

            GameObject block = BlockManager.instance.MakeObj(BlockManager.BlockType.DefaultBlock);
            block.transform.position = position;
            block.transform.rotation = transform.rotation;
            //Instantiate(block, position, transform.rotation);
            
            blockCount--;
            blockCountText.text = "남은 블럭 갯수 : " + blockCount;
        }
    }

    #endregion

    #region DIE
    public bool isDie = false;
    public void Die()
    {
        isDie = true;

        switch(playerType)
        {
            case PlayerType.PlayerA:
                winner = playerB;
                break;

            case PlayerType.PlayerB:
                winner = playerA;
                break;
        }

        GameManager.instance.GameOver();
    }
    #endregion

    #region UseItem
    Inventory inventory;
    void useItem()
    {
        if(Input.GetKeyDown(useItemKeyCode))
            inventory.useItem();
    }
    #endregion

    #region UseSkill
    delegate void Skill();
    Skill playerSkill;

    float skillCoolTime;
    float skillCurTime;

    const float BOY_SKILL_COOL_TIME = 0.5f;
    const float GIRL_SKILL_COOL_TIME = 1f;

    Image skillImg;
    Sprite skillSprite;

    void useSkill()
    {
        skillCurTime -= Time.deltaTime;
        skillImg.fillAmount = (skillCoolTime - skillCurTime) / skillCoolTime;
        if(Input.GetKeyDown(useSkillKeyCode) && skillCurTime <= 0)
        {
            playerSkill();
            skillCurTime = skillCoolTime;
        }
    }

    void BoySkill()
    {
        GameObject skillEffect = Instantiate(SkillEffectDatabase.instance.boySkillEffect, transform.position, Quaternion.identity);
        Vector3 vector3 = skillEffect.transform.localScale;
        vector3.x = sprite.flipX ? 1 : -1;
        skillEffect.transform.localScale = vector3;
    }
    void GirlSkill()
    {
        GameObject skillEffect = Instantiate(SkillEffectDatabase.instance.girlSkillEffect, transform.position, Quaternion.identity);
        Vector3 vector3 = skillEffect.transform.localScale;
        vector3.x = sprite.flipX ? -1 : 1;
        skillEffect.transform.localScale = vector3;
    }
    #endregion

    void Start()
    {
        buildLayerMask = (-1) - (1 << LayerMask.NameToLayer("Effect"));

        sprite = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        playerBoxColider = GetComponent<BoxCollider2D>();

        //block = Resources.Load<GameObject>("Prefabs/BlockDefault");

        rigid = GetComponent<Rigidbody2D>();

        StartCoroutine("IStartSetting");

        // 캐릭터 타입에 맞게 좌우 반전 및 설정 변경
        switch (characterType)
        {
            case CharacterType.Boy:

                playerSkill = BoySkill;
                skillCoolTime = skillCurTime = BOY_SKILL_COOL_TIME;

                skillSprite = InGameUIDatabase.instance.boySkillSprite;
                break;
            case CharacterType.Girl:
                flipNormal = false;
                playerSkill = GirlSkill;
                skillCoolTime = skillCurTime = GIRL_SKILL_COOL_TIME;

                skillSprite = InGameUIDatabase.instance.girlSkillSprite;
                break;
        }

        // 플레어어 타입에 맞게 키 설정 해주기
        switch (playerType)
        {
            case PlayerType.PlayerA:
                playerA = this;
                moveKeyName = "HorizontalA";
                jumpKeyCode = KeyCode.W;
                buildationKeyCode = KeyCode.Space;
                useItemKeyCode = KeyCode.R;
                useSkillKeyCode = KeyCode.E;

                skillImg = InGameUIDatabase.instance.skillA;
                skillImg.sprite = skillSprite;
                break;
            case PlayerType.PlayerB:
                playerB = this;
                moveKeyName = "HorizontalB";
                jumpKeyCode = KeyCode.I;
                buildationKeyCode = KeyCode.RightShift;
                useItemKeyCode = KeyCode.P;
                useSkillKeyCode = KeyCode.O;

                skillImg = InGameUIDatabase.instance.skillB;
                skillImg.sprite = skillSprite;
                break;
        }

        // 인벤토리 연결
        inventory = GetComponent<Inventory>();

        // 블럭 개수 설정
        blockCount = Random.Range(30, 61);

        blockCountText.text = "남은 블럭 갯수 : " + blockCount;
    }

    void Update()
    {
        move();
        jump();
        build();
        useItem();
        useSkill();
    }
}