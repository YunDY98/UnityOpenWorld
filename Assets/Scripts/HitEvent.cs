using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    public EnemyFSM efsm;
    
    //플레이어 데미지 
    public void PlayHit()
    {
        efsm.AttackAction();
    }
}
