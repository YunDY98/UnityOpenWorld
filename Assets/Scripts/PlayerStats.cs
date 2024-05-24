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



    private PlayerData playerData;
    
    public int level;
    public int exp;
    public int gold;

    public int masaAtk1Level;
    public int masaAtk3Level;

    

    public GameObject[] characterMode;

    public Transform camPos;

    private int selectedIndex;

    
    public int sceneNumber;

    public Slider expSlider;


    private int maxExp = 10000;

    public float x,y,z;

    public TextMeshProUGUI textgold;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textMasaAtk1Level;
   
    public TextMeshProUGUI textMasaAtk3Level;


    public TextMeshProUGUI textMasaAtk1LevelUpGold;
    public TextMeshProUGUI textMasaAtk3LevelUpGold;


    


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
       
        SetPlayerData();
        
        
        
       
    }

    //데이터 불러오기 
    public void SetPlayerData()
    {
        playerData = DataManager.dataManager.LoadPlayerData();

        if(playerData != null)
        {
            SetLevel(playerData.level);
            SetExp(playerData.exp); 
            SetGold(playerData.gold);
            masaAtk1Level = GetSkillLevel("Masa","Atk1",playerData.skills);
            masaAtk3Level = GetSkillLevel("Masa","Atk3",playerData.skills);


        }
        else
        {

            SetLevel(1);
            SetExp(0);
            SetGold(1000);
            masaAtk1Level = 1;
            masaAtk3Level = 1;

        }    

    }
    public void UpdateSkillText()
    {
        SetSkillLevel(ref masaAtk1Level,ref textMasaAtk1Level,ref textMasaAtk1LevelUpGold);
        SetSkillLevel(ref masaAtk3Level,ref textMasaAtk3Level, ref textMasaAtk3LevelUpGold);

    }
    
    public void Atk1SkillLevelUp()
    {
        SetSkillLevel(ref masaAtk1Level,ref textMasaAtk1Level,ref textMasaAtk1LevelUpGold,true);
        

    }

    public void Atk3SkillLevelUp()
    {
        SetSkillLevel(ref masaAtk3Level,ref textMasaAtk3Level, ref textMasaAtk3LevelUpGold,true);
    }

    public void SetSkillLevel(ref int _level,ref TextMeshProUGUI _textLevel,ref TextMeshProUGUI _textGold,bool _levelUp = false)
    {
        
        int _gold = (int)(500*(_level*1.1f)* (_level*2));

        if(UseGold(_gold) && _levelUp)
        {
            _level += 1;
            _textLevel.text = _level.ToString();
            _textGold.text = _gold.ToString();
        }
        else
        {
            _textLevel.text = _level.ToString();
            _textGold.text = _gold.ToString();

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
        // 솔저로 
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectCharacter = SelectCharacter.Solider;
            
            SetActiveCharacter((int)SelectCharacter.Solider);
            
            camPos.localPosition = new Vector3(0.05f,0.5f,0.3f);
            playerMove.CharacterReset();
            
           
        }
        // 마사 캐릭터로 
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectCharacter = SelectCharacter.MasaSchool;
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
       
        textgold.text = gold.ToString();
    }

    //골드 사용시 
    public bool UseGold(int _use)
    {
        if(0 > gold - _use)
            return false;

        gold -= _use;
        textgold.text = gold.ToString();
       

        return true;
    }

    //경험치 획득시 
    public void AddExp(int _exp)
    {
        exp += _exp;
       

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
    public int GetSkillLevel(string whoskill, string skillName, Skill[] skills)
    {
        // Skill 배열이 유효한지 확인합니다.
        if (skills != null)
        {
            // Skill 배열을 반복하면서 주어진 whoskill과 skillName을 가진 스킬을 찾습니다.
            foreach (Skill skill in skills)
            {
                // 현재 스킬의 whoskill과 skillName이 주어진 값과 일치하는지 확인합니다.
                if (skill.whoskill == whoskill && skill.skillName == skillName)
                {
                    // 일치하는 스킬을 찾으면 해당 스킬의 level 값을 반환합니다.
                    return skill.level;
                }
            }
        }

        // 일치하는 스킬을 찾지 못하면 기본값으로 1을 반환합니다.
        return 1;
    }

   
}
