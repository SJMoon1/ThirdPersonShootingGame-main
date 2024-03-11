using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private GameObject BloodEffect;
    private readonly string e_bulletTag = "E_BULLET";
    void Start()
    {
        BloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(e_bulletTag))
        {
            Destroy(col.gameObject);

            Vector3 hitpos = col.contacts[0].point; //맞은 지점
            Vector3 _normal = col.contacts[0].normal;
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            GameObject blood = Instantiate<GameObject>(BloodEffect, hitpos, rot);
            Destroy(blood, 0.5f);
        }    

    }

}
