using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;
using System.Data;
using Unity.VisualScripting;

public class Client : MonoBehaviour
{
    private static Client _instance;

    public static Client client
    {
        get
        {
            if (_instance == null)
            {
                // 씬에 Client 객체가 없다면 새로 생성합니다.
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<Client>();
                singletonObject.name = "Client";

                // Client가 씬 전환에서 파괴되지 않도록 설정합니다.
                DontDestroyOnLoad(singletonObject);
            }
            return _instance;
        }
    }
    public string serverIP = "192.168.35.105";

    public int serverPort = 8888;

    public const int statePacketSize = 2;
    public const int idPacketSize = 20;
    public const int pwdPacketSize = 20;


    public const int levelPacketSize = 33;
     public const int expPacketSize = 33;

    public const int magPacketSize = 33;

   
    public const int scenePacketSize = 2;



    public TMP_InputField id;
    public TMP_InputField pwd;

    public GameObject loginFailedPanel;
    public GameObject joinSuccessPanel;
    public GameObject joinFailedPanel;

    public GameObject alreadyLoginPanel;

    public GameObject notConnectedPanel;


    


    private TcpClient tcpClient;
    private NetworkStream stream;
    private Thread receiveThread;
    private bool running = true;

    

    public string receiveMessage;
    
    enum State
    {
        Login, //로그인 성공시 
        LoginFail, // 로그인 실패시 
        Join, // 회원가입 성공시 
        
        JoinFail,// 회원 가입 실패 
        Connecting, //현재 아이디가 접속중인지 
        KeepAlive, // 인터넷 연결 상태 확인
        UserStats, // 레벨 경험치 등 유저 정보 
        MagStat, // 총알 수 
        SceneNum, // 현재 씬 
        LevelStat, // 캐릭터 레벨
        ExpStat, // 캐릭터 경험치 
        


    }

    private float serverTime;
    private float clientTime;
    private float reciveTime = 20f;
    private float sendTime = 5f;
    
    private bool isConnected = true;

    public int level;
    public int exp; 
    public int mag;
    public int sceneNumber;
    

