using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
public class SoliderFire : MonoBehaviour
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

    public TMP_Text WModeTxt;

    bool zoomMode = false;

    //총을 쏠수 있는지 
    bool canShoot;
    public GameObject firePosition;

    public GameObject bombFactory;

    public float throwPower = 15f;

    public GameObject bulletEffect;

    Animator anim;
    
    
    //발사 무기 공격력
    public int weaponDamage = PlayerStats.playerStats.AtkDamage;

    public ParticleSystem ps;
    void Start ()
    {   
        anim = GetComponentInChildren<Animator>();
        wMode = WeaponMode.Rifle;
        useBullets = 1;
        useBomb = 10;

        //ps = bulletEffect.GetComponent<ParticleSystem>();

        canShoot = true;
    }

  


    // Update is called once per frame
    void Update()
    {
       
        // 게임 중일때만 동작 
        if((GameManager.gameManager.gState != GameManager.GameState.Run) || (PlayerStats.playerStats.selectCharacter != PlayerStats.SelectCharacter.Solider))
        {
           
            return;
        }
       
       print("solider");
        
        if(Input.GetMouseButtonDown(0))
        {
            if(PlayerStats.playerStats.UseGold(useBullets) && canShoot)
            {
               

                anim.SetTrigger("Attack");

                if(WeaponMode.Sniper == wMode)
                    StartCoroutine(Shoot(3f));
                else
                    StartCoroutine(Shoot(0f));


            }
           
            
        }

        
       

        if(Input.GetMouseButtonDown(1))
        {
            switch(wMode)
            {
                
                case WeaponMode.Sniper:
                    if(!zoomMode)
                    {
                        weaponDamage = (int)(weaponDamage * 1.5f);
                        Camera.main.fieldOfView = 15f;
                        zoomMode = true;
                       
                       
                    }
                    else
                    {
                        weaponDamage = (int)(weaponDamage * 5f);
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;
                        

                    }
                    break;
            }
        }
        if(Input.GetKeyUp(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Rifle;
            WModeTxt.text = "Rifle";
            useBullets = 1;
            Camera.main.fieldOfView = 60f;


        }
        else if(Input.GetKeyUp(KeyCode.Alpha1))
        {   
            WModeTxt.text = "Sniper";
            useBullets = 5;
            
            
            wMode = WeaponMode.Sniper;
        }

       
       
        
        if(Input.GetKeyUp(KeyCode.Alpha3))
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

    IEnumerator Shoot(float _delay)
    {
        // 화면의 정중앙 좌표 계산
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // 카메라에서 화면 정중앙을 기준으로 레이 생성
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        
        // 레이 시작점 과 방향 
        //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(ray, out hitInfo)) // out키워드는 주소를 복사해 가져옮, 반드시 함수안에서 파라미터 값을 할당할 것을 요구 
        {
            if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
               
                EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                eFSM.HitEnemy(weaponDamage);
                
            }
            else
            {
                // hit 지점에서 이팩트 
                bulletEffect.transform.position = hitInfo.point;

                // 이펙트 로워드 방향을 레이가 부딪힌 지점의 법선 벡터와 일치 시캄
                bulletEffect.transform.forward = hitInfo.normal;

                ps.Play();
            }
        }

        StartCoroutine(ShootEffectOn(0.05f));

        canShoot = false;
        yield return new WaitForSeconds(_delay);
        canShoot = true;

        

      

    }
   

    IEnumerator ShootEffectOn(float duration)
    {
        int num = Random.Range(0,muzzleFlash.Length);

        muzzleFlash[num].SetActive(true);

        yield return new WaitForSeconds(duration);

        muzzleFlash[num].SetActive(false);
    }
}
