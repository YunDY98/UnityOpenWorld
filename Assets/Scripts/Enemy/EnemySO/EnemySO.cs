using UnityEngine;




[CreateAssetMenu(fileName = "NewEnemy", menuName = "EnemySO/Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("EnemyInfo")]
    [SerializeField] private float attackDistance = 1f;
    public float AttackDistance => attackDistance;
    
    [SerializeField] private float moveSpeed = 7f;
    public float MoveSpeed => moveSpeed;
    
    [SerializeField] private float findDistance = 10f;
    public float FindDistance => findDistance;
    
    [SerializeField] private float moveDistance = 20f;
    public float MoveDistance => moveDistance;
    
    [SerializeField] private int maxHp = 15000;
    public int MaxHp => maxHp;
    
    [SerializeField] private int attackPower = 30;
    public int AttackPower => attackPower;

   

   
    
}