    private int state;
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
      
    }

    void Update()
    {
       
        if(receiveMessage.Length > 100)
        {
            // Null 문자 기준으로 문자열을 분리
            string[] parts = receiveMessage.Split(new char[] { '\0' }, System.StringSplitOptions.RemoveEmptyEntries);


            level = int.Parse(parts[0]); 
            exp = int.Parse(parts[1]);
            mag = int.Parse(parts[2]);
            sceneNumber = int.Parse(parts[3]);
            Debug.Log("parse3" + int.Parse(parts[3]));
            receiveMessage = "";
            //1 0 30 2
        }
        else
        {
            receiveMessage = receiveMessage.Trim();

            if(receiveMessage == EnumToString(State.KeepAlive))
            {
                Debug.Log("serverKeep");
                //KeepAlive(EnumToString(State.KeepAlive));
                serverTime = 0;
                receiveMessage = "";
            }

            if(receiveMessage == EnumToString(State.Login))
            {
                Debug.Log("로그인 성공");
                //로그인 성공 
                SceneManager.LoadScene(1);
                receiveMessage = "";


            }
            if(receiveMessage == EnumToString(State.Join))
            {
                //회원 가입 성공 
                JoinSuccessPanel();
            }
            if(receiveMessage == EnumToString(State.LoginFail))
            {
                //로그인 실패
                LoginFailedPanel();

                Debug.Log("로그인 실패");
            } 
            if(receiveMessage == EnumToString(State.JoinFail))
            {
                //가입 실패 
                JoinFailedPanel();
            }
            if(receiveMessage == EnumToString(State.Connecting))
            {
                //접속중
                AlreadyLoginPanel();
            }
        }
       
      
        serverTime += Time.deltaTime;
        if(serverTime > reciveTime && isConnected)
        {

            Time.timeScale = 0f;
            
            NotConnectedPanel();
            Debug.Log("서버와 연결 끊김");
            isConnected = false;
            
        }

        

        

      
       
        clientTime += Time.deltaTime;
        if(sendTime < clientTime)
        {
            clientTime = 0;
           
            KeepAlive(EnumToString(State.KeepAlive));
            
        }
        


             
        
    }

  

    void OnEnable()
    {
        ConnectToServer();
        // 수신 작업 시작
        receiveThread = new Thread(new ThreadStart(ReceiveMessages));
        receiveThread.Start();
        

    }

 

   
    

    void ConnectToServer()
    {
        try
        {
            tcpClient = new TcpClient(serverIP, serverPort);
            stream = tcpClient.GetStream();
            receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            receiveThread.Start();
            Debug.Log("Connected to server.");
        }
        catch (System.Exception e)
        {
            Debug.Log("Error connecting to server: " + e.Message);
        }
    }

    void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;
        while (running)
        {
            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.Log("Received message: " + message);
                    receiveMessage = message;
                    


                }
               
            
            }
            catch (System.Exception e)
            {
                Debug.Log("Error receiving message: " + e.Message);
                break;
            }
        }
    }

    public void Login()
    {
        // 로그인 
        IdPwd(EnumToString(State.Login));

    }

    public void Join()
    {
        //회원가입 
       
        IdPwd(EnumToString(State.Join));
       
        
    }

    private string EnumToString(State _state)
    {
        // enum -> int -> string
        // 123 -> "123"
        state = (int)_state;
        
        return state.ToString();
    }


    private void IdPwd(string _action)
    {
        string _id = id.text;
        string _pwd = pwd.text;
        
        Debug.Log(_id);

        byte[] _userinfo = Encoding.UTF8.GetBytes
        (
            _action.PadRight(statePacketSize,'\0')+
            _id.PadRight(idPacketSize,'\0')+
            _pwd.PadRight(pwdPacketSize,'\0')
        );
       
      
        stream.Write(_userinfo,0, _userinfo.Length);

        
       
        //stream.Flush();

        id.text = "";
        pwd.text = "";
        
    }

    public void KeepAlive(string _action)
    {
        byte[] _userinfo = Encoding.UTF8.GetBytes(_action.PadRight(statePacketSize,'\0'));
       
      
        stream.Write(_userinfo,0, _userinfo.Length);
        Debug.Log("Client -> Server");
    }

    public void ExpStat(int _exp)
    {
        string _action = "ExpStat";
       
        string _sExp = _exp.ToString();
       

        byte[] _userinfo = Encoding.UTF8.GetBytes
        (
            _action.PadRight(statePacketSize,'\0')+
            _sExp.PadRight(expPacketSize,'\0')
            
        );

        stream.Write(_userinfo,0, _userinfo.Length);

    }

    public void LevelStat(int _level)
    {
        string _action = "LevelStat";
        string _sLevel = _level.ToString();
         byte[] _userinfo = Encoding.UTF8.GetBytes
        (
            _action.PadRight(statePacketSize,'\0')+
            _sLevel.PadRight(levelPacketSize,'\0')
           
            
        );

        

    }
    public void SceneStat()
    {
        string _stat = "SceneNum";

        int _sceneNum = SceneManager.GetActiveScene().buildIndex;
        string _sSceneNum = _sceneNum.ToString();

        byte[] _userinfo = Encoding.UTF8.GetBytes(
            _stat.PadRight(statePacketSize,'\0')+
            _sSceneNum.PadRight(magPacketSize,'\0'));

        stream.Write(_userinfo,0, _userinfo.Length);
    }

    public void MagStat(int _mag)
    {
        string _action = "MagStat";
        string _sMag = _mag.ToString();
        byte[] _userinfo = Encoding.UTF8.GetBytes(
            _action.PadRight(statePacketSize,'\0')+
            _sMag.PadRight(magPacketSize,'\0'));
        stream.Write(_userinfo,0, _userinfo.Length);

    }

    public void QuitGame()
    {
        Application.Quit();
    }

   
    public void JoinFailedPanel()
    {
        joinFailedPanel.SetActive(!joinFailedPanel.activeSelf);
        receiveMessage = "";      
       
    }

    public void JoinSuccessPanel()
    {
        joinSuccessPanel.SetActive(!joinSuccessPanel.activeSelf);
        receiveMessage = "";
    }

    public void LoginFailedPanel()
    {
        loginFailedPanel.SetActive(!loginFailedPanel.activeSelf);
        receiveMessage = "";
    }

    public void AlreadyLoginPanel()
    {
        alreadyLoginPanel.SetActive(!alreadyLoginPanel.activeSelf);
        receiveMessage = "";
    }

    public void NotConnectedPanel()
    {
        notConnectedPanel.SetActive(!notConnectedPanel.activeSelf);

    }

    

    void OnDestroy()
    {
        running = false;
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        if (stream != null)
        {
            stream.Close();
        }
        if (tcpClient != null)
        {
            tcpClient.Close();
        }
    }
}
