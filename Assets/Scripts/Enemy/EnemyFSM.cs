using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;
using TMPro;
public class EnemyFSM : MonoBehaviour
{
    NavMeshAgent smith;
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    [Header("EnemyInfo")]
    public float attackDistance = 1f;
    public float moveSpeed = 7f;

    // 플레이어 발견 범위
    public float findDistance = 10f;

    //이동 가능 범위
    public float moveDistance = 20f;


    //적 HP
    public int hp = 15000;
    public int maxHp = 15000;
    public Slider hpSlider;

    public TextMeshProUGUI damageText;

    //공격력 
    public int attackPower = 30;

    
    //초기 위치 
    Vector3 originPos;
    Quaternion originRot;

    

    //public CharacterController cc;

    PlayerMove pm;

    Animator anim;
    //누적 시간 
    private float currentTime = 0f;
    private float attackDelay = 2f;
    // 에너미 상태 변수
    EnemyState m_State;

    NavMeshAgent navMeshAgent;

    Coroutine damageDisplay;
   
    //플레이어 트랜스폼 
    Transform player;

    //아이템 풀
    ItemPool itemPool;
    
    // Start is called before the first frame update
    void Start()
    {
        itemPool = FindObjectOfType<ItemPool>();
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_State = EnemyState.Idle;
        pm = GameObject.Find("Player").GetComponent<PlayerMove>();

        //자신의 초기 위치 
        originPos = transform.position;
        originRot = transform.rotation;

        smith = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player").transform;

        anim = transform.GetComponentInChildren<Animator>();
        navMeshAgent.enabled = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        //print(m_State);
        hpSlider.value = (float)hp/(float)maxHp;

        switch (m_State)
        {
            case EnemyState.Idle:  
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;

        }
        
    }

    void Idle()
    {
       
       
        // 플레이어와 거리가 액션 시작범위 이내라면 move 상태로 전환
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
           // print("Idle -> Move");

            //이동 애니메이션
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
       
       
        if(m_State == EnemyState.Die)
        {
            
            return;
        }


        //초기 위치에서 이동 가능 범위를 넘어간다면 
        if(Vector3.Distance(transform.position,originPos) > moveDistance || Vector3.Distance(transform.position,player.position) > moveDistance)
        {
            
            m_State = EnemyState.Return;
            //print("Move -> Return");
        }
        // 플레이어와의 거리가 공격 범위 밖이라면 플레이러를 향해 이동
        else if(Vector3.Distance(transform.position,player.position) > attackDistance)
        {
            
            // //이동 방향 설정 
            // Vector3 dir = (player.position - transform.position).normalized;    

            // //캐릭터 컨트롤러 이용하여 이동 
            // cc.Move(dir * moveSpeed * Time.deltaTime);

            // //플레이어를 향해 방향 전환 
            // transform.forward = dir;

            RouteReset();

            //내비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정
            smith.stoppingDistance = attackDistance;

            //내비게이션의 목적지를 플레이어의 위치로 설정
            smith.destination = player.position;
            


        }
        else
        {
           
            m_State = EnemyState.Attack;
            //print("Move -> Attack");
           

            //공격 대기 애니메이션 플레이 
            anim.SetTrigger("MoveToAttackDelay");
        }
       
    }

    void Attack()
    {
        

        //일정한 시간 마다 플레이러를 공격
        currentTime += Time.deltaTime;
       
        if(currentTime >= attackDelay)
        {
            
            // 플레이어가 공격 범위 이내에 있다면 공격 
            if(Vector3.Distance(transform.position,player.position) < attackDistance)
            {
               
                currentTime = 0;
                
                //공격 애니메이션 플레이 
                anim.SetTrigger("StartAttack");

            }
            else
            {
                m_State = EnemyState.Move;
                anim.SetTrigger("AttackToMove");
            }
        }
        else
        {
           
            m_State = EnemyState.Move;
            //  print("Attack -> Move");
            
            //이동 
            anim.SetTrigger("AttackToMove");

        }
    }

