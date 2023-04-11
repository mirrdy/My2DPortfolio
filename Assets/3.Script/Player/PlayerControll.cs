using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public enum Direction
    {
        Front,
        Back,
        Side
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

    public int baseSpeed = 5;
    private int speed = 0;

    private Direction direction;
    private PlayerState state;

    private void Awake()
    {
        TryGetComponent(out movement2D);
        TryGetComponent(out myRigid);
        TryGetComponent(out animator);
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
            if (Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.RightShift))
            {
                speed = baseSpeed;
            }

            float scaleX = Mathf.Abs(gameObject.transform.localScale.x);
            float scaleY = gameObject.transform.localScale.y;

            if (y == -1 && direction != Direction.Front)
            {
                animator.SetTrigger("Front");
                direction = Direction.Front;
            }
            else if (y == 1 && direction != Direction.Back)
            {
                animator.SetTrigger("Back");
                direction = Direction.Back;
            }
            else if (x < 0 && y == 0 && direction != Direction.Side)
            {
                gameObject.transform.localScale = new Vector3(-scaleX, scaleY, 1);

                animator.SetTrigger("Side");
                direction = Direction.Side;
            }
            else if (x > 0 && y == 0 && direction != Direction.Side)
            {
                gameObject.transform.localScale = new Vector3(scaleX, scaleY, 1);

                animator.SetTrigger("Side");
                direction = Direction.Side;
            }

            state = PlayerState.Move;
            Vector2 dirVector = new Vector2(x, y).normalized * speed * Time.deltaTime;
            myRigid.MovePosition((Vector2)transform.position + dirVector);
        }
        if(x==0 && y==0 && state != PlayerState.Attack)
        {
            state = PlayerState.Idle;
            if(direction == Direction.Front)
            {
                animator.Play("PlayerIdleFront");
            }
            else if(direction == Direction.Back)
            {
                animator.Play("PlayerIdleBack");
            }
            else if(direction == Direction.Side)
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
        AnimatorStateInfo aniState = animator.GetCurrentAnimatorStateInfo(0);

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
            case Direction.Side:
                {
                    animator.Play("PlayerAttackSide");
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackSide") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
                    break;
                }
        }
        state = PlayerState.Idle;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
    }
}
