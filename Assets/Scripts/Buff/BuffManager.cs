using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
//Hierarchy -> PlayerStats-> Player에 스크립트 할당
public class BuffManager : MonoBehaviour
{
    
    PlayerStats playerStats;
    GameManager gameManager;

    public GameObject buffPrefab; //Prefabs 폴더-> BuffImage
    public Transform contentPanel; //Hierachy -> UiManager -> Buff

    // 버프 
    Dictionary<string,GameObject> buffDic = new();
    Dictionary<string,IBuff> buffEffectDic = new();
    

    
    //파티클 
    //ParticleSystem buffParticle;
    //public GameObject buffEffect;

    int buff;
    // Start is called before the first frame update
    void Start()
    {
        
        playerStats = PlayerStats.Instance;
        gameManager = GameManager.Instance;
        //buffParticle = buffEffect.GetComponent<ParticleSystem>();

        buff = 0;

        buffEffectDic.Add("CommonSpdUp",new SpeedBuff());
        buffEffectDic.Add("CommonAtkUp",new AttackBuff());
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
        int _onBuff = StringToEnum(buffName,typeof(Buff));

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
            
           
            
            BuffDuration(buffName,_onBuff);

        }

       
        print(playerStats.GetSkillLevel(buffName));
    
       // buffParticle.Play();
       
    }

    void BuffDuration(string buffName,int onBuff)
    {
        if(!buffDic.ContainsKey(buffName))
        {
            GameObject _buffWindow = Instantiate(buffPrefab,contentPanel);
            Image _imageComponent = _buffWindow.GetComponent<Image>();
        
            

            Sprite _buffSprite;

            _buffSprite = Resources.Load<Sprite>("Sprites/" + buffName); // 이미지 파일 경로

            _imageComponent.sprite = _buffSprite;

            buffDic.Add(buffName,_buffWindow);
        }
        else
        {
            buffDic[buffName].SetActive(true);
        }
      
       float _duration = buffEffectDic[buffName].Duration;
       

        buff += onBuff;

        buffEffectDic[buffName].Apply();
    
        // 버프 지속 시간 동안 타이머 업데이트
        Tween _Tween = DOTween.To(() => _duration, x => _duration = x, 0, _duration);
        TextMeshProUGUI _timeText = buffDic[buffName].GetComponentInChildren<TextMeshProUGUI>();
        _Tween.OnUpdate(() => 
        {
            
            if((buff & onBuff) == 0 )
            {
                _Tween.Kill();
                buffEffectDic[buffName].Remove();
            
                buffDic[buffName].SetActive(false);
                
                
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
            
            buffDic[buffName].SetActive(false);
            buffEffectDic[buffName].Remove();
           
        });

       
    }
    int StringToEnum(string key, Type enumType)
    {
        
        object _enumValue = System.Enum.Parse(enumType, key);

        return (int)_enumValue;
    }


    enum Buff
    {
        none = 0,
        CommonAtkUp = 1 << 0,
        CommonSpdUp = 1 << 1,


    }
}

