using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAtk : MonoBehaviour
{
    protected List<GameObject> enemies = new List<GameObject>(); // 적 배열

    protected Animator anim;
    protected PlayerStats playerStats;
    protected GameManager gameManager;

    // 공격 관련 필드
    protected int attackDamage;
    protected float attackRange;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // 시작 시 공통 초기화
    protected virtual void Start()
    {
       
        playerStats = PlayerStats.Instance;
        gameManager = GameManager.Instance;
        
        
    }

    // 기본 단일 공격 메서드
    protected virtual void SingleAtk(string skillName, float rangeMult = 1.5f, int damageMult = 1, float goldMult = 0.1f)
    {
        int skillLevel = playerStats.GetSkillLevel(skillName);
        

        if (skillLevel < 0 || !playerStats.UseGold((int)(skillLevel * goldMult)))
        {
            return;
        }

        attackDamage = (int)(playerStats.InitDamage() * skillLevel * damageMult);
        attackRange = rangeMult * skillLevel;

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
        if(Physics.Raycast(ray, out hitInfo,attackRange,layerMask))
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
    protected virtual void MultiAtk(string skillName,float rangeMult = 1.5f,int damageMult = 1,int cntMult = 1)
    {
        if(playerStats.GetSkillLevel(skillName) < 0)
        {
            return;
        }
        int skillLevel = playerStats.GetSkillLevel(skillName);
        
        if(playerStats.UseGold((int)(skillLevel*1.1f)))
        {
            
            anim.SetTrigger(skillName);

           
        }

        MultiRaycastAtk(rangeMult,damageMult,cntMult,skillLevel);
       

    }

    protected virtual void MultiRaycastAtk(float rangeMult,int damageMult,int cntMult,int skillLevel)
    {
             
            float range = rangeMult * skillLevel;
            int cnt = (int)(cntMult + (skillLevel/10));

           
            int damage =  playerStats.InitDamage() * damageMult * skillLevel;
            print("InitDamage" + playerStats.InitDamage() +"DamageMult" + damageMult +"SKillLevle" + skillLevel);
           
            // 주변의 적을 감지
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
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
                if(0 < cnt)
                {
                    // EnemyFSM 컴포넌트 가져오기
                    EnemyFSM efsm = enemy.GetComponent<EnemyFSM>();
                    if (efsm != null)
                    {
                        efsm.HitEnemy(damage);
                        cnt--;
                    }
                }
                else
                {
                    break; // 최대 공격수에 도달하면 루프 종료
                }
            }
    }
   
}