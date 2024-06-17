using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    //게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        GameOver

    }

    
    private static GameManager _instance;
    // Start is called before the first frame update
    
    public static GameManager gameManager
    {
        get
        {

            return _instance;
        }
        set
        {

        }
    }
 
    public int rotSpeed = 2000;
    private int maxRotSpeed = 5000;

    // 게임 상태 ui 변수

    public GameObject gameLabel;

    // esc
    public GameObject esc;
    public RectTransform rectESC;
    public GameObject option;
    public RectTransform rectOption;

    public GameObject keyboard;
    public  RectTransform rectKeyboard;
    public GameObject skill;
    public RectTransform rectSkill;

    public GameObject inventory;
    public RectTransform rectInventory;

    public Slider rotSpeedSlider;

  
    public TextMeshProUGUI rotSpeedText;

    public Text gameText;

    
    public PlayerMove player;

    private Stack<bool> uiStack = new Stack<bool>();
    
   
   
    //움직임 관련 일시정지
    public bool isMove = true;

    //캐릭터 정면으로 보기 
    public bool showFace = false;

    // 유저가 설정한 키를 저장할 변수
    public KeyCode[] userKeys = new KeyCode[108];
    
    //반동 
    
  
    //현재 게임 상태 변수
    public GameState gState;

    private void Awake()
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
        // 키보드 keysetting Awake실행후 
        keyboard.SetActive(false);

        //gState = GameState.Ready;
        gState = GameState.Run;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rotSpeed = PlayerPrefs.GetInt("rotSpeed");
       // gameText = gameLabel.GetComponent<Text>();

      //  gameText.text = "Ready .. ";
        
      //  gameText.color = new Color32(255,185,0,255);
        // 게임 준비 -> 게임 중 상태로 전환 
       // StartCoroutine(ReadyToStart());sss

        //player = GameObject.Find("Player").GetComponent<PlayerMove>();
        
        
    }

    // IEnumerator ReadyToStart()
    // {
    //     yield return new WaitForSeconds(0.1f);

    //     gameText.text = "Go!";
        
    //     yield return new WaitForSeconds(.5f);

    //     gameLabel.SetActive(false);

    //     gState = GameState.Run;
    // }

    // Update is called once per frame
    void Update()
    {
    //     if(gState != GameState.Run)
    //         return;
       
        if(player.hp <= 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion",0f);

            gameLabel.SetActive(true);

            gameText.text = "Game Over";

            gameText.color = new Color32(255,0,0,255);

            gState = GameState.GameOver;
                
        }

        ESC();
        SkillWindow();
        Keyboard();
        Inventory();

        
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

    public void UiStack(bool _bool)
    {
        
        if(_bool)
        {
            
            isMove = false;
            uiStack.Push(_bool);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {  
            if(0 != uiStack.Count)
                uiStack.Pop();
            

        }
        
        if(0 == uiStack.Count)
        {
            isMove = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
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

    public void RestartGame()
    {
        Time.timeScale = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(1);
    }

    public void StartWarningUI(GameObject _warning)
    {
        StartCoroutine(WarningUI(_warning));
    }
    IEnumerator WarningUI(GameObject _warning)
    {
        _warning.SetActive(true);
       
        yield return new WaitForSecondsRealtime(2f);

       
        _warning.SetActive(false);

       


    }
    

    // 특정 UI 요소를 최상단에 배치하는 함수
    public void BringToFront(RectTransform uiElement)
    {
        uiElement.SetAsLastSibling();
    }


    
}
