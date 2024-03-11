using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { PATROL=1,TRACE,ATTACK,DIE}
    public State state = State.PATROL;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;
    [SerializeField] private MoveAgent moveAgent;
    [SerializeField] private EnemyFire enemyFire;
    public float attackDist = 5f;
    public float traceDist = 10f;
    public bool isDie = false;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("ForwardSpeed");
    private readonly int hashDie = Animator.StringToHash("DieTrigger");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");

    void Awake()
    {
        enemyFire = GetComponent<EnemyFire>();
        moveAgent = GetComponent<MoveAgent>();
        animator = GetComponent<Animator>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        tr = GetComponent<Transform>();
    }
    void OnEnable() //오브젝트가 활성화 될때 자동 호출 // 오브젝트 풀링
    {
        StartCoroutine(EnemyState()); //거리를 재어서 현재 상태만 알려준다.
        StartCoroutine(EnemyAction());
    }
    IEnumerator EnemyState()
    {
        while (!isDie) //계속 조건이 참이면 반복 해서 프레임이 돌게 할려고 
        {
            float dist = Vector3.Distance(playerTr.position, tr.position);
            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator EnemyAction()
    {
       
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.isPtrolling = true;
                    animator.SetBool(hashMove,true);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if(enemyFire.isFire == false)
                       enemyFire.isFire= true;
                    break;
                case State.DIE:
                    Die();

                    break;
            }

        }

    }

    public void Die()
    {
        enemyFire.isFire = false;
        state = State.DIE;
        isDie = true;
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 2));
        GetComponent<Rigidbody>().isKinematic = true; //물리 없음
        GetComponent<CapsuleCollider>().enabled = false;// 콜라이더 비활성화
        moveAgent.Stop();
        
    }

    void Start()
    {

    }
    
    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);

    }
    void OnDisable() //오브젝트가 비활성화 될때 자동 호출
    {

    }
}
