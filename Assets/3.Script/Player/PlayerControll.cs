using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControll : MonoBehaviour
{
    public enum Direction
    {
        Front,
        Back,
        Left,
        Right
    };
    public enum PlayerState
    {
        Idle,
        Move,
        Attack,
        Craft
    };
    public enum EquipPart
    {
        Weapon,
        Shield,
        NonEquipment
    }

    private Movement2D movement2D;
    private Rigidbody2D myRigid;
    private Animator animator;
    private Object targetObject;
    //private Weapon weapon;

    public int maxHp = 100;
    public int currentHp;
    public int baseSpeed;
    private int speed = 0;
    public float atkDamage = 1;
    public int def = 0;
    public Item[] equips = new Item[2];

    public Transform[] hitPosition;
    public LayerMask BrickLayer;

    public Direction direction;
    private PlayerState state;

    private WorkSpace workSpace;

    private void Awake()
    {
        TryGetComponent(out movement2D);
        TryGetComponent(out myRigid);
        TryGetComponent(out animator);

        currentHp = maxHp;
        speed = baseSpeed;
        direction = Direction.Front;
        state = PlayerState.Idle;
    }
    private void Start()
    {
        if (movement2D.moveSpeed <= 0f)
        {
            movement2D.moveSpeed = 2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        InputKey();

    }

    void InputKey()
    {
        #region 작업대
        if (state == PlayerState.Craft)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.B) && workSpace.craftUI.enabled == true))
            {
                workSpace.craftUI.CloseUI();
                state = PlayerState.Idle;
            }
            return;
        }
        #endregion
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        #region 이동
        if (state != PlayerState.Attack)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                speed = baseSpeed * 2;
            }
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                speed = baseSpeed;
            }

            float scaleX = Mathf.Abs(gameObject.transform.localScale.x);
            float scaleY = gameObject.transform.localScale.y;

            if (y == -1 && (direction != Direction.Front || state == PlayerState.Idle))
            {
                animator.SetTrigger("Front");
                direction = Direction.Front;
            }
            else if (y == 1 && (direction != Direction.Back || state == PlayerState.Idle))
            {
                animator.SetTrigger("Back");
                direction = Direction.Back;
            }
            else if (x < 0 && y == 0 && (direction != Direction.Left || state == PlayerState.Idle))
            {
                gameObject.transform.localScale = new Vector3(-scaleX, scaleY, 1);

                animator.SetTrigger("Side");
                direction = Direction.Left;
            }
            else if (x > 0 && y == 0 && (direction != Direction.Right || state == PlayerState.Idle))
            {
                gameObject.transform.localScale = new Vector3(scaleX, scaleY, 1);

                animator.SetTrigger("Side");
                direction = Direction.Right;
            }

            state = PlayerState.Move;
            Vector2 dirVector = new Vector2(x, y).normalized * speed * Time.deltaTime;
            myRigid.MovePosition((Vector2)transform.position + dirVector);
        }
        if(x==0 && y==0 && state != PlayerState.Attack)
        {
            state = PlayerState.Idle;
            if (direction == Direction.Front)
            {
                animator.Play("PlayerIdleFront");
            }
            else if(direction == Direction.Back)
            {
                animator.Play("PlayerIdleBack");
            }
            else if(direction == Direction.Left || direction == Direction.Right)
            {
                animator.Play("PlayerIdleSide");
            }
        }

        #endregion
        #region 공격
        //if (aniState.IsName("PlayerIdleFront") || aniState.IsName("PlayerIdleBack") || aniState.IsName("PlayerIdleSide"))
        if(Input.GetKey(KeyCode.Space) && state == PlayerState.Idle)
        {
            StartCoroutine(Attack_co());
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            UseCraft();
        }
        #endregion
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage - (def + sumAllEquipDef());
        Debug.Log($"플레이어 HP: {currentHp}/{maxHp}");
        if(currentHp <= 0)
        {
            Debug.Log("플레이어 사망");
            SceneManager.LoadScene("GameOver");
        }
    }

    private void UseCraft()
    {
        Collider2D overCollider2d;
        Vector2 tmpHitPosition;

        int dir = (int)direction;
        if(dir == 3)
        {
            dir = 2;
        }
        tmpHitPosition = hitPosition[dir].position;
        overCollider2d = Physics2D.OverlapCircle(tmpHitPosition, 0.01f);

        if(overCollider2d != null)
        {
            overCollider2d.TryGetComponent(out workSpace);
            if(workSpace != null)
            {
                workSpace.Open();
                state = PlayerState.Craft;
            }
        }
    }
    IEnumerator Attack_co()
    {
        state = PlayerState.Attack;

        switch (direction)
        {
            case Direction.Front:
                {
                    animator.Play("PlayerAttackFront");
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackFront") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
                    break;
                }
            case Direction.Back:
                {
                    animator.Play("PlayerAttackBack");
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackBack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
                    break;
                }
            case Direction.Left: case Direction.Right:
                {
                    animator.Play("PlayerAttackSide");
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackSide") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
                    break;
                }
        }

        Collider2D overCollider2d;
        Vector2 tmpHitPosition;

        int dir = (int)direction;
        if (dir == 3)
        {
            dir = 2;
        }
        tmpHitPosition = hitPosition[dir].position;
        overCollider2d = Physics2D.OverlapCircle(tmpHitPosition, 0.02f);

        if (overCollider2d != null)
        {
            overCollider2d.TryGetComponent(out targetObject);
            if (targetObject != null)
            {
                targetObject.TakeDamage((int)atkDamage + sumAllEquipAtk());
            }
        }


        state = PlayerState.Idle;
    }

    public bool Equip(int invenIndex)
    {
        Item equipItem = Inventory.instance.items[invenIndex];

        if(equipItem.equipPart == EquipPart.NonEquipment)
        {
            return false;
        }

        int equipPart = (int)equipItem.equipPart;
        if(equips[equipPart].itemName != "")
        {
            Inventory.instance.AddItem(equips[equipPart]);
        }
        equips[equipPart] = equipItem;
        Inventory.instance.RemoveItem(equipItem);

        return true;
    }
    public int sumAllEquipAtk()
    {
        int sumAtk = 0;
        foreach(Item equipment in equips)
        {
            if(equipment == null)
            {
                continue;
            }
            sumAtk += equipment.atk;
        }
        return sumAtk;
    }
    public int sumAllEquipDef()
    {
        int sumDef = 0;
        foreach (Item equipment in equips)
        {
            if (equipment == null)
            {
                continue;
            }
            sumDef += equipment.def;
        }
        return sumDef;
    }
    
    public void DigBrick()
    {
        int dir = ((int)direction);
        if (dir == 3)
        {
            dir = 2;
        }

        Collider2D overCollider2d;
        Vector2 tmpHitPosition;
        float scaleX = hitPosition[dir].localScale.x;
        float scaleY = hitPosition[dir].localScale.y;
        for (int i=-1; i<2; i++)
        {
            // side 방향 (y축 3개)
            if(dir == 2)
            {
                tmpHitPosition = hitPosition[dir].position + new Vector3(0, 0.07f * i / scaleY);
                overCollider2d = Physics2D.OverlapCircle(tmpHitPosition, 0.01f, BrickLayer);
            }
            // 앞뒤방향 (x축 3개)
            else
            {
                tmpHitPosition = hitPosition[dir].position + new Vector3(0.07f * i / scaleX, 0);
                overCollider2d = Physics2D.OverlapCircle(tmpHitPosition, 0.01f, BrickLayer);
            }

            if(overCollider2d != null)
            {
                overCollider2d.TryGetComponent(out Brick brick);
                if (brick != null)
                {
                    brick.TakeDamegeDot(tmpHitPosition, atkDamage);

                    break;
                }
            }
        }
    }

    
}
