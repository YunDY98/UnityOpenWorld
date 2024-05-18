using UnityEngine;
using System.Data;
using MySql.Data.MySqlClient;

public class MySQLConnector : MonoBehaviour
{
    private MySqlConnection connection;
    private string server = "127.0.0.1"; // MySQL 서버 주소
    private string database = "Login"; // 데이터베이스 이름
    private string username = "root"; // MySQL 사용자 이름
    private string password = "4321"; // MySQL 암호
    //private string port = "3306";

    
    void Start()
    {
        string connectionString = "Server=" + server + ";Database=" + database + ";Uid=" + username + ";Pwd=" + password + ";";
        //string connectionString = "Server=" + server + ";Port=" + port + ";Database=" + database + ";Uid=" + username + ";Pwd=" + password + ";";


        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            Debug.Log("MySQL 연결 성공");

            // 데이터를 읽어오거나 다른 작업 수행
            ReadData();
        }
        catch (MySqlException e)
        {
            Debug.LogError("MySQL 연결 실패: " + e.Message);
        }
    }

    void ReadData()
    {
        string query = "SELECT * FROM YourTableName"; // 데이터를 읽어올 쿼리

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // 데이터 읽어오기 예시:
                    int num = reader.GetInt32(0); // 첫 번째 열 값(Num)
                    string id = reader.GetString(1); // 두 번째 열 값(ID)
                    string pwd = reader.GetString(2); // 세 번째 열 값(PWD)

                    Debug.Log("Num: " + num + ", ID: " + id + ", PWD: " + pwd);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
            Debug.Log("MySQL 연결 종료");
        }
    }
}
