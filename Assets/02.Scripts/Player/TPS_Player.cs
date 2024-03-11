using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Player : MonoBehaviour
{
    [SerializeField] Transform tr;
    [SerializeField] Rigidbody rbody;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] Animation _animation;
    public float moveSpeed = 5f;
    public float turnSpeed = 90f;
    private float h, v;
    public bool isRun = false;
    void Start()
    {
        tr = GetComponent<Transform>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rbody = GetComponent<Rigidbody>();
        _animation = GetComponent<Animation>();
    }
    void Update()
    {
        PlayerMove();
        PlayerAnimation();
        Sprint();
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 13f;
            _animation.CrossFade("SprintF", 0.3f);
            isRun = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 5f;
            isRun = false;
        }
    }

    private void PlayerAnimation()
    {
        if (h >= 0.1f) //양수 == D
        {             // 직전 동작과 지금 동작 사이를 0.3초 동안 블랜드 한다.
            _animation.CrossFade("RunR", 0.3f);
        }
        else if (h <= -0.1f) //음수 == A
        {
            _animation.CrossFade("RunL", 0.3f);
        }
        else if (v >= 0.1f)
        {
            _animation.CrossFade("RunF", 0.3f);
        }
        else if (v <= -0.1f)
        {
            _animation.CrossFade("RunB", 0.3f);
        }
        else
        {
            _animation.CrossFade("Idle", 0.3f);
        }
    }

    private void PlayerMove()
    {
        h = Input.GetAxis("Horizontal"); //A,D
        v = Input.GetAxis("Vertical"); //W ,S
        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        tr.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed);
    }
}
