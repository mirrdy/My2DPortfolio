using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Attack
    };

    private Movement2D movement2D;
    private Rigidbody2D myRigid;
    private Animator animator;
    private Weapon weapon;

    public int maxHp = 100;
    public int currentHp;
    public int baseSpeed = 5;
    private int speed = 0;
    public float atkDamage = 1;

    public Transform[] hitPosition;
    public LayerMask hitLayer;

    public Direction direction;
    private PlayerState state;

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
        #endregion
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
        state = PlayerState.Idle;
        Debug.Log("State: co finish - Idle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"collision Enter: {collision.collider.name}");
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log($"collision Stay: {collision.collider.name}");
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
                overCollider2d = Physics2D.OverlapCircle(tmpHitPosition, 0.01f, hitLayer);
            }
            // 앞뒤방향 (x축 3개)
            else
            {
                tmpHitPosition = hitPosition[dir].position + new Vector3(0.07f * i / scaleX, 0);
                overCollider2d = Physics2D.OverlapCircle(tmpHitPosition, 0.01f, hitLayer);
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
