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

// �÷��̾� �̵� ����
    #region PlayerMove

    // #.�÷��̾� �̵� ���� ����
    Rigidbody2D rigid;

    public int moveSpeed = 8;
    float xAxis;

    // #. �÷��̾� �̵� ���� �Լ�
    public void move()
    {
        xAxis = Input.GetAxisRaw(moveKeyName);

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
    public bool canJump()
    {
        fromJumpPoint = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
        toJumpPoint = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.6f);
        // raycast2D�� ���� �ٴ� �˻�
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

    // �� ��ġ�� ������ �������� Ȯ��
    GameObject checkBox;
    // ��ġ �� ���� ũ�� ����
    Vector2 fromBuildPoint;
    Vector2 toBuildPoint;
    // ��ġ�� �� �ִ� �� ����
    int blockCount;

    #endregion

    void Start()
    {
        block = Resources.Load<GameObject>("Prefabs/BlockDefault");

        rigid = GetComponent<Rigidbody2D>();
        
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

    public bool canBuild()
    {
        // ��ġ �� ���� �� ���� ����
        // + ���� �� ũ�⿡ ���� �޸������� �����ؾ� ��
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
                Debug.Log("�Ұ���!");
                return;
            }
            Vector2 position = new Vector2(transform.position.x, transform.position.y - 1);
            Instantiate(block, position, transform.rotation);
        }
    }
}