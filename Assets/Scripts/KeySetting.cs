
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class KeySetting : MonoBehaviour, IDropHandler
{
    
    string key;


    
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
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 현재 드래그 중인 UI 요소 가져오기
        GameObject draggingObject = eventData.pointerDrag;

        //이미 키세팅이 되있다면 리턴 
        if(image.sprite != null)
            return;

        // 드래그 중인 UI 요소가 있다면
        if (draggingObject != null)
        {
            
            key = draggingObject.GetComponent<SkillInfo>()._key;
            Sprite skillImage = Resources.Load<Sprite>("Sprites/" + key); // 이미지 파일 경로
            
            image.sprite = skillImage;

            string _text = text.text;

            int _KeyCode = (int)_text[0];
            
            
            
            GameManager.gameManager.userKeys[StringToEnum(key)] = (KeyCode)_KeyCode;
               
            
            
            
            
        }
        
        
    }

    int StringToEnum(string _key)
    {
        SkillEnum _enum = (SkillEnum)System.Enum.Parse(typeof(SkillEnum), _key);


        return (int)_enum;
    }
    // 키셋팅된 단축키 변경 
    public void Drop()
    {

        // 현재 클릭한 시간
        float currentTime = Time.time;

        // 현재 시간과 마지막으로 클릭한 시간의 차이가 더블 클릭 감지 시간보다 짧으면 더블 클릭으로 처리
        if (currentTime - lastClickTime < doubleClickTime)
        {
            // 더블 클릭 이벤트 처리
            Debug.Log("Double click!");
            image.sprite = null;
            GameManager.gameManager.userKeys[StringToEnum(key)] = KeyCode.None;
           
        }

        // 마지막으로 클릭한 시간 갱신
        lastClickTime = currentTime;
       
    }

     
}
public enum SkillEnum
{
    // 0~49 공격
    // 50~100 버프
    // 그외 
    none = 0,
    MasaAtk1,
    MasaAtk2,
    MasaAtk3,


}