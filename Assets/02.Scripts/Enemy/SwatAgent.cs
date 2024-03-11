using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwatAgent : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private List<Transform> patrolList;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform swatTr;
    private float walkSpeed = 1.5f;
    private float runSpeed = 4f;
    private int nextIdx = 0;
    private float damping;
    private bool _isPatrolling;
    public bool IsPatrolling
    {
        get { return _isPatrolling; }
        set
        {
            _isPatrolling = value;
            agent.speed = walkSpeed;
            damping = 1f;
        }
    }
    private Vector3 _tracetarget;
    public Vector3 tracetarget
    {
        get { return _tracetarget; }
        set
        {
            _tracetarget = value;
            damping = 7.0f;
            agent.speed = runSpeed;
            TraceTarget(_tracetarget);
        }
    }
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        swatTr = GetComponent<Transform>();
        patrolPoints = GameObject.Find("PatrolPoints").GetComponentsInChildren<Transform>();
        for(int i =0; i< patrolPoints.Length; i++)
        {
            patrolList.Add(patrolPoints[i]);
        }
        patrolList.RemoveAt(0);
        WayPointMove();
    }
    void Update()
    {
        if(agent.isStopped ==false)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            swatTr.rotation = Quaternion.Slerp(swatTr.rotation, rot, Time.deltaTime * damping);
        }
        if (!IsPatrolling) return;
        if(agent.velocity.sqrMagnitude > 0.2f*0.2f && agent.remainingDistance <=0.3f)
        {
            nextIdx = ++nextIdx % patrolList.Count;
            WayPointMove();
        }


    }
    void WayPointMove()
    {
        if (agent.isPathStale) return;
        agent.destination = patrolList[nextIdx].position;
        agent.isStopped = false;
    }
    void TraceTarget(Vector3 target)
    {
        if (agent.isPathStale) return;
        agent.destination = target;
       
        agent.isStopped = false;
    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        IsPatrolling = false;
    }
}
