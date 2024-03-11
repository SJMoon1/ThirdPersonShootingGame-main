using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//1.총알 프리팹 2. 발사위치 3. 총소리 오디오소스 오디오 클립
public class FireCtrl : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem CartridgeEjectEffect;
    [SerializeField] private Image magazineImg;
    [SerializeField] private Text magazineText;
    [SerializeField] private Animator animator;
    private readonly int hashReload = Animator.StringToHash("ReloadTrigger");
    private readonly int hashFire = Animator.StringToHash("FireTrigger");
    private int remaingBullet = 0;
    private readonly int maxBullet = 10;
    private float timePrev;
    private float fireRate = 0.1f; //발사 간격 시간 
    private Player_Mecanim player;
    private bool isReloding = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        bulletPrefab = Resources.Load("Weapons/Bullet")as GameObject;
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Transform>();
        player = GetComponent<Player_Mecanim>();
        source = GetComponent<AudioSource>();
        magazineImg = GameObject.Find("Panel-Magazine").transform.GetChild(2).GetComponent<Image>();
        magazineText = GameObject.Find("Panel-Magazine").transform.GetChild(0).GetComponent<Text>();
        muzzleFlash = firePos.GetChild(0).GetComponent<ParticleSystem>();
        CartridgeEjectEffect = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
        fireClip = Resources.Load<AudioClip>("Sounds/p_ak_1");
        muzzleFlash.Stop();
        remaingBullet = maxBullet;
        remaingBullet = Mathf.Clamp(remaingBullet, 0, 10);
    }
    void Update()
    {
        if(Input.GetMouseButton(0)&&Time.time - timePrev >fireRate)
        {
             if(!player.isRun &&!isReloding)
             {
                --remaingBullet;
                 Fire();
                if(remaingBullet ==0)
                {
                    StartCoroutine(Reloading());
                }
             }
                 
            timePrev = Time.time;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            muzzleFlash.Stop();
            CartridgeEjectEffect.Stop();
        }
    }
    void Fire()
    {
        source.PlayOneShot(fireClip,1.0f);
        Instantiate(bulletPrefab, firePos.position,firePos.rotation);
        animator.SetTrigger(hashFire);
        muzzleFlash.Play() ;
        CartridgeEjectEffect.Play();
        magazineImg.fillAmount = (float)remaingBullet / (float)maxBullet;
        
        magazineTextShow();
    }
    void magazineTextShow()
    {
        magazineText.text = "<color=#ff0000>" + remaingBullet.ToString() + "</color>" + "/" + maxBullet.ToString();
    }
    IEnumerator Reloading()
    {
        magazineTextShow();
        isReloding = true;
        animator.SetTrigger(hashReload);
        muzzleFlash.Stop();
        CartridgeEjectEffect.Stop();
        yield return new WaitForSeconds(1.5f);
        magazineImg.fillAmount = 1.0f;
        isReloding = false;
        remaingBullet = maxBullet;
        magazineTextShow();

    }
}
