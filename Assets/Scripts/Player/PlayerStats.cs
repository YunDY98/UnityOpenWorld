using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats _instance;

    public static PlayerStats Instance
    {
        get
        {

            return _instance;
        }
    }
   
    public SelectCharacter selectCharacter;

    public PlayerMove playerMove;
    


    public PlayerData playerData;

    public event Action<int> GoldUpdate;
    public event Action<bool> AimOnOff;
    public event Action<GameObject> StartUi;

    public event Action<float> HpSlider;
    public event Action<float> ExpSlider;

    public event Action<int> LevelText;
    public event Action LevelUpEvent;
    public event Action HitEffectEvent;

    public event Action GoldShortageEvent;


    //비행중인지 
    private bool _isFly;
    public bool IsFly
    {
        get { return _isFly; }
        set { _isFly = value; }
    }
    
    private int _level;
    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            //textLevel.text = _level.ToString();
            LevelText.Invoke(_level);
            maxExp = _level*2000;
        }
    }
    private int _exp;
    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;
            //expSlider.@value = (float)_exp/maxExp;
            ExpSlider.Invoke((float)_exp/maxExp);
            
           
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
            
            GoldUpdate.Invoke(_gold);
           
        }
    }

  

    //기본 공격력

    private int _atk = 10;
    public int Atk
    {
        
        get { return _atk; }

        set
        {
            _atk = value;
        }
    }

    private int _atkBuff = 0;
    public int AtkBuff
    {
        get { return _atkBuff; }
        set
        {
            _atkBuff = value;
        }

    }
    public int AtkDamage =>  Atk + AtkBuff;

    private int _hp = 1000;
    //체력 
    public int HP
    {
        get { return _hp; }
        set
        {
            _hp = Mathf.Clamp(value, 0, maxHp);
            HpSlider.Invoke((float)_hp/maxHp);
           
        }
    }

    int maxHp = 1000;
    


    

    public GameObject[] characterMode;

    public Transform camPos;

    private int selectedIndex;

    
    public int sceneNumber;

   


    private int maxExp = 10000;


    
    
   
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


    void Start()
    {  
        playerData = DataManager.Instance.LoadPlayerData();

        //활성화된 캐릭터 
        selectedIndex = 0;
        SetActiveCharacter((int)selectCharacter);

        if(playerData == null)
        {
            Level = 1;
            Exp = 0;
            Gold = 1000000;

            
           
            foreach(SelectCharacter weapon in Enum.GetValues(typeof(SelectCharacter)))
            {
               
               if(!weaponDictionary.ContainsKey(weapon))
               {
                   //weaponDictionary.Add(_weapon,1);
                   weaponDictionary[weapon] = 1;
               }
            }
                 
        }
        else
        {
           
            foreach(var weapon in playerData.weapons)
            {
                SetWeapon(weapon);
            }
            
              // skills 배열에 저장된 스킬
            foreach(var skill in playerData.skills)
            {
                SetSkill(new Skill(skill.whoSkill, skill.skillName, skill.level));
               
            }

        }

         

       
        CreateSkill();
        SetPlayerData();
        maxExp = Level*2000;
        
        Atk = (int)(Level * 1.1f);
        print("maxExp"+maxExp);
       
    }

    //데이터 불러오기 
    public void SetPlayerData()
    {
        

        if(playerData != null)
        {
            Level = playerData.level;
           
            Exp = playerData.exp;
           
            
            Gold = playerData.gold;
            
            int skillsLength = playerData.skills.Length;

            for(int i=0;i<skillsLength;i++)
            {
                Skill skill = playerData.skills[i];
                
                SetSkillLevel(skill);
       
            }

        }
      
    }
   
   
    public void SetSceneNumber(int sceneNumber)
    {
        this.sceneNumber = sceneNumber;

    }

    // Uwwwwwpdate is called once per frame
    void Update()
    {   
        //print(maxExp);
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
            selectCharacter = SelectCharacter.Masa;
            SetActiveCharacter((int)SelectCharacter.Masa);
            camPos.localPosition = new Vector3(0f,0.45f,-2f);
            
            playerMove.CharacterReset();
            
            
        }
        #if UNITY_EDITOR


        if(Input.GetKey(KeyCode.Alpha8))
        {
            Gold = 2147483647;
            HP = 1;

        }

        #endif

        if(Gold <= 30)
        {
           Gold += 300;
        }


        
      
    }

   

    // 골드 획득시
    public void AddGold(int gold)
    {
        // 21억 넘어갈시 
        try
        {
            checked
            {
                Gold += gold;
                
            }
        }
        catch (OverflowException)
        {
            
            Gold = int.MaxValue; 
        }
  
    }

    //골드 사용시 
    public bool UseGold(int use)
    {
        
        if(0 > Gold - use)
        {

            //골드 부족 
            //GameManager.gameManager.StartUI(goldShortage);
            GoldShortageEvent.Invoke();
            return false;

        }
            
        Gold -= use;
       
        return true;
    }

    //경험치 획득시 
    public void AddExp(int exp)
    {
        Exp += exp;
       
        if(Exp >= maxExp)
        {
            LevelUp();
        }
    }

    // 레벨업시 
    private void LevelUp()
    {
        
        while(Exp >= maxExp)
        {
            Exp -= maxExp;
            Level++;
            AddGold(Level*1000);
            Atk = (int)(Level * 1.1f);
        }
        //textLevel.text = Level.ToString();
        LevelText.Invoke(Level);
        //expSlider.value = (float)Exp/maxExp;
        ExpSlider.Invoke((float)Exp/maxExp);
      
        //GameManager.gameManager.StartUI(levelUp);
        //StartUi?.Invoke(levelUp);
        LevelUpEvent.Invoke();

        
        HP = maxHp;
        
        //레벨업시 알려주기 
        DataManager.Instance.SavePlayerData();
        print("maxExp"+maxExp);
        
    }

    // 현재 고른 캐릭터 
    public void SetActiveCharacter(int index)
    {
        //aim.SetActive(false);
        AimOnOff?.Invoke(false);
        //카메라 배율 초기화 
        Camera.main.fieldOfView = 60f;
        // 배열에 있는 모든 오브젝트를 비활성화
        for (int i = 0; i < characterMode.Length; i++)
        {
            characterMode[i].SetActive(false);
           
        }

        // 선택된 인덱스가 유효한 경우 해당 오브젝트를 활성화
        if (index >= 0 && index < characterMode.Length)
        {
            characterMode[index].SetActive(true);
            selectedIndex = index;

            if((int)SelectCharacter.Soldier == index)
            {
                // aim.SetActive(true);
                AimOnOff?.Invoke(true);
            }
           
            
        }
    }

    //현재 스킬에 레벨 
    public int GetSkillLevel(string key)
    {
        
       
        if(!skillDictionary.ContainsKey(key))
            return -1;
       
        return skillDictionary[key].level;
    }





    // 딕셔너리에 스킬 추가 
    void SetSkill(Skill skill)
    {
        skillDictionary[$"{skill.whoSkill}{skill.skillName}"] = skill;
    }
    void SetWeapon(WeaponInfo _weapon)
    {
        weaponDictionary[_weapon.whoWeapon] = _weapon.level;
    }

    public void NewSkill()
    {
        
        Skill skill = new Skill("Masa","Atk1",15);
        AddSkill(skill);

    }

    // 스킬 추가 
    public void AddSkill(Skill skill,string key = "")
    {
        if(key == "")
            key = $"{skill.whoSkill}{skill.skillName}";
        
        
        if(skillDictionary.ContainsKey(key))
            return;
       // skillDictionary[key] = skill;
       



        GameObject skillWindow = Instantiate(skillPrefab,contentPanel);
        TextMeshProUGUI[] texts = skillWindow.GetComponentsInChildren<TextMeshProUGUI>();
        Button button = skillWindow.GetComponentInChildren<Button>();
        skillObjectDictionary[key] = skillWindow;

        // 이미지 로드 및 할당
        Sprite skillImage = Resources.Load<Sprite>("Sprites/" + key); // 이미지 파일 경로
        Transform imageTransform = skillWindow.transform.Find("SkillImage");
        skillWindow.GetComponentInChildren<SkillInfo>().key = key;

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
        texts[(int)SkillText.Level].text = skill.level.ToString(); // 레벨이 몇인지
        texts[(int)SkillText.SkillName].text = skill.skillName; // 스킬 이름 
        //texts[3].text =  LevelUp 고정
        texts[(int)SkillText.Gold].text = "1000"; // 몇 골드 드는지
        //texts[5].text =  // G 고정 
        // 버튼 클릭 이벤트 추가
        skillDictionary[key] = skill;
        button.onClick.AddListener(() => SetSkillLevel(skill,true));

       
        //DataManager.dataManager.SavePlayerData();

    }

    

    // 딕셔너리에 추가된 스킬을 ui로 생성 
    void CreateSkill()
    {
        int i = 0;
        foreach(KeyValuePair<string,Skill> enrty in skillDictionary)
        {
            Skill skill = enrty.Value;
            string key = $"{skill.whoSkill}{skill.skillName}";
            GameObject skillWindow = Instantiate(skillPrefab,contentPanel);
            TextMeshProUGUI[] texts = skillWindow.GetComponentsInChildren<TextMeshProUGUI>();
            Button button = skillWindow.GetComponentInChildren<Button>();
            skillObjectDictionary[key] = skillWindow;

            // 이미지 로드 및 할당
            Sprite skillImage = Resources.Load<Sprite>("Sprites/" + key); // 이미지 파일 경로

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
            skillInfo.key = key;

             
            skill = playerData.skills[i++];
            //texts[0].text = // LV:고정
            texts[(int)SkillText.Level].text = skill.level.ToString(); // 레벨이 몇인지
            texts[(int)SkillText.SkillName].text = skill.skillName; // 스킬 이름 
            //texts[3].text =  LevelUp 고정
            texts[(int)SkillText.Gold].text = "1000"; // 몇 골드 드는지
            //texts[5].text =  // G 고정 


            // 버튼 클릭 이벤트 추가
            button.onClick.AddListener(() => SetSkillLevel(skill,true));
      
        }
    }
    
    // 스킬레벨을 세팅 _levelUp == true 이면 스킬레벫업 
    void SetSkillLevel(Skill skill,bool levelUp = false) 
    {
        
        string _key = $"{skill.whoSkill}{skill.skillName}";
        
        GameObject skillWindow = skillObjectDictionary[_key];

       
        TextMeshProUGUI[] texts = skillWindow.GetComponentsInChildren<TextMeshProUGUI>();
         
        
        int _gold = (int)(500*skill.level*skill.level*2);;
        if(!skillDictionary.ContainsKey(_key))
            return;
        if(levelUp && UseGold(_gold))
        {
            
            skill.level += 1;

            

            _gold = (int)(500*skill.level*skill.level*2);

           
            skillDictionary[_key].level = skill.level;

               
            // UI 요소의 텍스트를 변경합니다.
            texts[(int)SkillText.Level].text = skill.level.ToString(); // 레벨이 몇인지
            //texts[2].text = skill.skillName; // 스킬 이름 
            texts[(int)SkillText.Gold].text = _gold.ToString(); // 몇골드 드는지

            //스킬레벨업시 저장 
            DataManager.Instance.SavePlayerData();
           
        }
        else
        {
            
            texts[(int)SkillText.Level].text = skill.level.ToString(); // 레벨이 몇인지
            
            texts[(int)SkillText.Gold].text = _gold.ToString(); // 몇골드 드는지

        }
    }  

    public int InitDamage()
    {
        int _initDamage;
        // 전체기본공격력에 + 캐릭별 무기 공격력 
        _initDamage = AtkDamage + weaponDictionary[selectCharacter]*2 + Level*2;
        print("Atk" + AtkDamage + "WeaponDictionary" + weaponDictionary[selectCharacter]*2+ "Level" + Level*2);


        return _initDamage;
    }


    public void DamageAction(int damage)
    {
        // 에너미의 공격력 만큼 
        HP -= damage;

        // 피격 효과 
        if(_hp > 0)
        {
            // StartCoroutine(PlayHitEffect());
            HitEffectEvent.Invoke();
        }

    }

 
    enum SkillText
    {
        Level = 1,
        SkillName = 2,
        Gold = 4,


    }


   
}
