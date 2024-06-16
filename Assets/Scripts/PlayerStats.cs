using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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
   
    public SelectCharacter selectCharacter;

    public PlayerMove playerMove;

    public GameObject aim;
    public GameObject goldShortage;

    GameManager gm = GameManager.gameManager;

    public PlayerData playerData;
    
    private int _level;
    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            textLevel.text = _level.ToString();
            maxExp += (_level*1000);
        }
    }
    private int _exp;
    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;
            expSlider.value = (float)_exp/(float)maxExp;
           
        }
    }

    //보유 중인 골드 
    private int _gold;
    public int Gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            textGold.text = _gold.ToString();
           
        }
    }

    //골드 사용량 
    private int _goldUsage = 10;
    public int GoldUsage
    {
        get { return _goldUsage; }
        set
        {
            _goldUsage = value; 
        }
    }

    //기본 공격력

    private int _atkDamage = 10;
    public int AtkDamage
    {
        
        get { return _atkDamage; }

        set
        {
            _atkDamage = value;
        }
    }

    

    public GameObject[] characterMode;

    public Transform camPos;

    private int selectedIndex;

    
    public int sceneNumber;

    public Slider expSlider;


    private int maxExp = 10000;


    public TextMeshProUGUI textGold;
    public TextMeshProUGUI textLevel;
   
    // 스킬 Ui
    public GameObject skillPrefab;
    //스킬 담을 패널 
    public Transform contentPanel;

    

    //스킬 정보가 담긴딕셔너리
    public Dictionary<string, Skill> skillDictionary = new();
    //스킬 Ui패널에 추가된 스킬들 
    public Dictionary<string, GameObject> skillObjectDictionary = new();
    //무기 정보 
    public Dictionary<SelectCharacter, int> weaponDictionary = new();
   
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


    // 스크립트 실행 순서 조절 가능
    void Start()
    {  
        playerData = DataManager.dataManager.LoadPlayerData();

       
       
        //활성화된 캐릭터 
        selectedIndex = 0;
        SetActiveCharacter((int)selectCharacter);

        if(playerData == null)
        {
            Level = 1;
            Exp = 0;
            Gold = 10000;
            AtkDamage += (int)(Level * 1.1f);

            foreach(var _weapon in playerData.weapons)
            {
                SetWeapon(_weapon);
            }

            // skills 배열에 저장된 스킬
            foreach(var _skill in playerData.skills)
            {
                SetSkill(new Skill(_skill.whoSkill, _skill.skillName, _skill.level));
               
            }
            
        }
        else
        {
             foreach(SelectCharacter _weapon in Enum.GetValues(typeof(SelectCharacter)))
             {
                
                if(!weaponDictionary.ContainsKey(_weapon))
                {
                    //weaponDictionary.Add(_weapon,1);
                    weaponDictionary[_weapon] = 1;
                }
             }

        }

       
        CreateSkill();
        SetPlayerData();
       
        

       
        AtkDamage += (int)(Level * 1.1f);
       
    }

    //데이터 불러오기 
    public void SetPlayerData()
    {
        

        if(playerData != null)
        {
            Level = playerData.level;
            //SetLevel(playerData.level);
            //SetExp(playerData.exp); 
            Exp = playerData.exp;
            //SetGold(playerData.gold);
            Gold = playerData.gold;
            // masaAtk1Level = GetSkillLevel("Masa","Atk1",playerData.skills);
            // masaAtk3Level = GetSkillLevel("Masa","Atk3",playerData.skills);
            int _skillsLength = playerData.skills.Length;

            for(int i=0;i<_skillsLength;i++)
            {
                Skill _skill = playerData.skills[i];
                print("level" + _skill.level);
                SetSkillLevel(_skill);
       
            }

        }
          
        
        
    
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
            selectCharacter = SelectCharacter.Soldier;
            
            SetActiveCharacter((int)SelectCharacter.Soldier);
            
            
            camPos.localPosition = new Vector3(0.05f,0.5f,0.5f);
            playerMove.CharacterReset();
            
           
        }
        // 마사 캐릭터로 
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectCharacter = SelectCharacter.MasaSchool;
            SetActiveCharacter((int)SelectCharacter.MasaSchool);
            camPos.localPosition = new Vector3(0f,0.45f,-0.8f);
            
            playerMove.CharacterReset();
            
            
        }
        #if UNITY_EDITOR


        if(Input.GetKey(KeyCode.Alpha8))
        {
            Gold = 2147483647;

        }

        #endif

        if(Gold <= 30)
        {
           Gold += 300;
        }

        

       
    }

    

    

    

    // 골드 획득시
    public void AddGold(int _gold)
    {
        // 21억 넘어갈시 
        try
        {
            checked
            {
                Gold += _gold;
            }
        }
        catch (OverflowException)
        {
            
            Gold = int.MaxValue; 
        }

        //textGold.text = Gold.ToString();
        
    }

    //골드 사용시 
    public bool UseGold(int _use)
    {
        _use *= GoldUsage;
        if(0 > Gold - _use)
        {

            //골드 부족 
            GameManager.gameManager.StartWarningUI(goldShortage);
            return false;

        }
            
        Gold -= _use;
        //textGold.text = Gold.ToString();

       
       

        return true;
    }

    //경험치 획득시 
    public void AddExp(int _exp)
    {
        Exp += _exp;
       

        LevelUp();

        //expSlider.value = (float)Exp/(float)maxExp;

    }

    // 레벨업시 
    private void LevelUp()
    {
        
        if(Exp >= maxExp)
        {
            
            Exp -= maxExp;
            Level++;

           

            maxExp += Level*1000;
            textLevel.text = Level.ToString();
            expSlider.value = (float)Exp/(float)maxExp;
            AddGold(Level*1000);
            AtkDamage += (int)(Level * 1.1f);
            
            DataManager.dataManager.SavePlayerData();
        }
    }

    // 현재 고른 캐릭터 
    public void SetActiveCharacter(int _index)
    {
        aim.SetActive(false);

        //카메라 배율 초기화 
        Camera.main.fieldOfView = 60f;
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

            if((int)SelectCharacter.Soldier == _index)
            {
                aim.SetActive(true);
            }
           
            
        }
    }

    //현재 스킬에 레벨 
    public int GetSkillLevel(string _key)
    {
        
       
        if(!skillDictionary.ContainsKey(_key))
            return -1;
       
        return skillDictionary[_key].level;
    }





    // 딕셔너리에 스킬 추가 
    void SetSkill(Skill _skill)
    {
        skillDictionary[_skill.whoSkill + _skill.skillName] = _skill;
    }
    void SetWeapon(WeaponInfo _weapon)
    {
        weaponDictionary[_weapon.whoWeapon] = _weapon.level;
    }

    public void NewSkill()
    {
        
        Skill _skill = new Skill("Masa","Atk1",15);
        AddSkill(_skill);

    }

    // 스킬 추가 
    public void AddSkill(Skill _skill,string _key = "")
    {
        if(_key == "")
            _key = _skill.whoSkill + _skill.skillName;
        
        
        if(skillDictionary.ContainsKey(_key))
            return;
       // skillDictionary[_key] = _skill;
       



        GameObject skillWindow = Instantiate(skillPrefab,contentPanel);
        TextMeshProUGUI[] texts = skillWindow.GetComponentsInChildren<TextMeshProUGUI>();
        Button button = skillWindow.GetComponentInChildren<Button>();
        skillObjectDictionary[_key] = skillWindow;

        // 이미지 로드 및 할당
        Sprite skillImage = Resources.Load<Sprite>("Sprites/" + _key); // 이미지 파일 경로
        Transform imageTransform = skillWindow.transform.Find("SkillImage");
        skillWindow.GetComponentInChildren<SkillInfo>()._key = _key;

        if(imageTransform != null)
        {
            Image imageComponent = imageTransform.GetComponent<Image>();
            if(imageComponent != null)
            {
                if(skillImage != null)
                {
                    imageComponent.sprite = skillImage;
                }
            }
        }
       
       
        
        //texts[0].text = // LV:고정
        texts[(int)SkillText.Level].text = _skill.level.ToString(); // 레벨이 몇인지
        texts[(int)SkillText.SkillName].text = _skill.skillName; // 스킬 이름 
        //texts[3].text =  LevelUp 고정
        texts[(int)SkillText.Gold].text = "1000"; // 몇 골드 드는지
        //texts[5].text =  // G 고정 
        // 버튼 클릭 이벤트 추가
        skillDictionary[_key] = _skill;
        button.onClick.AddListener(() => SetSkillLevel(_skill,true));

       
        //DataManager.dataManager.SavePlayerData();

    }

    

    // 딕셔너리에 추가된 스킬을 ui로 생성 
    void CreateSkill()
    {
        int i = 0;
        foreach(KeyValuePair<string,Skill> enrty in skillDictionary)
        {
            Skill _skill = enrty.Value;
            string _key = _skill.whoSkill+_skill.skillName;
            GameObject skillWindow = Instantiate(skillPrefab,contentPanel);
            TextMeshProUGUI[] texts = skillWindow.GetComponentsInChildren<TextMeshProUGUI>();
            Button button = skillWindow.GetComponentInChildren<Button>();
            skillObjectDictionary[_key] = skillWindow;

            // 이미지 로드 및 할당
            Sprite skillImage = Resources.Load<Sprite>("Sprites/" + _key); // 이미지 파일 경로

            Transform imageTransform = skillWindow.transform.Find("SkillImage");
            if (imageTransform != null)
            {
                Image imageComponent = imageTransform.GetComponent<Image>();
                if (imageComponent != null)
                {
                    if (skillImage != null)
                    {
                        imageComponent.sprite = skillImage;
                    }
                }
            }

            SkillInfo skillInfo = skillWindow.GetComponentInChildren<SkillInfo>();
            skillInfo._key = _key;

             
            _skill = playerData.skills[i++];
            //texts[0].text = // LV:고정
            texts[(int)SkillText.Level].text = _skill.level.ToString(); // 레벨이 몇인지
            texts[(int)SkillText.SkillName].text = _skill.skillName; // 스킬 이름 
            //texts[3].text =  LevelUp 고정
            texts[(int)SkillText.Gold].text = "1000"; // 몇 골드 드는지
            //texts[5].text =  // G 고정 


            // 버튼 클릭 이벤트 추가
            button.onClick.AddListener(() => SetSkillLevel(_skill,true));
      
        }
    }
    
    // 스킬레벨을 세팅 _levelUp == true 이면 스킬레벫업 
    void SetSkillLevel(Skill _skill,bool _levelUp = false) 
    {
        
        string _key = _skill.whoSkill+_skill.skillName;
        
        GameObject skillWindow = skillObjectDictionary[_skill.whoSkill+_skill.skillName];

       
        TextMeshProUGUI[] texts = skillWindow.GetComponentsInChildren<TextMeshProUGUI>();
         
        
        int _gold = (int)(500*_skill.level*_skill.level*2);;
        if(!skillDictionary.ContainsKey(_key))
            return;
        if(_levelUp && UseGold(_gold))
        {
            
            _skill.level += 1;

            

            _gold = (int)(500*_skill.level*_skill.level*2);

           
            skillDictionary[_key].level = _skill.level;

               
            // UI 요소의 텍스트를 변경합니다.
            texts[(int)SkillText.Level].text = _skill.level.ToString(); // 레벨이 몇인지
            //texts[2].text = skill.skillName; // 스킬 이름 
            texts[(int)SkillText.Gold].text = _gold.ToString(); // 몇골드 드는지

            //스킬레벨업시 저장 
            DataManager.dataManager.SavePlayerData();
           
        }
        else
        {
            
            texts[(int)SkillText.Level].text = _skill.level.ToString(); // 레벨이 몇인지
            
            texts[(int)SkillText.Gold].text = _gold.ToString(); // 몇골드 드는지

        }
    }  

    public int InitDamage()
    {
        int _initDamage;
        // 전체기본공격력에 + 캐릭별 무기 공격력 
        _initDamage = AtkDamage + weaponDictionary[selectCharacter]*2 + Level*2;


        return _initDamage;
    }

    enum SkillText
    {
        Level = 1,
        SkillName = 2,
        Gold = 4,


    }


   
}
