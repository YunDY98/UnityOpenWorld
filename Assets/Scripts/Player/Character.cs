using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected List<GameObject> enemies = new List<GameObject>(); // 적 배열

    protected Animator anim;
    protected PlayerStats playerStats;
    protected GameManager gameManager;

    // 공격 관련 필드
    protected int attackDamage;
    protected float attackRange;

    // 시작 시 공통 초기화
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        playerStats = PlayerStats.playerStats;
        gameManager = GameManager.gameManager;
    }

    // 기본 단일 공격 메서드
    protected void SingleAtk(string skillName, float rangeMult = 1.5f, int damageMult = 1, float goldMult = 0.1f)
    {
        int _skillLevel = playerStats.GetSkillLevel(skillName);
        

        if (_skillLevel < 0 || !playerStats.UseGold((int)(_skillLevel * goldMult)))
        {
            return;
        }

        attackDamage = (int)(playerStats.InitDamage() * _skillLevel * damageMult);
        attackRange = rangeMult * _skillLevel;

        anim.SetTrigger(skillName);

        RaycastAtk();
    }

    // Raycast를 통한 공격 처리
    protected RaycastHit RaycastAtk()
    {
        // 화면의 정중앙 좌표 계산
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // 카메라에서 화면 정중앙을 기준으로 레이 생성
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        // "IgnoreRaycast" 레이어를 제외한 레이어 마스크 생성
        int layerMask = ~LayerMask.GetMask("Player");

        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(ray, out hitInfo,attackRange,layerMask)) // out키워드는 주소를 복사해 가져옮, 반드시 함수안에서 파라미터 값을 할당할 것을 요구 
        {
            
            if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                print(hitInfo.transform.gameObject.name);
                EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                print(attackDamage);
                eFSM?.HitEnemy(attackDamage);
                return hitInfo;
            }
        }
        return hitInfo;
    }
    protected void MultiAtk(string _skillName,float _rangeMult = 1.5f,int _damageMult = 1,int _cntMult = 1)
    {
        if(playerStats.GetSkillLevel(_skillName) < 0)
        {
            return;
        }
        int _skillLevel = playerStats.GetSkillLevel(_skillName);
        
        if(playerStats.UseGold((int)(_skillLevel*1.1f)))
        {
            
            anim.SetTrigger(_skillName);

            
           
           
            float _range = _rangeMult * _skillLevel;
            int _cnt = (int)(_cntMult + (_skillLevel/10));

           
            int _damage =  playerStats.InitDamage() * _damageMult * _skillLevel;
           

            // 주변의 적을 감지
            Collider[] colliders = Physics.OverlapSphere(transform.position, _range);
            enemies.Clear(); // 리스트 초기화
            foreach(Collider other in colliders)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    enemies.Add(other.gameObject); // 적을 리스트에 추가
                }
            }
    
            // cnt 수만큼 적 공격 
        
            foreach(GameObject enemy in enemies)
            {
                if(0 < _cnt)
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
   
}