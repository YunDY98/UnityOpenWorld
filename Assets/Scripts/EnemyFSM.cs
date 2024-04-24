using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
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
    public float attackDistance = 2f;
    public float moveSpeed = 5f;

    // 플레이어 발견 범위
    public float findDistance = 8f;

    public int hp = 15;
    int maxHp = 15;

    public Slider hpSlider;
    public int attackPower = 20;

    //초기 위치 
    Vector3 originPos;
    Quaternion originRot;

    //이동 가능 범위
    public float moveDistance = 20f;


    public CharacterController cc;

    public PlayerMove pm;

    public Animator anim;
    //누적 시간 
    public float currentTime = 0;
    public float attackDelay = 2f;
    // 에너미 상태 변수
    EnemyState m_State;

   
    //플레이어 트랜스폼 
    public Transform player;

    
    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle;

        //자신의 초기 위치 
        originPos = transform.position;
        originRot = transform.rotation;

       // player = GameObject.Find("Player").transform;

      // anim = transform.GetComponentInchildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
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
               // Die();
                break;


                


        }
        
    }

    void Idle()
    {
        // 플레이어와 거리가 액션 시작범위 이내라면 move 상태로 전환
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("Idle -> Move");

            //이동 애니메이션
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {

        //초기 위치에서 이동 가능 범위를 넘어간다면 
        if(Vector3.Distance(transform.position,originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            print("Move -> Return");
        }
        // 플레이어와의 거리가 공격 범위 밖이라면 플레이러를 향해 이동
        else if(Vector3.Distance(transform.position,player.position) > attackDistance)
        {
            //이동 방향 설정 
            Vector3 dir = (player.position - transform.position).normalized;    

            //캐릭터 컨트롤러 이용하여 이동 
            cc.Move(dir * moveSpeed * Time.deltaTime);

            //플레이어를 향해 방향 전환 
            transform.forward = dir;



        }
        else
        {
            m_State = EnemyState.Attack;
            print("Move -> Attack");
            currentTime = attackDelay;
        }
    }

    void Attack()
    {
        // 플레이어가 공격 범위 이내에 있다면 공격 
        if(Vector3.Distance(transform.position,player.position) < attackDistance)
        {
            //일정한 시간 마다 플레이러를 공격
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                print("attack");

                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
               
                pm.DamageAction(attackPower);
                currentTime = 0;

            }

        }
        else
        {
            m_State = EnemyState.Move;
            print("Attack -> Move");
            currentTime = attackDelay;

        }
    }

    void Return()
    {
        if(Vector3.Distance(transform.position,originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * currentTime* Time.deltaTime);

            //방향을 복귀 지점으로 
            transform.forward = dir;

        }
        else
        {
            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxHp;
            m_State =  EnemyState.Idle;
            print("Return -> Idle");

            anim.SetTrigger("MoveToIdle");

        }
    }

    public void HitEnemy(int hitPower)
    {
        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }
        //플레이어의 공격력만큼 에너미 체력 감소 
        hp -= hitPower;

        //에너미의 체력이 0보다 크면 피격 상태
        if(hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("Any State -> Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("Any State -> Die");
            Die();
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다
        yield return new WaitForSeconds(0.5f);

        //현재 상태를 이동 상태로 전환 
        m_State = EnemyState.Move;
        print("Damaged -> Move");

    }

    void Die()
    {
        StopAllCoroutines();

        //죽음 상태 처리
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        //캐릭터컨트롤러 비활성화
        cc.enabled = false;

        //2초후 제거
        yield return new WaitForSeconds(2f);
        print("소멸");
        Destroy(gameObject);
    }
}
