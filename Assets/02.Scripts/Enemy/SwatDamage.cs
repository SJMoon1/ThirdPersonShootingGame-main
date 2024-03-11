using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatDamage : MonoBehaviour
{
    [SerializeField] private GameObject BloodEffect;
    public float hp = 100f;
    public float hpMax = 100f;
    private  string bulletTag ="BULLET";
    void Start()
    {

       BloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");

    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(bulletTag))
        {
            hp -= col.gameObject.GetComponent<BulletCtrl>().damage;
            ContactPoint cp = col.GetContact(0);
            Vector3 _normal = col.GetContact(0).normal;
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            var blood = Instantiate(BloodEffect, cp.point, rot);
            Destroy(blood, 0.5f);
            if(hp<=0f)
            {
                GetComponent<SwatAI>().Die();
            }
        }
        
    }

}
