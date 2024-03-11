using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Player_Mecanim : MonoBehaviour
{
    [SerializeField] Transform tr;
    [SerializeField] Rigidbody rbody;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] Animator animator;
    public float moveSpeed = 5f;
    public float turnSpeed = 90f;
    private float h, v;
    public bool isRun = false;
    void Start()
    {
        tr = GetComponent<Transform>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rbody = GetComponent<Rigidbody>();
       animator = GetComponent<Animator>();
    }
    void Update()
    {
        PlayerMoveAndAnim();
        Sprint();
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 13f;
            animator.SetBool("IsSprint", true);
            isRun = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 5f;
            animator.SetBool("IsSprint", false);
            isRun = false;
        }
    }
    private void PlayerMoveAndAnim()
    {
        h = Input.GetAxis("Horizontal"); //A,D
        v = Input.GetAxis("Vertical"); //W ,S
        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        {
            animator.SetFloat("speedX",h ,0.01f,Time.deltaTime);
            animator.SetFloat("speedY", v, 0.01f, Time.deltaTime);
        }
        tr.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X")  * Time.deltaTime* turnSpeed);
    }
}
