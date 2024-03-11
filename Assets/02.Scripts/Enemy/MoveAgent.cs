using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//경고  애튜리 뷰트  // 이오브젝트에서는  NavMeshAgent가 없으면 안된다.
[RequireComponent(typeof(NavMeshAgent))] 
public class MoveAgent : MonoBehaviour
{
    [SerializeField] private Transform[] Points;
    [SerializeField] private List<Transform> wayPointList = new List<Transform>();
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;
    public int nextIdx = 0;
    public float walkSpeed = 1.5f;
    public float runSpeed = 4.0f;
    private float damping;
    private bool _isPtrolling;
    public bool isPtrolling  //프로퍼티 원본 변수를 보호 하기 위해 
    {
        get { return _isPtrolling; } //읽기만 한다

        set { 
              _isPtrolling = value;  //수정
              if(_isPtrolling)
              {
                agent.speed = walkSpeed;
                damping = 1.0f;
                WayPointMove();
              }
                
           }
    }
    private Vector3 _traceTarget;
    public Vector3  traceTarget
    {
        get {  return  _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = runSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }
    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
        tr = GetComponent<Transform>(); 
        Points = GameObject.Find("PatrolPoints").GetComponentsInChildren<Transform>();
        for(int i =0; i< Points.Length; i++)
        {
            wayPointList.Add(Points[i]);//리스트가 트랜스폼 배열을 다담는다.
        }
        wayPointList.RemoveAt(0); //첫번째 인덱스를 삭제 
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        WayPointMove();
    }
    void Update()
    {         //목적지의 거리가  0.5보다 작거나 같다면 
        if (agent.remainingDistance <= 0.5f)
        {
            nextIdx = ++nextIdx % wayPointList.Count;
            WayPointMove();
        }

    }
    void WayPointMove()
    {     
        //최단 경로가 탐색 되지 않으면 빠져 나간다.
        if (agent.isPathStale) return;

        //추적을 활성화 하고 추적대상을 찾는다.
        agent.isStopped = false;
        agent.destination = wayPointList[nextIdx].position;
         // 패트롤 포인트 첫번째 지점을 찾는다. 
    }
    public void TraceTarget(Vector3 target)
    {
        if(agent.isPathStale) return;
        agent.destination = target;
         agent.isStopped = false;

    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _isPtrolling = false;
    }
}
