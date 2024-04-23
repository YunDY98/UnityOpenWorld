using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float findDistance = 8f;

    public int attackPower = 3;

    public CharacterController cc;

    PlayerMove playerMove;


    //누적 시간 
    public float currentTime = 0;
    public float attackDelay = 2f;
    // 에너미 상태 변수
    EnemyState m_State;

    // 플레이어 발견 범위
    
    //플레이어 트랜스폼 
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle;

        //player = GameObject.Find("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:  
                Idle();
                break;
            case EnemyState.Move:
               // Move();
                break;
            case EnemyState.Attack:
                //Attack();
                break;
            case EnemyState.Return:
              //  Return();
                break;
            case EnemyState.Damaged:
               // Damaged();
                break;
            case EnemyState.Die:
              //  Die();
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
        }
    }

    void Move()
    {
        // 플레이어와의 거리가 공격 범위 밖이라면 플레이러를 향해 이동
        if(Vector3.Distance(transform.position,player.position) > attackDistance)
        {
            //이동 방향 설정 
            Vector3 dir = (player.position - transform.position).normalized;    

            //캐릭터 컨트롤러 이용하여 이동 
            cc.Move(dir * moveSpeed * time.deltaTime);



        }
        else
        {
            m_State = EnemyState.Attack;
            print("Move -> Attack")
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
                playerMove.DamageAction(attackPower);
                
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
}
