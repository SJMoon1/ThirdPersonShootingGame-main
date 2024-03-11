using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private GameObject BloodEffect;
    private readonly string playerbulletTag = "BULLET";
    private float hp = 0f;
    private float maxhp = 100f;
    void Start()
    {
        BloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");
        hp = maxhp;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(playerbulletTag))
        {
            Destroy(col.gameObject);
            hp -= col.gameObject.GetComponent<BulletCtrl>().damage;
            if(hp <= 0f)
            {
                //GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                GetComponent<EnemyAI>().Die();
            }

            ShowBloodEffect(col);

        }

    }

    private void ShowBloodEffect(Collision col)
    {
        Vector3 hitpos = col.contacts[0].point; //맞은 지점
        Vector3 _normal = col.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate<GameObject>(BloodEffect, hitpos, rot);
        Destroy(blood, 0.5f);
    }
}
