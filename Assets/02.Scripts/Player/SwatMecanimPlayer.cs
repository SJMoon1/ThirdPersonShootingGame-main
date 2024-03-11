using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwatMecanimPlayer : MonoBehaviour
{
    [SerializeField] private Transform tr;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    public float moveSpeed = 5f;
    public float turnSpeed = 90f;
    private float h, v, r;

   
    void Start()
    {
        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

    }
    void Update()
    {
        MoveAndRoatate();
        Sprint();
        StopJump();
        ForwardJump();

    }

    private void ForwardJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && v > 0.1f)
        {
            animator.SetTrigger("ForwardJumpTrigger");
            rb.velocity = Vector3.up * 4f;
        }
    }

    private void StopJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && h == 0 && v == 0)
        {
            animator.SetTrigger("StopJumpTrigger");
            rb.velocity = Vector3.up * 2.5f;
        }
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 12f;
            animator.SetBool("IsSprint", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("IsSprint", false);
            moveSpeed = 5f;
        }
    }

    private void MoveAndRoatate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxisRaw("Mouse X");
        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir * Time.deltaTime * moveSpeed);
        {
            animator.SetFloat("posX", h, 0.1f, Time.deltaTime);
            animator.SetFloat("posY", v, 0.1f, Time.deltaTime);

        }
        tr.Rotate(Vector3.up * r * Time.deltaTime * turnSpeed);
    }
}
