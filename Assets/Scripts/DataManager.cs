using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager dataManager { get { return _instance; } }

    private string dataFilePath;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // 데이터 파일 경로 설정
        dataFilePath = Application.persistentDataPath + "/data.json";
    }

   

    void Start()
    {
       
        
       
       
    }

    // 데이터 저장
    public void SavePlayerData()
    {
        PlayerData pd = new PlayerData();

        pd.level = PlayerStats.playerStats.level;
        pd.exp = PlayerStats.playerStats.exp;
        pd.gold = PlayerStats.playerStats.gold;

    


        // 데이터를 JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(pd);

        // JSON 데이터를 파일로 저장
        File.WriteAllText(dataFilePath, jsonData);
    }

    // 데이터 불러오기
    public PlayerData LoadPlayerData()
    {
        if (File.Exists(dataFilePath))
        {
            // 파일에서 JSON 데이터 읽기
            string jsonData = File.ReadAllText(dataFilePath);

            // JSON 데이터를 역직렬화하여 객체로 변환
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            Debug.LogError("데이터 파일이 존재하지 않습니다.");
            return null;
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public int level;
    public int exp;
    public int gold;
    public Item[] items;
    public Skill[] skills;

}

[System.Serializable]
public class Item
{
    public string itemName;
    public int cnt;
   
}

[System.Serializable]
public class Skill
{
    public string skillName;
    public int level;
  
}
