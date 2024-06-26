using UnityEngine;

public class EnemyEvent : MonoBehaviour
{
    public EnemyFSM efsm;
    
    //플레이어 데미지 
    public void PlayHit()
    {
        efsm.AttackAction();
    }
    public void Respawn()
    {
        efsm.Respawn();
    }
   
}
