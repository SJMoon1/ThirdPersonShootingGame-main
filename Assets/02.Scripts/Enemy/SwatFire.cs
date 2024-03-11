using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SwatFire : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform swatTr;
    [SerializeField] private Transform firePos;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireClip;
    public bool isFire = false;
    private float fireRate = 0.1f;
    private float nextTime = 0f;
    private readonly int hashReload = Animator.StringToHash("ReloadTrigger");
    public bool isReloading = false;
    private WaitForSeconds reloadWs = new WaitForSeconds(2.0f);
    private int remainingBullet = 10;
    private readonly int maxBullet = 10;
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        swatTr = GetComponent<Transform>();
        firePos = transform.GetChild(2).GetChild(0).GetChild(0).transform;
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        bulletPrefab = Resources.Load<GameObject>("Weapons/E_Bullet");
        fireClip = Resources.Load<AudioClip>("Sounds/p_m4_1");
    }
    void Update()
    {
        if(isFire&&!isReloading)
        {
            if(Time.time > nextTime)
            {
                Fire();
                nextTime = Time.time + fireRate + Random.Range(0.0f, 0.2f);
            }
            Vector3 targetNormal = (playerTr.position - swatTr.position);
            swatTr.rotation = Quaternion.Slerp(swatTr.rotation, Quaternion.LookRotation(targetNormal),
                Time.deltaTime * 10f);
        }
        

    }
    void Fire()
    {
        Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        source.PlayOneShot(fireClip, 1.0f);
        isReloading = --remainingBullet % maxBullet == 0;
        if (isReloading)
            StartCoroutine(Reloading());
    }
    IEnumerator Reloading()
    {
        isReloading = true;
        animator.SetTrigger(hashReload);
        yield return reloadWs;
        isReloading = false;
        remainingBullet = maxBullet;
    }
}
