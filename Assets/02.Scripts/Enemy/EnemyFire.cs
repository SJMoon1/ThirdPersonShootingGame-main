using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class EnemyFire : MonoBehaviour
{
    [SerializeField] private GameObject E_bullet;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSfx;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform enemyTr;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform firePos;
    public bool isFire = false;
    private float nextTime = 0f;
    private readonly int hashReload = Animator.StringToHash("ReloadTrigger");
    public bool isReloading = false;
    private int remainingBullet = 10;
    private readonly int maxBullet = 10;
    IEnumerator Start()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        E_bullet = (GameObject)Resources.Load("Weapons/E_Bullet");
        fireSfx = Resources.Load<AudioClip>("Sounds/p_m4_1");
        yield return new WaitForSeconds(0.3f);
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").transform;
        firePos = transform.GetChild(5).GetChild(0).GetChild(0).transform;
    }
    void Update()
    {
        if (isFire && !isReloading)
        {
            if (Time.time >= nextTime)
            {
                   
                Fire();
                nextTime = Time.time + Random.Range(0.1f, 0.3f);
               
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * 10f);
        }
       
    }
    void Fire()
    {
        Instantiate(E_bullet, firePos.position, firePos.rotation);
        source.PlayOneShot(fireSfx, 1.0f);
        isReloading = --remainingBullet % maxBullet == 0;
        if (isReloading)
        {
            StartCoroutine(Reloading());
        }
    }
    IEnumerator Reloading()
    {
        isReloading = true;
        animator.SetTrigger(hashReload);
        yield return new WaitForSeconds(1.7f);
        remainingBullet = maxBullet;
        isReloading = false;
    }
}
