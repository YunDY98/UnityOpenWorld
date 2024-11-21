using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textGold;
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private GameObject aim;

    [SerializeField] public  GameObject goldShortage;

    [SerializeField] private GameObject gameLabel;
    [SerializeField] private Text gameText;

    [SerializeField] private GameObject esc;
    [SerializeField] private RectTransform rectESC;

    // 옵션창
    [SerializeField] private GameObject option;
    [SerializeField] private RectTransform rectOption;

    //키셋팅 키보드
    [SerializeField] private GameObject keyboard;
    [SerializeField] private  RectTransform rectKeyboard;

    // 스킬
    [SerializeField] private GameObject skill;
    [SerializeField] private RectTransform rectSkill;

    //인벤토리
    [SerializeField] private GameObject inventory;
    [SerializeField] private RectTransform rectInventory;

    //마우스 감도 
    [SerializeField] private Slider rotSpeedSlider;
    [SerializeField] private TextMeshProUGUI rotSpeedText;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider expSlider;     
    [SerializeField] private GameObject levelUp;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject itemWarning;

    private Coroutine uiCoroutine;

    public int rotSpeed = 2000;
   
    private Stack<bool> uiStack = new Stack<bool>();
    private PlayerStats playerStats;
    private GameManager gameManager;

    private InventorySystem inventorySystem;




    






    private static UiManager _instance;
    
    
    public static UiManager uiManager
    {
        get
        {

            return _instance;
        }
        
    }
 
    
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
        keyboard.SetActive(false);
    }

    private void OnEnable()
    {
       
        playerStats = PlayerStats.playerStats;
        gameManager = GameManager.gameManager;
        inventorySystem = InventorySystem.inventorySystem;


        // 이벤트 구독
        playerStats.GoldUpdate += GoldUpdate;  
        playerStats.AimOnOff += AimOnOff;
        playerStats.HpSlider += HpSlider;
        playerStats.ExpSlider += ExpSlider;
        playerStats.LevelText += LevelText;
        playerStats.LevelUpEvent += LevelUp;
        playerStats.HitEffectEvent += HitEffect;
        playerStats.GoldShortageEvent += GoldShortage;

        

        


       
        gameManager.GameLabel += GameLabel;
        gameManager.GameText += GameOver;

      
        
           
        
        
    }

    void Update()
    {
        // if(PlayerStats.playerStats.HP <= 0)
        // {
        //     player.GetComponentInChildren<Animator>().SetFloat("MoveMotion",0f);

        //     gameLabel.SetActive(true);

        //     gameText.text = "Game Over";

        //     gameText.color = new Color32(255,0,0,255);

        //     //gState = GameState.GameOver;
                
        // }

        // ESC();

        SkillWindow();
        Keyboard();
        ESC();
        Inventory();

    }

    // 골드 업데이트
    private void GoldUpdate(int gold)
    {
        textGold.text = gold.ToString();
    }

    private void AimOnOff(bool isAim)
    {
        aim.SetActive(isAim);

    }

    // private void ESC()
    // {
    //     if(Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         bool _bool = !esc.activeSelf;
    //         BringToFront(rectESC);
    //         UiStack(_bool);
    //         esc.SetActive(_bool);
    //         //gamestate를 바꿔줘야되나?
            
    //     }

    // }
      // 특정 UI 요소를 최상단에 배치하는 함수
    public void BringToFront(RectTransform uiElement)
    {
        uiElement.SetAsLastSibling();
    }

    public void UiStack(bool _bool)
    {
        
        if(_bool)
        {
            
            
            uiStack.Push(_bool);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            // gState = GameState.Ready;
            
        }
        else
        {  
            if(0 != uiStack.Count)
                uiStack.Pop();
            

        }
        
        if(0 == uiStack.Count)
        {
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            // gState = GameState.Run;
            
        }

           
    }

    void GameLabel(bool isActive)
    {
        gameLabel.SetActive(isActive);
    }

    void GameOver()
    {
        gameText.text = "Game Over";

        gameText.color = new Color32(255,0,0,255);

    }

    public void SkillWindow()
    {
        
        if(Input.GetKeyDown(KeyCode.K))
        {
            
            
            bool _bool = !skill.activeSelf;

          
            BringToFront(rectSkill);
            UiStack(_bool);
            skill.SetActive(_bool);
           
        }

    }

    void Keyboard()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            bool _bool = !keyboard.activeSelf;
            BringToFront(rectKeyboard);
            UiStack(_bool);
            keyboard.SetActive(_bool);
            

        }

    }

    public void ESC()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            bool _bool = !esc.activeSelf;
            BringToFront(rectESC);
            UiStack(_bool);
            esc.SetActive(_bool);
            
        }

    }


   

    public void Inventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            
            
            bool _bool = !inventory.activeSelf;
            BringToFront(rectInventory);

            UiStack(_bool);
            inventory.SetActive(_bool);
           
        }

    }

    public void Option()
    {
        option.SetActive(!option.activeSelf);

        if(option.activeSelf)
        {
            BringToFront(rectOption);
            if(PlayerPrefs.HasKey("rotSpeed"))
            {
                rotSpeedSlider.value = PlayerPrefs.GetInt("rotSpeed");
            
            }
            else
            {
                rotSpeedSlider.value = rotSpeed;
            }

            
            rotSpeedText.text = rotSpeed.ToString();
  
            
        }
        else
        {
            PlayerPrefs.Save();
            
        }
        
    }

    public void MouseSensitivity()
    {
        rotSpeed = (int)rotSpeedSlider.value;
       
        //rotSpeedText.text = $"{rotSpeed}";
        rotSpeedText.text = rotSpeed.ToString();

        PlayerPrefs.SetInt("rotSpeed", rotSpeed);
        
    }

    public void StartUI(GameObject ui,float time = 2)
    {
        if(uiCoroutine != null)
        {
            StopCoroutine(uiCoroutine);
        }

        uiCoroutine = StartCoroutine(UiCo(ui,time));
    }


    IEnumerator UiCo(GameObject ui,float time = 2f)
    {
        ui.SetActive(true);
       
        yield return new WaitForSecondsRealtime(time);

       
        ui.SetActive(false);
        uiCoroutine = null;

       
    }

    public void HpSlider(float value)
    {
       hpSlider.value = value;
    }

    public void ExpSlider(float value)
    {
        expSlider.value = value;
    }
    
    public void LevelText(int value)
    {
        textLevel.text = value.ToString();
    }
    public void LevelUp()
    {
        StartUI(levelUp);

    }

    public void HitEffect()
    {
        StartUI(hitEffect,0.3f);
    }

    public void GoldShortage()
    {
        StartUI(goldShortage);
    }

    public void ItemWarning()
    {
        StartUI(itemWarning);
    }
    
 


}