    //플레이어의 스크립트의 데미지 처리 함수를 실행 
    public void AttackAction()
    {
        if(Vector3.Distance(transform.position,player.position) < attackDistance)
            PlayerStats.playerStats.DamageAction(attackPower);
            
    }

    void Return()
    {
        //리턴중 공격거리내 캐릭터가 있을시 
        if(Vector3.Distance(transform.position,originPos) < moveDistance && Vector3.Distance(transform.position,player.position) < attackDistance)
        {
            m_State = EnemyState.Move;
            anim.SetTrigger("Move");
            
            
        }
        else if(Vector3.Distance(transform.position,originPos) > 0.2f)
        {
            
            
            // Vector3 dir = (originPos - transform.position).normalized;
            // cc.Move(dir * moveSpeed * currentTime* Time.deltaTime);

            // //방향을 복귀 지점으로 
            // transform.forward = dir;

            //내비게이션의 목적지를 초기 저장된 위치로 설정한다.
            smith.destination = originPos;

            //내비게이션으로 접근하는 최소 거리를 '0'으로 설정
            smith.stoppingDistance = 0;

        }
        else
        {
            
            RouteReset();

            transform.position = originPos;
            transform.rotation = originRot;

            //hp 다시 회복 
            hp = maxHp;

            m_State =  EnemyState.Idle;
           // print("Return -> Idle");

            anim.SetTrigger("MoveToIdle");

        }
    }
    IEnumerator DamageDisplay(int _damaged)
    {
        float _time = 0;
        float _duration = 1.5f;
       
        if(_damaged <= 0)
        {
            damageText.text = "Miss";
        }
        else
        {
            damageText.text = _damaged.ToString();
        }
       
        while(_time < _duration)
        {
            _time += Time.deltaTime;
            yield return null;
        }



        damageText.text = "";
       
    }



    public void HitEnemy(int _damaged)
    {
        

        if(m_State == EnemyState.Die)
            return;
        
        if(damageDisplay != null)
            StopCoroutine(damageDisplay);

        damageDisplay = StartCoroutine(DamageDisplay(_damaged));
        

        RouteReset();

        //플레이어의 공격력만큼 에너미 체력 감소 
        hp -= _damaged;
        
    
        if(hp <= 0)
        {
            damageText.text = "";
            m_State = EnemyState.Die;

            //print("Any State -> Die");
            
            Die();
            damageDisplay = StartCoroutine(DamageDisplay(_damaged));
            return;
        }
       
        //에너미의 체력이 0보다 크면 피격 상태
        if(hp > 0 && m_State == EnemyState.Idle)
        {
            m_State = EnemyState.Damaged;
            print("Any State -> Damaged");
            
           // 피격 애니메이션 
           anim.SetTrigger("Damaged");
            
        }
        
        
        

    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다
        yield return new WaitForSeconds(1f);

        
        //현재 상태를 이동 상태로 전환 
        m_State = EnemyState.Idle;
       // print("Damaged -> Move");

    }

    void Die()
    {
       
        //죽음 상태 처리
        StartCoroutine(DieProcess());
        itemPool.GetItem(transform.position);
       
        
    }

    IEnumerator DieProcess()
    {
        
        //캐릭터컨트롤러 비활성화
       // cc.enabled = false;
        
        //움직임이 끝나고 die애니메이션 진행 Rebind()로 해결 
        anim.Rebind();

        anim.SetTrigger("Die");
    
        //N초후 제거
        yield return new WaitForSeconds(10f);

    }

    

    public void Respawn()
    {
        // 적을 초기 위치로 이동시키고 체력을 회복하며 다시 활성화
        transform.position = originPos;
        transform.rotation = originRot;
        hp = maxHp;
        m_State = EnemyState.Idle;
        anim.Play("Idle");
        //anim.SetTrigger("DieToIdle");
        //gameObject.SetActive(true);
       // print("Respawn");
    }



    void RouteReset()
    {
        //내비게이션 에이전트의 이동을 멈추고 경로를 초기화 
        smith.isStopped = true; 
        smith.ResetPath();

    }
}
