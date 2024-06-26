using UnityEngine;
using DG.Tweening;
public class ItemMove : MonoBehaviour
{
    
    private GameObject character; // 캐릭터의 Transform
    public float attractionDistance = 10f; // 아이템이 캐릭터에게 끌려오는 거리
    private float attractionDuration = 0.1f; // 아이템이 캐릭터에게 이동하는 시간
    private Tween moveTween;
    void Start()
    {
        character = GameObject.Find("Player");
      
      
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

    void OnDestroy()
    {
        //moveTween.Kill(); 
        transform.DOKill();
    }
}
