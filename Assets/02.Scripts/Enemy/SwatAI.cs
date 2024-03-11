using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatAI : MonoBehaviour
{
    public enum State { PATROL=1,TRACE,ATTACK,DIE}
    public State state = State.PATROL;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform swatTr;
    [SerializeField] private SwatAgent swatAgent;
    [SerializeField] private SwatFire swatFire;

    public float attackDist = 5f;
    public float traceDist = 10f;
    private WaitForSeconds ws = new WaitForSeconds(0.3f);
    public bool isDie = false;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("FowardSpeed");
    private readonly int hashDie = Animator.StringToHash("DieTrigger");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerTr = GameObject.FindWithTag("Player").transform;
        swatTr = GetComponent<Transform>();
        swatAgent = GetComponent<SwatAgent>();
        swatFire = GetComponent<SwatFire>();
    }
    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(StateAction());
    }
    IEnumerator CheckState()
    {
        while(!isDie)
        {
            float dist = (playerTr.position - swatTr.position).magnitude;
            if (dist <= attackDist)
                state = State.ATTACK;
            else if (dist <= traceDist)
                state = State.TRACE;
            else
                state = State.PATROL;
            yield return ws;
        }
    }
    IEnumerator StateAction()
    {
        while (!isDie)
        {
            yield return ws;
            switch(state)
            {
                case State.PATROL:
                    swatFire.isFire = false;
                    swatAgent.IsPatrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    swatFire.isFire = false;
                    swatAgent.tracetarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    if (isDie) yield break;
                    swatAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if (swatFire.isFire == false)
                        swatFire.isFire = true;
                    break;
                case State.DIE:
                    
                    Die();
                    break;
            }

        }
    }
    public void Die()
    {
        swatAgent.Stop();
        isDie = true;
        swatFire.isFire = false;
        state = State.DIE;
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 2));
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        //StopAllCoroutines();
    }

    void Update()
    {
        animator.SetFloat(hashSpeed, swatAgent.speed);

    }
    private void OnDisable()
    {
        
    }
}
