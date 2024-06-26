using System.Collections.Generic;
using UnityEngine;

public class Masa : MonoBehaviour
{
    Animator anim;
    
    
    private List<GameObject> enemies = new List<GameObject>(); // 적 배열

    // 유저가 설정한 키를 저장할 변수
    private KeyCode[] userKeys = new KeyCode[108];
    
    
    PlayerStats playerStats;
    GameManager gameManager;
    
    private int singleAtkDamage;
   
    private float singleAtkRange;
    
    private int charDamage;
    

   

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerStats = PlayerStats.playerStats;
        gameManager = GameManager.gameManager;

    }


    // Update is called once per frame
    void Update()
    {
        if(gameManager.gState != GameManager.GameState.Run || playerStats.selectCharacter != SelectCharacter.Masa || !gameManager.isMove || playerStats.IsFly)
        {
            
            return;
        }

        
        // atk1 
        if(Input.GetKeyUp(gameManager.userKeys[(int)SkillEnum.MasaAtk1]))
        {
            MultiAtk("MasaAtk1");
        }

        

        // atk3 
        if(Input.GetKeyUp(gameManager.userKeys[(int)SkillEnum.MasaAtk3]))
        {
            SingleAtk("MasaAtk3",_damageMult: 3);
        }

        
        
    }

    void SingleAtk(string _skillName,float _rangeMult = 1.5f,int _damageMult = 1,float _goldMult = 0.1f)
    {
        int _skillLevel = playerStats.GetSkillLevel(_skillName);

        if(_skillLevel < 0)
        {
            return;
        }

        
        int _damage = (int)(playerStats.InitDamage() * _skillLevel * _damageMult);
       
       
        singleAtkDamage = _damage;
        singleAtkRange = _rangeMult * _skillLevel;

        if(playerStats.UseGold((int)(_skillLevel * _goldMult)))
        {
            IsMove();
            anim.SetTrigger(_skillName);
            
        }
    }

   
    
    
    void MultiAtk(string _skillName,float _rangeMult = 1.5f,int _damageMult = 1,int _cntMult = 1)
    {
        if(playerStats.GetSkillLevel(_skillName) < 0)
        {
            return;
        }
        int _skillLevel =   playerStats.GetSkillLevel(_skillName);
        
        if(playerStats.UseGold((int)(_skillLevel*1.1f)))
        {
            IsMove();
            anim.SetTrigger(_skillName);

            
           
           
            float _range = _rangeMult * _skillLevel;
            int _cnt = (int)(_cntMult + (_skillLevel/10));

           
            int _damage =  playerStats.InitDamage() * _damageMult * _skillLevel;
           

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

    }

   
 
 
    
       


        
        
    

    //애니메이션에서 실행 
    private void SingleAttack()
    {
        // 화면의 정중앙 좌표 계산
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // 카메라에서 화면 정중앙을 기준으로 레이 생성
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        // "IgnoreRaycast" 레이어를 제외한 레이어 마스크 생성
        int layerMask = ~LayerMask.GetMask("Player");

        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(ray, out hitInfo,singleAtkRange,layerMask)) // out키워드는 주소를 복사해 가져옮, 반드시 함수안에서 파라미터 값을 할당할 것을 요구 
        {
            
            if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                print(hitInfo.transform.gameObject.name);
                EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                eFSM.HitEnemy(singleAtkDamage);
                
            }
            
        }
        
    }

    public void IsMove()
    {
        gameManager.isMove = ! gameManager.isMove;
    }


    //이모션 

    // IEnumerator Emotion(float _delay,string _emo)
    // {
        
    //     gm.isMove = false;
    //     Vector3 _temp = ps.camPos.localPosition;
    //     ps.camPos.localPosition = new Vector3(0.04f,0.5f,-2f);
    //     transform.Rotate(0,180,0);
    //     show.SetActive(true);
        
    //     yield return new WaitForSeconds(0.5f);
    //     anim.SetTrigger(_emo);

    //     yield return new WaitForSeconds(_delay);
    //     show.SetActive(false);

    //     gm.isMove = true;
    //     transform.Rotate(0,180,0);
    //     ps.camPos.localPosition = _temp;

    // }


    
   



}
