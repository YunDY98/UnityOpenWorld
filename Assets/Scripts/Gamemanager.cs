using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Runtime.CompilerServices;

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
    
    
    public static GameManager Instance
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

    //public GameObject gameLabel;

   // public delegate void GameState();
    public event Action<bool> GameLabel;
    public event Action GameText;
    public bool isUI;
    
    public PlayerMove player;

    
   
   
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
        
        if(PlayerPrefs.HasKey("Data"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("fps");
            rotSpeed = PlayerPrefs.GetInt("rotSpeed");
        }
        else
        {
            PlayerPrefs.SetInt("fps",60);
            PlayerPrefs.SetInt("rotSpeed",2000);
            Application.targetFrameRate = 60;
            rotSpeed = 2000;
        }
    }



    void Start()
    {   

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
       
        if(PlayerStats.Instance.HP <= 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion",0f);

            //gameLabel.SetActive(true);
            GameLabel?.Invoke(true);
            GameText?.Invoke();

            
            gState = GameState.GameOver;
                
        }

       
       
        
    }

   

    public void RestartGame()
    {
        Time.timeScale = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(1);
    }



    public void ExitGame()
    {
        DataManager.Instance.SavePlayerData();
        Application.Quit();
    }
    
    public void FPS(int fps)
    {
        Application.targetFrameRate = fps;
    }


   
    
}
