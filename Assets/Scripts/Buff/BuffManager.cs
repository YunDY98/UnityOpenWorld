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

    private PlayerMove playerMove;

    public GameObject buffPrefab; //Prefabs 폴더-> BuffImage
    public Transform contentPanel; //Hierachy -> UiManager -> Buff

    // 버프 
    Dictionary<string,GameObject> buffDic = new();

    
    //파티클 
    //ParticleSystem buffParticle;
    //public GameObject buffEffect;

    int buff;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
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
    
       // buffParticle.Play();
       
    }

    void BuffDuration(string buffName,float duration,int onBuff,float buffAmount)
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
      
       
       

        buff += onBuff;
        switch(onBuff)
        {
            case (int)Buff.CommonSpdUp:
                playerMove.moveSpeed += buffAmount;
                break;
            case (int)Buff.CommonAtkUp:
                playerStats.AtkDamage += (int)buffAmount;
                break;
        }
        // 버프 지속 시간 동안 타이머 업데이트
        Tween _Tween = DOTween.To(() => duration, x => duration = x, 0, duration);
        _Tween.OnUpdate(() => 
        {
            
            if((buff & onBuff) == 0 )
            {
                
                switch(onBuff)
                {
                    case (int)Buff.CommonSpdUp:
                        playerMove.moveSpeed -= buffAmount;
                        break;
                    case (int)Buff.CommonAtkUp:
                        playerStats.AtkDamage -= (int)buffAmount;
                        break;
                }
            
                buffDic[buffName].SetActive(false);
                
                _Tween.Kill();
                return;
                
            }
            
            TextMeshProUGUI _timeText = buffDic[buffName].GetComponentInChildren<TextMeshProUGUI>();
            _timeText.text = Mathf.CeilToInt(duration).ToString();
        })
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            // 버프 제거
            buff -= onBuff;
            switch(onBuff)
            {
                case (int)Buff.CommonSpdUp:
                    playerMove.moveSpeed -= buffAmount;
                    break;
                case (int)Buff.CommonAtkUp:
                    playerStats.AtkDamage -= (int)buffAmount;
                    break;
            }
            
            buffDic[buffName].SetActive(false);
           
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

