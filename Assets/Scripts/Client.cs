using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Client : MonoBehaviour
{
    public string serverIP = "192.168.35.105";

    public int serverPort = 8888;
    

    public TMP_InputField id;
    public TMP_InputField pwd;

    public GameObject loginFailedPanel;
    public GameObject joinSuccessPanel;
    public GameObject joinFailedPanel;

    public GameObject connectingPanel;
    


    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;
    private bool running = true;


    public string receiveMessage;
    
    enum State
    {
        Login,
        LoginFail,
        Join,
        
        JoinFail,
        Connecting

    } 

    private int state;
    void Awake()
    {
        // 해당 오브젝트를 파괴하지 않음
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
       
    }

    void Update()
    {
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
            ConnectingPanel();
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
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();
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

        byte[] _userinfo = Encoding.UTF8.GetBytes(_action.PadRight(2,'\0')+_id.PadRight(20,'\0')+_pwd.PadRight(20,'\0'));
       
      
        stream.Write(_userinfo,0, _userinfo.Length);

        
       
        //stream.Flush();

        id.text = "";
        pwd.text = "";
        
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

    public void ConnectingPanel()
    {
        connectingPanel.SetActive(!connectingPanel.activeSelf);
        receiveMessage = "";
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
        if (client != null)
        {
            client.Close();
        }
    }
}
