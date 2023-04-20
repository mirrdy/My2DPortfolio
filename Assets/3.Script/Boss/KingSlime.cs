using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : MonoBehaviour
{
    public Movement2D movement2D;
    public BoxCollider2D boxCollider;
    public Rigidbody2D myRigid;
    IEnumerator changeCollider;
    public bool isJumping = false;
    public bool isAggressive = false;
    private Animator animator;
    public float speed;

    private void Awake()
    {
        TryGetComponent(out movement2D);
        TryGetComponent(out boxCollider);
        TryGetComponent(out myRigid);
        TryGetComponent(out animator);
        changeCollider = ChangeCollider_co();
        StartCoroutine(changeCollider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ChangeCollider_co()
    {
        while (true)
        {
            animator.SetTrigger("PrepareJump");
            boxCollider.isTrigger = false;
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("PrepareJump") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

            animator.SetTrigger("Jump");
            boxCollider.isTrigger = true;
            isJumping = true;

            Vector2 destJumpPos = transform.position + new Vector3(1, 1);   
            Vector2 dirVector = new Vector2(5, 5).normalized * speed * Time.deltaTime;
            movement2D.MoveTo(dirVector);

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
            transform.position == (Vector3)destJumpPos );

            animator.SetTrigger("Ground");
            boxCollider.isTrigger = false;
            isJumping = false;

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

}
