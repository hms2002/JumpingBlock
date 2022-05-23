using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // #.�÷��̾� ���� enum
    public enum PlayerType
    {
        PlayerA,
        PlayerB
    }
    public PlayerType playerType;

    // #.���ÿ� ���� ����Ű �ٲٴ� ����
    string moveKeyName;
    KeyCode jumpKeyCode;
    KeyCode buildationKeyCode;

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

// �÷��̾� �̵� ����
    #region PlayerMove

    // #.�÷��̾� �̵� ���� ����
    Rigidbody2D rigid;

    public int moveSpeed = 8;

    float xAxis;

    // #. �÷��̾� �̵� ���� �Լ�
    
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
    GameObject block;

    // �÷��̾� ũ�� ������ boxCollider
    BoxCollider2D playerBoxColider;

    // ��ġ �� ���� ũ�� ����
    Vector2 fromBuildPoint;
    Vector2 toBuildPoint;
    // ��ġ�� �� �ִ� �� ����
    int blockCount;

    float boxLength = 1f;
    float boxHeight = 1f;

    // �� ��ġ ���� �Լ���
    public bool checkCanBuild()
    {
        if (canBuild == false) return false;
        // ��ġ �� ���� �� ���� ����
        // + ���� �� ũ�⿡ ���� �޸������� �����ؾ� ��
        float playerPosX = transform.position.x + playerBoxColider.offset.x;
        float playerPosY = transform.position.y + playerBoxColider.offset.y;
        // �÷��̾��� �ݶ��̴� �ٴ� y��ǥ���� ����
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
                Debug.Log("�Ұ���!");
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

        // �÷���� Ÿ�Կ� �°� Ű ���� ���ֱ�
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