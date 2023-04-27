using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Object
{
    public Transform target;
    public Movement2D movement2D;
    public BoxCollider2D boxCollider;
    public Rigidbody2D myRigid;

    IEnumerator findTarget;

    private Animator animator;
    public float speed;
    public float findRadius;
    public int damage;

    private void OnEnable()
    {
        TryGetComponent(out movement2D);
        TryGetComponent(out boxCollider);
        TryGetComponent(out myRigid);
        TryGetComponent(out animator);
        findTarget = FindTarget_co();

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

        myRigid.velocity = (target.position - transform.position).normalized * speed;

        if(Vector3.Distance(target.position, transform.position) <= 1)
        {
            myRigid.velocity = Vector3.zero;
            if (aniState.IsName("Idle") && Vector3.Distance(target.position, transform.position) <= 1.2)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    private IEnumerator FindTarget_co()
    {
        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
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

    // Attack Animation 마지막프레임 이벤트
    private void Attack()
    {
        if(target == null)
        {
            return;
        }
        target.gameObject.TryGetComponent(out PlayerControll player);
        player.TakeDamage(damage);
    }

    public override void DestroyThisObject()
    {
        StopCoroutine(findTarget);
        target = null;

        animator.SetTrigger("Die");
    }
}
