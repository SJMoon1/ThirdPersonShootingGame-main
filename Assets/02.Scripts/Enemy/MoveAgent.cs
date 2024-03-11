using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//���  ��Ʃ�� ��Ʈ  // �̿�����Ʈ������  NavMeshAgent�� ������ �ȵȴ�.
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
    public bool isPtrolling  //������Ƽ ���� ������ ��ȣ �ϱ� ���� 
    {
        get { return _isPtrolling; } //�б⸸ �Ѵ�

        set { 
              _isPtrolling = value;  //����
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
            wayPointList.Add(Points[i]);//����Ʈ�� Ʈ������ �迭�� �ٴ�´�.
        }
        wayPointList.RemoveAt(0); //ù��° �ε����� ���� 
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        WayPointMove();
    }
    void Update()
    {         //�������� �Ÿ���  0.5���� �۰ų� ���ٸ� 
        if (agent.remainingDistance <= 0.5f)
        {
            nextIdx = ++nextIdx % wayPointList.Count;
            WayPointMove();
        }

    }
    void WayPointMove()
    {     
        //�ִ� ��ΰ� Ž�� ���� ������ ���� ������.
        if (agent.isPathStale) return;

        //������ Ȱ��ȭ �ϰ� ��������� ã�´�.
        agent.isStopped = false;
        agent.destination = wayPointList[nextIdx].position;
         // ��Ʈ�� ����Ʈ ù��° ������ ã�´�. 
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
