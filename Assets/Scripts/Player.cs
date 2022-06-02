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

    // #.�÷��̾� ���� enum
    public enum PlayerType
    {
        PlayerA,
        PlayerB
    }
    public PlayerType playerType;

    // ���� �Ǳ���
    public static Player playerA = null;
    public static Player playerB = null;
    public static Player winner = null;

    // #.���ÿ� ���� ����Ű �ٲٴ� ����
    string moveKeyName;
    KeyCode jumpKeyCode;
    KeyCode buildationKeyCode;
    KeyCode useItemKeyCode;
    KeyCode useSkillKeyCode;


    // ���������� ���̴� �ִϸ�����, ��������Ʈ ���� ����
    Animator anim;
    SpriteRenderer sprite;


// �÷��̾� ���� ����
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

// �÷��̾� �̵� ����
    #region PlayerMove

    // #.�÷��̾� �̵� ���� ����
    Rigidbody2D rigid;

    public int moveSpeed = 8;

    float xAxis;

    // #. �÷��̾� �̵� ���� �Լ�
    
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

// �÷��̾� ���� ����
    #region PlayerJump
    // #.�÷��̾� ���� ���� ����
    // ���� �������� Ȯ�� ����
    public LayerMask mask;
    Vector2 fromJumpPoint;
    Vector2 toJumpPoint;

    // ���� ��� ���� ����
    public float jumpPower = 50;

    // #.�÷��̾� ���� ���� �Լ�
    float playerPosX;
    float playerPosY;
    public bool checkCanJump()
    {
        if (canJump == false) return false;
        // ���� �� ���� �� ���� ����
        playerPosX = transform.position.x + playerBoxColider.offset.x;
        playerPosY = transform.position.y + playerBoxColider.offset.y;
        // �÷��̾��� �ݶ��̴� �ٴ� y��ǥ���� ����
        float playerUnderPosY = playerPosY - playerBoxColider.size.y / 2f;

        fromJumpPoint = new Vector2(playerPosX - playerBoxColider.size.x / 2f, playerUnderPosY);
        toJumpPoint = new Vector2(playerPosX + playerBoxColider.size.x / 2f, playerUnderPosY - 0.1f);
        // raycast2D�� ���� �ٴ� �˻�
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

            // �� �� �̾ ���� ���� �� �Ȱ��� ���̷� �����ϵ��� y�ӵ� �ʱ�ȭ
            rigid.velocity = new Vector2(rigid.velocity.x, 0);

            // addForce�� �̿��� ����
            Vector2 jumpVector = new Vector2(0, jumpPower * 10);
            rigid.AddForce(jumpVector);
        }
    }
    #endregion

// �÷��̾� �� ��ġ ����
    #region PlayerBuildation
    // ��ġ �� �� �������� ������ ����
    //GameObject block;

    // �÷��̾� ũ�� ������ boxCollider
    BoxCollider2D playerBoxColider;

    // ��ġ �� ���� ũ�� ����
    Vector2 fromBuildPoint;
    Vector2 toBuildPoint;
    // ��ġ�� �� �ִ� �� ����
    int blockCount;

    public void AddBlock(int count)
    {
        blockCount += count;
        blockCountText.text = "���� �� ���� : " + blockCount;
    }

    float boxLength = 1f;
    float boxHeight = 1f;

    // �� ���� ��� �ؽ�Ʈ
    public Text blockCountText;

    // ���̾��ũ
    int buildLayerMask;// = (-1) - (1 << LayerMask.NameToLayer("Effect"));

    // �� ��ġ ���� �Լ���
    public bool checkCanBuild()
    {
        if (canBuild == false || blockCount <= 0) return false;
        // ��ġ �� ���� �� ���� ����
        // + ���� �� ũ�⿡ ���� �޸������� �����ؾ� ��
        float playerPosX = transform.position.x + playerBoxColider.offset.x;
        float playerPosY = transform.position.y + playerBoxColider.offset.y;
        // �÷��̾��� �ݶ��̴� �ٴ� y��ǥ���� ����
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
                Debug.Log("�Ұ���!");
                return;
            }

            Vector2 position = new Vector2(transform.position.x, transform.position.y - 1);

            GameObject block = BlockManager.instance.MakeObj(BlockManager.BlockType.DefaultBlock);
            block.transform.position = position;
            block.transform.rotation = transform.rotation;
            //Instantiate(block, position, transform.rotation);
            
            blockCount--;
            blockCountText.text = "���� �� ���� : " + blockCount;
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

        // ĳ���� Ÿ�Կ� �°� �¿� ���� �� ���� ����
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

        // �÷���� Ÿ�Կ� �°� Ű ���� ���ֱ�
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

        // �κ��丮 ����
        inventory = GetComponent<Inventory>();

        // �� ���� ����
        blockCount = Random.Range(30, 61);

        blockCountText.text = "���� �� ���� : " + blockCount;
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