using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] private Transform tr;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TrailRenderer trailRenderer;
    public float speed = 2500f;
    public float damage = 34f;
    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        rb.AddForce(tr.forward * 2500f);
        Destroy(gameObject, 3.0f);
    }

   
}
