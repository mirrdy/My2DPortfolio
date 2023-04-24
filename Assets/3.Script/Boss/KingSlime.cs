using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : Object
{
    public Transform target;
    public Movement2D movement2D;
    public BoxCollider2D boxCollider;
    public Rigidbody2D myRigid;

    IEnumerator chaseTarget;
    IEnumerator findTarget;
    public bool isJumping = false;
    public bool isAggressive = false;
    private Animator animator;
    public float speed;
    public float findRadius;
    private Vector3 destJumpPos;

    public Transform shadeTransform;

    private Vector3 jumpPosOffset;
    public int damage;


    private void Awake()
    {
        TryGetComponent(out movement2D);
        TryGetComponent(out boxCollider);
        TryGetComponent(out myRigid);
        TryGetComponent(out animator);
        findTarget = FindTarget_co();
        
        jumpPosOffset = new Vector3(0, 1.75f);
        currentHp = maxHp;
        StartCoroutine(findTarget);
    }
    
    private void FixedUpdate()
    {
        ChaseTarget();
    }

    private void ChaseTarget()
    {
        AnimatorStateInfo aniState = animator.GetCurrentAnimatorStateInfo(0);

        if (target == null)
        {
            if (aniState.IsName("Idle") == false && aniState.normalizedTime >= 1)
            {
                animator.Play("Idle");
            }
            return;
        }

        if (aniState.IsName("Idle"))
        {
            animator.SetTrigger("PrepareJump");
        }

        else if (aniState.IsName("PrepareJump") && aniState.normalizedTime >= 1)
        {
            boxCollider.enabled = false;
            animator.SetTrigger("Jump");

            myRigid.velocity = (target.position - transform.position);

            destJumpPos = target.position + jumpPosOffset;
            transform.position += jumpPosOffset;
        }

        else if (aniState.IsName("Jump") && aniState.normalizedTime >= 1 && Vector3.Distance(transform.position, destJumpPos) < 0.1f)
        {
            transform.position -= jumpPosOffset;
            animator.SetTrigger("Ground");

            myRigid.velocity = Vector2.zero;
            boxCollider.enabled = true;
        }
        else
        {

        }
    }

    private IEnumerator FindTarget_co()
    {
        while (true)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                yield return new WaitForSeconds(1);
                continue;
            }
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, findRadius);

            if (cols.Length > 0)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i].CompareTag("Player"))
                    {
                        Vector2 position = myRigid.transform.position;

                        Vector2 direction = cols[i].transform.position - (Vector3)position;
                        float distance = Vector2.Distance(cols[i].transform.position, position);

                        // 레이 쏘기
                        //Debug.DrawRay(position, direction, new Color(0, 1, 0));

                        // 오브젝트 레이어 이름으로 판별 
                        RaycastHit2D raycast = Physics2D.Raycast(position, direction, distance, LayerMask.GetMask("Brick"));
                        if (raycast.collider != null)
                        {
                            Debug.Log("거리 안에 있지만 벽에 막혀서 못찾음");
                            target = null;
                        }
                        else
                        {
                            target = cols[i].gameObject.transform;
                        }
                    }
                }
            }
            else
            {
                target = null;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.TryGetComponent(out Brick brick);

            ContactPoint2D[] colPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(colPoints);

            for (int i = 0; i < colPoints.Length; i++)
            {
                brick.TakeDamegeDot(colPoints[i].point, 10);
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out PlayerControll player);

            player.TakeDamage(damage);
        }
    }

    public override void DestroyThisObject()
    {
        StopCoroutine(findTarget);

        animator.SetTrigger("Die");
    }
}
