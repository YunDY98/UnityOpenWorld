
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class KeySetting : MonoBehaviour, IDropHandler
{
    
    string key;

    string sText;

    int keyCode;

    int skillEnum;

    string preKey;
    Image image;

    Button btn;

    Text text;


    private float doubleClickTime = 0.3f; // 더블 클릭 감지를 위한 시간 간격
    private float lastClickTime = 0f; // 마지막으로 클릭한 시간

    void Start()
    {
        image = GetComponent<Image>();
        btn = GetComponent<Button>();

        btn.onClick.AddListener(Drop);

        text = GetComponentInChildren<Text>();

        sText = text.text;
        key = PlayerPrefs.GetString(sText);

        if(key == "")
            return;
        
        Sprite _skillImage = Resources.Load<Sprite>("Sprites/" + key);

        preKey = key;
       
        image.sprite = _skillImage;

        keyCode = StringToEnum(sText,typeof(KeyCode));

        skillEnum = PlayerPrefs.GetInt(keyCode.ToString());

        GameManager.gameManager.userKeys[skillEnum] = (KeyCode)keyCode;




    

      
        

        
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 현재 드래그 중인 UI 요소 가져오기
        GameObject _dragObject = eventData.pointerDrag;

        key = _dragObject.GetComponent<SkillInfo>()._key;



        
        

       
         
       
        // 스킬 하나당 하나의 키셋팅 
        if(GameManager.gameManager.userKeys[StringToEnum(key,typeof(SkillEnum))] != KeyCode.None)
            return;


        

        // 드래그 중인 UI 요소가 있다면
        if(_dragObject != null)
        {

            //이미 키세팅이 되있다면
            if(preKey != null)
            {
                // 새로 드래그된 스킬로 변경 
                GameManager.gameManager.userKeys[StringToEnum(preKey,typeof(SkillEnum))] = KeyCode.None;
               
            }

            preKey = key;
            
            
            Sprite skillImage = Resources.Load<Sprite>("Sprites/" + key); // 이미지 파일 경로

            

            //이미지 정보 
            PlayerPrefs.SetString(sText, key);

            keyCode = StringToEnum(sText,typeof(KeyCode));
            skillEnum = StringToEnum(key,typeof(SkillEnum));
            GameManager.gameManager.userKeys[skillEnum] = (KeyCode)keyCode;

            

            
            // 키코드 
            PlayerPrefs.SetInt(keyCode.ToString(),skillEnum);

            
             
            image.sprite = skillImage;

            
               
        }
        
        
    }

    int StringToEnum(string _key, Type _enumType)
    {
        object _enumValue = System.Enum.Parse(_enumType, _key);
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

        // 현재 클릭한 시간
        float currentTime = Time.realtimeSinceStartup; // time.scale = 0 이므로 realtime

        // 현재 시간과 마지막으로 클릭한 시간의 차이가 더블 클릭 감지 시간보다 짧으면 더블 클릭으로 처리
        if (currentTime - lastClickTime < doubleClickTime)
        {
            print(currentTime +" - " +lastClickTime);
            if(image.sprite == null)
                return;
            // 더블 클릭 이벤트 처리
            Debug.Log("Double click!");
            image.sprite = null;
            preKey = null;
            GameManager.gameManager.userKeys[StringToEnum(key,typeof(SkillEnum))] = KeyCode.None;

            PlayerPrefs.DeleteKey(sText);
            PlayerPrefs.DeleteKey(keyCode.ToString());
           
        }

        // 마지막으로 클릭한 시간 갱신
        lastClickTime = currentTime;
       
    }

     
}
public enum SkillEnum
{
    // 1~50 공격
    // 51~100 버프
    // 그외 
    none = 0,
    MasaAtk1,
    MasaAtk2,
    MasaAtk3,


}