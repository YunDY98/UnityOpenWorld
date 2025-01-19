using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class SoldierFire : CharacterAtk
{
    
    
    
    enum WeaponMode
    {
        None,
        Rifle,
        Sniper
    }
    WeaponMode wMode;

    public GameObject[] muzzleFlash;

 
    //public PlayerStats playerStats;
    private int useBullets;
    private int useBomb;
    private int damageRate = 1;
    public TMP_Text WModeTxt;

    bool zoomMode = false;

    //총을 쏠수 있는지 
    bool canShoot;
    public GameObject firePosition;

    public CamRotate camRotate;

    public GameObject bombFactory;

    public float throwPower = 15f;

    public GameObject bulletEffect;
   


    ParticleSystem bulletParticle;
    protected override void Start ()
    {   
        
        base.Start ();
        attackRange = Mathf.Infinity;
        useBomb = 10;
        
        Rifle();
        bulletParticle = bulletEffect.GetComponent<ParticleSystem>();

        
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
       
        // // 게임 중일때만 동작                                                                                                           
        if((GameManager.gameManager.gState != GameManager.GameState.Run) || (playerStats.selectCharacter != SelectCharacter.Soldier)|| !GameManager.gameManager.isMove)
        {
            return;
        }
       
       
        if(GameManager.gameManager.isUI)
        {
            
            return;
        }
        
        if(Input.GetMouseButtonDown(0))
        {
          
            if(!canShoot)
                return;
            

            if(playerStats.UseGold(useBullets))
            {
               

                anim.SetTrigger("Attack");

                if(WeaponMode.Sniper == wMode)
                {
                   
                    
                    StartCoroutine(Shoot(3f));
                }
                   
                else
                {
                    
                    StartCoroutine(Shoot(0f));
                }
                    

            } 
            
        }

        if(Input.GetMouseButtonDown(1))
        {
            switch(wMode)
            {
                
                case WeaponMode.Sniper:
                    if(!zoomMode)
                    {

                        damageRate = 5;
                        Camera.main.fieldOfView = 15f;
                        zoomMode = true;
                    
                    }
                    else
                    {
                       
                        attackDamage = (int)(playerStats.InitDamage() * 2);
                        damageRate = 2;
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;
                        

                    }
                    break;
            }
        }
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            Rifle();



        }
        if(Input.GetKeyUp(KeyCode.Alpha2))
        {   
            WModeTxt.text = "Sniper";
            useBullets = 5 * playerStats.Level;
            
            attackDamage = (int)(playerStats.InitDamage() * 5);
            damageRate = 2;
            
            wMode = WeaponMode.Sniper;
        }

       
       
        
        if(Input.GetKeyUp(KeyCode.Alpha6))
        {
            if(PlayerStats.playerStats.UseGold(useBomb))
            {
                GameObject bomb = Instantiate(bombFactory);
                bomb.transform.position = firePosition.transform.position;
                Rigidbody rb = bomb.GetComponent<Rigidbody>();
                rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
            }
           
        }
    }

    IEnumerator Shoot(float delay)
    {
        

        
       
        RaycastHit _hitInfo = RaycastAtk();
    
        if(_hitInfo.transform.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            // hit 지점에서 이팩트 
            bulletEffect.transform.position = _hitInfo.point;
            // 이펙트 로워드 방향을 레이가 부딪힌 지점의 법선 벡터와 일치 시캄
            bulletEffect.transform.forward = _hitInfo.normal;
            bulletParticle.Play();
            
        }
        
        

        StartCoroutine(ShootEffectOn(0.05f));

        canShoot = false;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
   

    IEnumerator ShootEffectOn(float duration)
    {
        int _num = Random.Range(0,muzzleFlash.Length);

      

        muzzleFlash[_num].SetActive(true);

        yield return new WaitForSeconds(duration);

        muzzleFlash[_num].SetActive(false);
    }

    void Rifle()
    {
       
        damageRate = 1;
        wMode = WeaponMode.Rifle;
        WModeTxt.text = "Rifle";
        useBullets = 2 * playerStats.Level;
        Camera.main.fieldOfView = 60f;
       

    }
}
