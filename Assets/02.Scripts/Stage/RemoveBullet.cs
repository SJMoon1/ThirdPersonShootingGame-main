using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1. 파티클 구현 소리 구현 
public class RemoveBullet : MonoBehaviour
{
    [SerializeField] GameObject Spark;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hitclip;
    private string bulletTag = "BULLET";
    private string E_bulletTag = "E_BULLET";
    void Start()
    {
        source = GetComponent<AudioSource>();
        hitclip = Resources.Load<AudioClip>("Sounds/bullet_hit_metal_enemy_4");
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(bulletTag)|| col.collider.CompareTag(E_bulletTag))
        {       
            Destroy(col.gameObject);
            //맞은 위치를 hitPos에 넘겨준다.
            Vector3 hitPos = col.contacts[0].point;
            Vector3 _normal = col.contacts[0].normal; // 발사한 방향
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            var spk = Instantiate(Spark,hitPos,rot);
            Destroy(spk,2.5f);
            source.PlayOneShot(hitclip, 1.0f);
        }

    }
}
