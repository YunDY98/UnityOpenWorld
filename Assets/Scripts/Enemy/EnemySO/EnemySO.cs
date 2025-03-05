using UnityEngine;




[CreateAssetMenu(fileName = "NewEnemy", menuName = "EnemySO/Enemy")]
public class EnemySO : ScriptableObject
{
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

     //공격력 
    public int attackPower = 30;

   

   
    
}

