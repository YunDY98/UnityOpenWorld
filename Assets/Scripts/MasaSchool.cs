using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MasaSchool : MonoBehaviour
{
    Animator anim;
    
    
    private List<GameObject> enemies = new List<GameObject>(); // 적 배열

    // 유저가 설정한 키를 저장할 변수
    private KeyCode[] userKeys = new KeyCode[108];
    
    

    
    private int singleAttackPower;
    private float singleAttackRange;

    private int singleAttackGold;
    

    
    private int multiAttackGold = 30;

   

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        userKeys[0] = KeyCode.C;
        userKeys[1] = KeyCode.X;
       
    }


    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameManager.gState != GameManager.GameState.Run || PlayerStats.playerStats.selectCharacter != PlayerStats.SelectCharacter.MasaSchool || !GameManager.gameManager.isMove)
        {
            
            return;
        }

        
        // atk1 
        if(Input.GetKeyUp(GameManager.gameManager.userKeys[(int)SkillEnum.MasaAtk1]))
        {
            MasaAtk1();
        }

        

        // atk3 
        if(Input.GetKeyUp(userKeys[1]))
        {
            MasaAtk3();
        }

        // if(Input.GetKeyUp(KeyCode.Alpha1))
        // {
        //     if(PlayerStats.playerStats.UseGold((int)(300)))
        //     {
        //         IsMove();
        //         anim.SetTrigger("Attack3");
                
                
               
        //     }

        // }
 
        
    }

    void MasaAtk3()
    {
        if(PlayerStats.playerStats.GetSkillLevel("MasaAtk3") < 0)
        {
            return;
        }
        int _atk3level = PlayerStats.playerStats.GetSkillLevel("MasaAtk3");
        
        
        int _damage = (int)(200* _atk3level * 1.2f);
        //Gold, Damage, range
        SetSingleAttack(300,_damage,4f);
        if(PlayerStats.playerStats.UseGold((int)(singleAttackGold * _atk3level*1.1f)))
        {
            IsMove();
            anim.SetTrigger("Attack3");
            
            
           
        }

    }

    void MasaAtk1()
    {
        if(PlayerStats.playerStats.GetSkillLevel("MasaAtk1") < 0)
        {
            return;
        }
        int atk1level =  PlayerStats.playerStats.GetSkillLevel("MasaAtk1");
        if(PlayerStats.playerStats.UseGold((int)(multiAttackGold * atk1level*1.1f)))
        {
            IsMove();
            anim.SetTrigger("Attack1");
            print(atk1level);
            //Range, cnt , damage
            MultiAttack(2f * (atk1level * 1.1f) ,(int)(1 + (atk1level/10)),(int)(10 * atk1level*1.2f));
           
        }

    }

   

 
    void SetSingleAttack(int _gold,int _damage,float _range)
    {
        singleAttackGold = _gold;
        singleAttackRange =_range;
        singleAttackPower = _damage;


    }
    void MultiAttack(float _range,int _cnt,int _damage)
    {
        // 주변의 적을 감지
        Collider[] colliders = Physics.OverlapSphere(transform.position, _range);
        enemies.Clear(); // 리스트 초기화
        foreach (Collider other in colliders)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                enemies.Add(other.gameObject); // 적을 리스트에 추가
            }
        }

        // cnt 수만큼 적 공격 
       
        foreach (GameObject enemy in enemies)
        {
            if (0 < _cnt)
            {
                // EnemyFSM 컴포넌트 가져오기
                EnemyFSM efsm = enemy.GetComponent<EnemyFSM>();
                if (efsm != null)
                {
                    efsm.HitEnemy(_damage);
                    _cnt--;
                }
            }
            else
            {
                break; // 최대 공격수에 도달하면 루프 종료
            }
        }

      
       

        
       


        
        
    }
    public void SingleAttack()
    {
        // 화면의 정중앙 좌표 계산
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // 카메라에서 화면 정중앙을 기준으로 레이 생성
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        // "IgnoreRaycast" 레이어를 제외한 레이어 마스크 생성
        int layerMask = ~LayerMask.GetMask("Player");

        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(ray, out hitInfo,singleAttackRange,layerMask)) // out키워드는 주소를 복사해 가져옮, 반드시 함수안에서 파라미터 값을 할당할 것을 요구 
        {
            print(hitInfo.transform.gameObject.name);
            if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                
                EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                eFSM.HitEnemy(singleAttackPower);
                
            }
            
        }
        
    }

    public void IsMove()
    {
        GameManager.gameManager.isMove = !GameManager.gameManager.isMove;
    }


    //이모션 

    // IEnumerator Emotion(float _delay,string _emo)
    // {
        
    //     GameManager.gameManager.isMove = false;
    //     Vector3 _temp = PlayerStats.playerStats.camPos.localPosition;
    //     PlayerStats.playerStats.camPos.localPosition = new Vector3(0.04f,0.5f,-2f);
    //     transform.Rotate(0,180,0);
    //     show.SetActive(true);
        
    //     yield return new WaitForSeconds(0.5f);
    //     anim.SetTrigger(_emo);

    //     yield return new WaitForSeconds(_delay);
    //     show.SetActive(false);

    //     GameManager.gameManager.isMove = true;
    //     transform.Rotate(0,180,0);
    //     PlayerStats.playerStats.camPos.localPosition = _temp;

    // }

   



}
