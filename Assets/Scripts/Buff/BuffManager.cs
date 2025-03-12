using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using Yun;
//Hierarchy -> PlayerStats-> Player에 스크립트 할당
public class BuffManager : MonoBehaviour
{
    
    PlayerStats playerStats;
    GameManager gameManager;

    public GameObject buffPrefab; //Prefabs 폴더-> BuffImage
    public Transform contentPanel; //Hierachy -> UiManager -> Buff

    BuffPool buffPool;
    // 버프 
    Dictionary<string,GameObject> buffUIDic = new();
    Dictionary<string,IBuff> buffEffDic = new();

    //파티클 
    //ParticleSystem buffParticle;
    //public GameObject buffEffect;

    int buff;
    

    // Start is called before the first frame update
    void Start()
    {
        buffPool = new ();
        buffEffDic = buffPool.GetBuffEffDic();
       
        playerStats = PlayerStats.Instance;
        gameManager = GameManager.Instance;
        //buffParticle = buffEffect.GetComponent<ParticleSystem>();

        buff = 0;

        
    }

    // Update is called once per frame
    void Update()
    {
        
        //공용 스킬 
        if(Input.GetKeyUp(gameManager.userKeys[(int)SkillEnum.CommonSpdUp]))
        {
            UseBuff("CommonSpdUp");
        }

        if(Input.GetKeyUp(gameManager.userKeys[(int)SkillEnum.CommonAtkUp]))
        {
            UseBuff("CommonAtkUp");
        }
       
    }


    void UseBuff(string buffName)
    {
        
        int _onBuff = GameUtility.StringToEnumInt(buffName,typeof(Buff));

        if((buff & _onBuff) != 0 )
        {
            
            //버프 삭제 
            buff -= _onBuff;
            return;
        }

        int _skillLevel = playerStats.GetSkillLevel(buffName);

        if(_skillLevel < 0)
        {
            return;
        }
        
        if(playerStats.UseGold(_skillLevel))
        {   
            
           
            BuffUI(buffName);
            BuffOnOff(buffName,_onBuff);
            
        }
    
       // buffParticle.Play();
       
    }

    void BuffOnOff(string buffName,int onBuff)
    {
        IBuff _IBuff = buffEffDic[buffName];
        _IBuff.Apply();

        buff += onBuff;

        float _duration = _IBuff.Duration;
  
        // 버프 지속 시간 동안 타이머 업데이트
        Tween _Tween = DOTween.To(() => _duration, x => _duration = x, 0, _duration);
        TextMeshProUGUI _timeText = buffUIDic[buffName].GetComponentInChildren<TextMeshProUGUI>();
        _Tween.OnUpdate(() => 
        {
            
            if((buff & onBuff) == 0 )
            {
                _Tween.Kill();
                _IBuff.Remove();
            
                buffUIDic[buffName].SetActive(false);
                
                
                return;
                
            }
            
           
            _timeText.text = Mathf.CeilToInt(_duration).ToString();
        })
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
           
            _Tween.Kill();
            // 버프 제거
            buff -= onBuff;
            
            buffUIDic[buffName].SetActive(false);
            _IBuff.Remove();
           
        });

       
    }
   
    void BuffUI(string buffName)
    {
        if(!buffUIDic.ContainsKey(buffName))
        {
            GameObject _buffWindow = Instantiate(buffPrefab,contentPanel);
            Image _imageComponent = _buffWindow.GetComponent<Image>();

            Sprite _buffSprite;

            _buffSprite = Resources.Load<Sprite>("Sprites/" + buffName); // 이미지 파일 경로

            _imageComponent.sprite = _buffSprite;

            buffUIDic.Add(buffName,_buffWindow);
        }
        else
        {
            buffUIDic[buffName].SetActive(true);
        }
    }

    enum Buff
    {
        none = 0,
        CommonAtkUp = 1 << 0,
        CommonSpdUp = 1 << 1,


    }
}

