using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BuffManager : MonoBehaviour
{
    
    PlayerStats playerStats;
    GameManager gameManager;

    public PlayerMove playerMove;

    public GameObject buffPrefab;
    public Transform contentPanel;

    // 버프 
    Dictionary<string,Sprite> spriteDic = new();

    
    //파티클 
    ParticleSystem buffParticle;
    public GameObject buffEffect;

    int buff;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.playerStats;
        gameManager = GameManager.gameManager;
        buffParticle = buffEffect.GetComponent<ParticleSystem>();

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



    void UseBuff(string buffName,float durationMult = 2f,float buffAmount = 0.5f)
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
        
     
       

        if(playerStats.UseGold((int)(_skillLevel)))
        {   
            float _duration = _skillLevel * durationMult;
            float _amount = _skillLevel * buffAmount;
            
            BuffDuration(buffName,_duration,_onBuff,_amount);

        }
    
        buffParticle.Play();
       
       
        
    }

    void BuffDuration(string _buffName,float _duration,int _onBuff,float _buffAmount)
    {
        
        GameObject _buffWindow = Instantiate(buffPrefab,contentPanel);
        Sprite _buffSprite;
        // 이미지 로드 및 할당
        if(!spriteDic.ContainsKey(_buffName))
        {
            _buffSprite = Resources.Load<Sprite>("Sprites/" + _buffName); // 이미지 파일 경로
          
            spriteDic.Add(_buffName, _buffSprite);
        }
        else
        {
            _buffSprite = spriteDic[_buffName];
        }
       
       
        Image imageComponent = _buffWindow.GetComponent<Image>();
        
        TextMeshProUGUI timeText = _buffWindow.GetComponentInChildren<TextMeshProUGUI>();
       
        
       
       
        if(imageComponent != null)
        {
            if(_buffSprite != null)
            {
                imageComponent.sprite = _buffSprite;
            }
        }



        buff += _onBuff;
        switch(_onBuff)
        {
            case (int)Buff.CommonSpdUp:
                playerMove.moveSpeed += _buffAmount;
                break;
            case (int)Buff.CommonAtkUp:
                playerStats.AtkDamage += (int)_buffAmount;
                break;
                
            

        }
        // 버프 지속 시간 동안 타이머 업데이트
        Tween _Tween = DOTween.To(() => _duration, x => _duration = x, 0, _duration);
        _Tween.OnUpdate(() => 
        {
            
            if((buff & _onBuff) == 0 )
            {
                
                switch(_onBuff)
                {
                    case (int)Buff.CommonSpdUp:
                        playerMove.moveSpeed -= _buffAmount;
                        break;
                    case (int)Buff.CommonAtkUp:
                        playerStats.AtkDamage -= (int)_buffAmount;
                        break;
                }
            
                Destroy(_buffWindow);
                
                _Tween.Kill();
                return;
                
            }
            
            
            timeText.text = Mathf.CeilToInt(_duration).ToString();
        })
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            // 버프 제거
            buff -= _onBuff;
            switch(_onBuff)
            {
                case (int)Buff.CommonSpdUp:
                    playerMove.moveSpeed -= _buffAmount;
                    break;
                case (int)Buff.CommonAtkUp:
                    playerStats.AtkDamage -= (int)_buffAmount;
                    break;
            }
            
            Destroy(_buffWindow);
           
        });

       
    }
    int StringToEnum(string _key, Type _enumType)
    {
        
        object _enumValue = System.Enum.Parse(_enumType, _key);

        
        
            
        return (int)_enumValue;
    }

    
    

    enum Buff
    {
        none = 0,
        CommonAtkUp = 1 << 0,
        CommonSpdUp = 1 << 1,


    }
}

