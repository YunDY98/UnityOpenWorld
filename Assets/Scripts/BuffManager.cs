using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    
    PlayerStats playerStats;
    GameManager gameManager;

    public PlayerMove playerMove;

    int buff;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.playerStats;
        gameManager = GameManager.gameManager;

        

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
       

        print("Move Speed = " + playerMove.moveSpeed);
        print("Atk Up = " + playerStats.AtkDamage);

        
    }

    void UseBuff(string _buffName,float _durationMult = 10f,float _buffAmount = 1f)
    {
        int _onBuff = StringToEnum(_buffName,typeof(Buff));
        if((buff & _onBuff) != 0 )
        {
            //버프가 켜져있슴
            print("버프가 켜져 있음");
            return;
        }
        
            

        int _skillLevel = playerStats.GetSkillLevel(_buffName);

        if(_skillLevel < 0)
        {
            return;
        }
        
       

        if(playerStats.UseGold((int)(_skillLevel)))
        {   
            float _duration = _skillLevel * _durationMult;
            float _Amount = _skillLevel * _buffAmount;
            
            StartCoroutine(BuffDurationCoroutine(_duration,_onBuff,_Amount));
            
        }
    

       
       
       
        
    }

    IEnumerator BuffDurationCoroutine(float _duration,int _onBuff,float _buffAmount)
    {
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

        float _elapsedTime = 0f;

        while(_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

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

