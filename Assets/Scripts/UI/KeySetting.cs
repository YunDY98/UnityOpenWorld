using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class KeySetting : MonoBehaviour, IDropHandler
{
    
    string key;

    string inputKey;

    int keyCode;

    int skillEnum;

    string preKey;
    Image image;

    Text text;

    GameManager gameManager;

    void Awake()
    {
        
        image = GetComponent<Image>();
        
        text = GetComponentInChildren<Text>();

        inputKey = text.text;
       
        key = PlayerPrefs.GetString(inputKey);
       
        if(key == "")
        {
            
            return;
        }
            
        
        Sprite _skillImage = Resources.Load<Sprite>("Sprites/" + key);

        preKey = key;
       
        image.sprite = _skillImage;

        keyCode = StringToEnumInt(inputKey,typeof(KeyCode));

        skillEnum = PlayerPrefs.GetInt(keyCode.ToString());

       
        GameManager.Instance.userKeys[skillEnum] = (KeyCode)keyCode;
       

        
    }
    void Start()
    {

        
        gameManager = GameManager.Instance;
       

    }

    public void OnDrop(PointerEventData eventData)
    {
        
        // 현재 드래그 중인 UI 요소 가져오기
        GameObject _dragObject = eventData.pointerDrag;

        
        if(_dragObject.TryGetComponent(out SkillInfo skillInfo))
        {
            key = skillInfo.key;
        }
        else if(_dragObject.TryGetComponent(out KeySetting keySetting))
        {
            key = keySetting.key;
        }
        else
        {
            return;
        }
      
        print("drop" + key);
         
        // 스킬 하나당 하나의 키셋팅 
        if(gameManager.userKeys[StringToEnumInt(key,typeof(SkillEnum))] != KeyCode.None)
        {
            return;
        }
            

        // 드래그 중인 UI 요소가 있다면
        if(_dragObject != null)
        {

            //이미 키세팅이 되있다면
            if(preKey != null)
            {
                // 새로 드래그된 스킬로 변경 
                gameManager.userKeys[StringToEnumInt(preKey,typeof(SkillEnum))] = KeyCode.None;
               
            }

            preKey = key;
            
            
            Sprite skillImage = Resources.Load<Sprite>("Sprites/" + key); // 이미지 파일 경로

            //이미지 정보 
            PlayerPrefs.SetString(inputKey, key);
           
            keyCode = StringToEnumInt(inputKey,typeof(KeyCode));
            //Input -> 0 
            //0~9일경우 Alpha0 -> 48로 반환 
            
            skillEnum = StringToEnumInt(key,typeof(SkillEnum));
            gameManager.userKeys[skillEnum] = (KeyCode)keyCode;

            //키세팅값 저장  
            PlayerPrefs.SetInt(keyCode.ToString(),skillEnum);

            
             
            image.sprite = skillImage;
       
        }
     
        
    }

    int StringToEnumInt(string key, Type enumType)
    {
        
        // "Alpha1"과 같은 형태의 문자열을 생성
        if (int.TryParse(key, out int numericKey) && numericKey >= 0 && numericKey <= 9)
        {
            key = "Alpha" + key;
        }
        
        object _enumValue = System.Enum.Parse(enumType, key);

        
        
            
        return (int)_enumValue;
    }

    // int StringToKeyCode(string _key)
    // {

    //     KeyCode _enum = (KeyCode)System.Enum.Parse(typeof(KeyCode), _key);

    //     return (int)_enum;
    // }

    // int StringToSkillEnum(string _key)
    // {
    //     SkillEnum _enum = (SkillEnum)System.Enum.Parse(typeof(SkillEnum), _key);


    //     return (int)_enum;
    // }
    // 키셋팅된 단축키 변경 
    public void Drop()
    {
        
        text = GetComponentInChildren<Text>();

        inputKey = text.text;
       
       
        if(image.sprite == null)
            return;
        
        
        image.sprite = null;
        preKey = null;
        
        gameManager.userKeys[StringToEnumInt(key,typeof(SkillEnum))] = KeyCode.None;
        PlayerPrefs.DeleteKey(inputKey);
        PlayerPrefs.DeleteKey(keyCode.ToString());
           
    }

     
}
public enum SkillEnum
{
    
    none = 0,
    MasaAtk1,
    MasaAtk2,
    MasaAtk3,
    CommonSpdUp,
    CommonAtkUp,
    HPPotion,



}