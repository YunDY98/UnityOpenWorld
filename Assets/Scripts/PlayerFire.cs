using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerFire : MonoBehaviour
{
    enum WeaponMode
    {
        None,
        Rifle,
        Sniper
    }
    WeaponMode wMode;

    public GameObject[] muzzleFlash;

    public TMP_Text WModeTxt;

    bool ZoomMode = false;
    public GameObject firePosition;

    public GameObject bombFactory;

    public float throwPower = 15f;

    public GameObject bulletEffect;

    Animator anim;
    

    //발사 무기 공격력
    public int weaponPower = 1;

    public ParticleSystem ps;
    void Start ()
    {   
        wMode = WeaponMode.Rifle;
        
        //ps = bulletEffect.GetComponent<ParticleSystem>();

        anim = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
         // 게임 중일때만 동작 
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            
            anim.SetTrigger("Attack");

            StartCoroutine(ShootEffectOn(0.05f));
            

        }

        if(Input.GetMouseButtonDown(1))
        {
            switch(wMode)
            {
                case WeaponMode.None:
                    break;
                case WeaponMode.Sniper:
                    if(!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;

                       
                    }
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;
                        

                    }
                    break;
            }
        }
        if(Input.GetKeyUp(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Rifle;
            WModeTxt.text = "Rifle";
            Camera.main.fieldOfView = 60f;

        }
        else if(Input.GetKeyUp(KeyCode.Alpha1))
        {   
            WModeTxt.text = "Sniper";
            
            wMode = WeaponMode.Sniper;
        }

       
       

        if(Input.GetKeyUp(KeyCode.Alpha3))
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }

        if(Input.GetMouseButtonDown(0))
        {
            // 레이 시작점 과 방향 
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            if(Physics.Raycast(ray, out hitInfo)) // out키워드는 주소를 복사해 가져옮, 반드시 함수안에서 파라미터 값을 할당할 것을 요구 
            {
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    Debug.Log("hitEnemy");
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                    
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
        }
        
    }

    IEnumerator ShootEffectOn(float duration)
    {
        int num = Random.Range(0,muzzleFlash.Length);

        muzzleFlash[num].SetActive(true);

        yield return new WaitForSeconds(duration);

        muzzleFlash[num].SetActive(false);
    }
}
