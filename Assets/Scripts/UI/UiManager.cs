using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textGold;
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private GameObject aim;

    [SerializeField] private GameObject goldShortage;

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
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private RectTransform rectInventory;

    //마우스 감도 
    [SerializeField] private Slider rotSpeedSlider;
    [SerializeField] private TextMeshProUGUI rotSpeedText;
    private int rotSpeed = 2000;

    //FPS
    [SerializeField] private Slider fpsSlider;
    [SerializeField] private TextMeshProUGUI fpsText;
    private int fps = 60;


    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider expSlider;     
    [SerializeField] private GameObject levelUp;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject itemWarning;

    private Coroutine uiCoroutine;

    
   
    private Stack<bool> uiStack = new();
    private PlayerStats playerStats;
    private GameManager gameManager;

    //private IInventoryModel inventory;

    private static UIManager _instance;
    
    
    public static UIManager Instance
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

    void OnEnable()
    {
        keyboard.SetActive(false);
        playerStats = PlayerStats.Instance;
        gameManager = GameManager.Instance;

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


    // public void SetInventory(IInventoryModel inventory)
    // {
    //     this.inventory = inventory;
    // }

  
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

       
        ToggleUI(KeyCode.K,skill,rectSkill);
        ToggleUI(KeyCode.M,keyboard,rectKeyboard);
       
        ToggleUI(KeyCode.I,inventoryUI,rectInventory);

        // 옵션창이 켜져있다면 esc  불가능
        if(!option.activeSelf)
            ToggleUI(KeyCode.Escape,esc,rectESC);

        
       
    }

    // 골드 업데이트
    private void GoldUpdate(int value)
    {
        //textGold.text = gold.ToString();
        textGold.text = $"Gold : {value}";
    }

    private void AimOnOff(bool isAim)
    {
        aim.SetActive(isAim);

    }

   
    // 특정 UI 요소를 최상단에 배치하는 함수
    public void BringToFront(RectTransform uiElement)
    {
        uiElement.SetAsLastSibling();
    }

    public void UiStack(bool isbool)
    {
        
        if(isbool)
        {
            
            
            uiStack.Push(isbool);
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


    public void ToggleUI(KeyCode keyCode,GameObject gameObject,RectTransform rectTransform)
    {
        if(Input.GetKeyDown(keyCode))
        {
            
            
            bool isbool = !gameObject.activeSelf;
            BringToFront(rectTransform);

            UiStack(isbool);
            gameObject.SetActive(isbool);
           
        }


    }

    public void Option()
    {
        BringToFront(rectOption);
        option.SetActive(!option.activeSelf);
        
        if(option.activeSelf)
        {
           
            if(PlayerPrefs.HasKey("Data"))
            {
                rotSpeedSlider.value = PlayerPrefs.GetInt("rotSpeed");
                fpsSlider.value = PlayerPrefs.GetInt("fps");
            
            }
            else
            {
                PlayerPrefs.SetInt("Data",1);
                PlayerPrefs.SetInt("rotSpeed", rotSpeed);
                PlayerPrefs.SetInt("fps", fps);

                rotSpeedSlider.value = rotSpeed;
                fpsSlider.value = fps;
            }
            
            
            
            rotSpeedText.text = $"{rotSpeed}";
            fpsText.text = $"{fps}";

  
            
        }
        
        
    }

    public void OptionApply()
    {
        PlayerPrefs.SetInt("rotSpeed", rotSpeed);
        gameManager.rotSpeed = rotSpeed;

        PlayerPrefs.SetInt("fps", fps);
        gameManager.FPS(fps);
        

        PlayerPrefs.Save();

        option.SetActive(!option.activeSelf);
    }

    public void MouseSensitivity()
    {
        rotSpeed = (int)rotSpeedSlider.value;
        
        
        rotSpeedText.text = $"{rotSpeed}";

        
        

       
        
    }
    public void FPS()
    {
        fps = (int)fpsSlider.value;
       
        
        fpsText.text = $"{fps}";
        
       
        
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
       // textLevel.text = value.ToString();
        textLevel.text = $"Lv : {value}";
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
