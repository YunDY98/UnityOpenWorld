using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
public class Client : MonoBehaviour
{
    public string serverIP = "192.168.35.105";

    public int serverPort = 8888;
    

    public TMP_InputField id;
    public TMP_InputField pwd;




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

    } 

    void Start()
    {
        
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
                    if(message == State.Login.ToString())
                    {
                        //로그인 성공 
                    }
                    if(message == State.Join.ToString())
                    {
                        //회원 가입 성공 
                    }
                    if(message == State.LoginFail.ToString())
                    {
                        //로그인 실패
                    } 
                    if(message == State.JoinFail.ToString())
                    {
                        //가입 실패 
                    }                   


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
        
        IdPwd(State.Login.ToString());
    }

     public void Join()
    {
        IdPwd(State.Join.ToString());
       
        
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

    private void Testa(string _action)
    {
        
        string _id = id.text;
        string _pwd = pwd.text;
        
        Debug.Log(_id);

        byte[] _idbuffer = Encoding.UTF8.GetBytes(_id);
        byte[] _pwdbuffer = Encoding.UTF8.GetBytes(_pwd);
      


        
        stream.Write(_idbuffer, 0, _idbuffer.Length);
        
        stream.Write(_pwdbuffer, 0, _pwdbuffer.Length);
       
        //stream.Flush();

        id.text = "";
        pwd.text = "";

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
