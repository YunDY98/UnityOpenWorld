using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MasaSchool : MonoBehaviour
{
    Animator anim;
    
    
    private List<GameObject> enemies = new List<GameObject>(); // 적 배열




    

    private int attack1 = 30;
    private int attack2 = 100;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameManager.gState != GameManager.GameState.Run || PlayerStats.playerStats.selectCharacter != PlayerStats.SelectCharacter.MasaSchool)
        {
            
            return;
        }

        if(GameManager.gameManager.isMove)
        {
            if(Input.GetKeyUp(KeyCode.Alpha2))
            {
            
                //StartCoroutine(Emotion(2f,"emo2"));

            }

            if(Input.GetKeyUp(KeyCode.C))
            {
                
                if(PlayerStats.playerStats.UseGold(attack1))
                {
                    anim.SetTrigger("Attack1");
                    
                   
                    Attack(10f,10,10);

                }



            }

            if(Input.GetKeyUp(KeyCode.X))
            {
                
                if(PlayerStats.playerStats.UseGold(attack2))
                {
                    anim.SetTrigger("Attack3");
                    
                    Attack(0.7f,1,100);
                   

                }



            }

        }

       

        
    }

   

 

    void Attack(float _range,int _cnt,int _damage)
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

        // 가까운 적만 선택하여 공격
        int enemiesAttacked = 0;
        foreach (GameObject enemy in enemies)
        {
            if (enemiesAttacked < _cnt)
            {
                // EnemyFSM 컴포넌트 가져오기
                EnemyFSM efsm = enemy.GetComponent<EnemyFSM>();
                if (efsm != null)
                {
                    efsm.HitEnemy(_damage);
                    enemiesAttacked++;
                }
            }
            else
            {
                break; // 최대 공격수에 도달하면 루프 종료
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
