using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Gold : MonoBehaviour
{
   // private PlayerStats playerStats;
    private GameObject character; // 캐릭터의 Transform
    public float attractionDistance = 10f; // 아이템이 캐릭터에게 끌려오는 거리
    private float attractionDuration = 0.1f; // 아이템이 캐릭터에게 이동하는 시간
    
    
    public int exp = 100;

    public int addGold = 30;
    private Tween moveTween;
    void Start()
    {
        character = GameObject.Find("Player");
       // playerStats = character.GetComponent<PlayerStats>();
      
    }
  
    void Update()
    {
        float distance = Vector3.Distance(transform.position, character.transform.position);
        
        if (distance <= attractionDistance)
        {
            
            {
                // DoTween을 사용하여 아이템을 캐릭터에게 이동시킴
                moveTween = transform.DOMove(character.transform.position, attractionDuration)
                     .SetEase(Ease.InQuad); // 애니메이션 곡선 설정 (원하는대로 조정 가능)
            }
        }
            
           
    }
    

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerStats.playerStats.AddGold(addGold);
            PlayerStats.playerStats.AddExp(exp);
            
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        //moveTween.Kill(); 
        transform.DOKill();
    }
  
}
