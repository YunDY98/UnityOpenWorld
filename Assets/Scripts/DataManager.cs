using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

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
        print("save");
        PlayerData _pd = new PlayerData();

        

        _pd.level = PlayerStats.playerStats.level;
        _pd.exp = PlayerStats.playerStats.exp;
        _pd.gold = PlayerStats.playerStats.gold;
        

        int _skillCount = PlayerStats.playerStats.skillDictionary.Count;

        _pd.skills = new Skill[_skillCount];

        int _index = 0;

        foreach (var _skill in PlayerStats.playerStats.skillDictionary)
        {
            _pd.skills[_index++] = _skill.Value;
        }

        
       
       

        
       
       


       

        // 데이터를 JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(_pd);

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
    public string whoSkill;
    public string skillName;
    public int level;
    public Skill(string _whoSKill,string _skillName, int _level)
    {
        this.whoSkill = _whoSKill;
        this.skillName = _skillName;
        this.level = _level;
    }
   
  
}

