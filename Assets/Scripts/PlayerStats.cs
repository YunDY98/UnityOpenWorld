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
        Solider,
        MasaSchool,

    }

    public SelectCharacter selectCharacter;

    public PlayerMove playerMove;
    
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
            SetGold(100000);
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
            selectCharacter = SelectCharacter.Solider;
            SetActiveCharacter((int)selectCharacter);
            camPos.localPosition = new Vector3(0.05f,0.5f,0.3f);
            playerMove.CharacterReset();
          
           
        }
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectCharacter = SelectCharacter.MasaSchool;
            SetActiveCharacter((int)selectCharacter);
            camPos.localPosition = new Vector3(-0.03f,0.45f,-0.8f);
            playerMove.CharacterReset();
            
            
        }


       
    }

    
    public void AddGold()
    {
        gold += 30;
        if(Client.client != null)
            Client.client.UserStats(level,gold,exp);
        textgold.text = gold.ToString();
    }

    public void UseGold(int _use)
    {
        if(0 > gold - _use)
            return;

        gold -= _use;
        textgold.text = gold.ToString();
        if(Client.client != null)
            Client.client.UserStats(level,gold,exp);
    }

    public void AddExp(int _exp)
    {
        exp += _exp;
        if(Client.client != null)
            Client.client.UserStats(level,gold,exp);

        LevelUp();

        expSlider.value = (float)exp/(float)maxExp;

    }

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


        }
    }

    public void SetActiveCharacter(int index)
    {
        // 배열에 있는 모든 오브젝트를 비활성화합니다.
        for (int i = 0; i < characterMode.Length; i++)
        {
            characterMode[i].SetActive(false);
           
        }

        // 선택된 인덱스가 유효한 경우 해당 오브젝트를 활성화합니다.
        if (index >= 0 && index < characterMode.Length)
        {
            characterMode[index].SetActive(true);
            selectedIndex = index;
            
        }
    }

   
}
