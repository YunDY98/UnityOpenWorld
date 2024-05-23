using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Palmmedia.ReportGenerator.Core;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats _instance;

    public static PlayerStats playerStats
    {
        get
        {

            return _instance;
        }
    }
    public enum SelectCharacter
    {
        
        MasaSchool,
        Solider,

    }

   

    public SelectCharacter selectCharacter;

    public PlayerMove playerMove;

    public GameObject aim;
    
    public int level;
    public int exp;
    public int gold;

    public GameObject[] characterMode;

    public Transform camPos;

    private int selectedIndex;

    
    public int sceneNumber;

    public Slider expSlider;


    private int maxExp = 10000;

    public float x,y,z;

    public TextMeshProUGUI textgold;
    public TextMeshProUGUI textLevel;


    void Awake()
    {   
        // 이미 인스턴스가 존재한다면 파괴합니다.
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        selectedIndex = 0;
        
        SetActiveCharacter((int)selectCharacter);
        if(Client.client != null)
        {
            SetLevel(Client.client.level);
            SetExp(Client.client.exp); 
            SetGold(Client.client.gold);


        }
        else
        {
            SetLevel(1);
            SetExp(1);
            SetGold(1000);
        }
       
    }

    public void SetLevel(int _level)
    {
        level = _level;
        textLevel.text = level.ToString();
        maxExp = maxExp + (level*1000);
    }

    public void SetGold(int _gold)
    {
        gold = _gold;
        textgold.text = gold.ToString();
    }

    public void SetExp(int _exp)
    {
        exp = _exp;
        expSlider.value = (float)exp/(float)maxExp;
    }

    public void SetSceneNumber(int _sceneNumber)
    {
        sceneNumber = _sceneNumber;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 솔저로 
            SetActiveCharacter((int)SelectCharacter.Solider);
            
            camPos.localPosition = new Vector3(0.05f,0.5f,0.3f);
            playerMove.CharacterReset();
            
           
        }
        // 마사 캐릭터로 
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            SetActiveCharacter((int)SelectCharacter.MasaSchool);
            camPos.localPosition = new Vector3(-0.03f,0.45f,-0.8f);
            
            playerMove.CharacterReset();
            
            
        }

        if(gold <= 30)
        {
            gold += 30;
        }


       
    }

    // 골드 획득시
    public void AddGold(int _gold)
    {
        gold += _gold;
        if(Client.client != null)
            Client.client.UserStats(level,gold,exp);
        textgold.text = gold.ToString();
    }

    //골드 사용시 
    public bool UseGold(int _use)
    {
        if(0 > gold - _use)
            return false;

        gold -= _use;
        textgold.text = gold.ToString();
        if(Client.client != null)
            Client.client.UserStats(level,gold,exp);

        return true;
    }

    //경험치 획득시 
    public void AddExp(int _exp)
    {
        exp += _exp;
        if(Client.client != null)
            Client.client.UserStats(level,gold,exp);

        LevelUp();

        expSlider.value = (float)exp/(float)maxExp;

    }

    // 레벨업시 
    private void LevelUp()
    {
        
        if(exp >= maxExp)
        {
            
            exp -= maxExp;
            level++;

            if(Client.client != null)
                Client.client.UserStats(level,gold,exp);

            maxExp += level*1000;
            textLevel.text = level.ToString();
            expSlider.value = (float)exp/(float)maxExp;
            AddGold(level*1000);


        }
    }

    // 현재 고른 캐릭터 
    public void SetActiveCharacter(int _index)
    {
        aim.SetActive(false);
        // 배열에 있는 모든 오브젝트를 비활성화
        for (int i = 0; i < characterMode.Length; i++)
        {
            characterMode[i].SetActive(false);
           
        }

        // 선택된 인덱스가 유효한 경우 해당 오브젝트를 활성화
        if (_index >= 0 && _index < characterMode.Length)
        {
            characterMode[_index].SetActive(true);
            selectedIndex = _index;

            if((int)SelectCharacter.Solider == _index)
            {
                aim.SetActive(true);
            }
           
            
        }
    }

   
}
