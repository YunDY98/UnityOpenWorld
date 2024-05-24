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
        PlayerData pd = new PlayerData();

        

        pd.level = PlayerStats.playerStats.level;
        pd.exp = PlayerStats.playerStats.exp;
        pd.gold = PlayerStats.playerStats.gold;
        

        // // 현재 스킬 배열의 길이를 저장해둡니다.
        // int currentSkillCount = pd.skills != null ? pd.skills.Length : 0;

        // // 새로운 스킬을 추가하려면 기존 스킬 배열보다 하나 더 큰 배열이 필요합니다.
        // Skill[] newSkills = new Skill[currentSkillCount + 1];

        // // 기존 스킬을 새로운 배열에 복사합니다.
        // if (pd.skills != null)
        // {
        //     for (int i = 0; i < currentSkillCount; i++)
        //     {
        //         newSkills[i] = pd.skills[i];
        //     }
        // }
        // // 새로운 스킬을 배열에 추가합니다.
        // newSkills[currentSkillCount] = newSkill;

        // // PlayerData의 스킬 배열을 새로운 배열로 교체합니다.
        // pd.skills = newSkills;

        pd.skills = new Skill[2];

        
        Skill Skill = new Skill();
        Skill.whoskill = "Masa";
        Skill.skillName = "Atk1"; 
        Skill.level = PlayerStats.playerStats.masaAtk1Level;

        Skill Skill3 = new Skill();
        Skill3.whoskill = "Masa";
        Skill3.skillName = "Atk3"; 
        Skill3.level = PlayerStats.playerStats.masaAtk3Level;

        
       
        pd.skills[0] = Skill;  
        pd.skills[1] = Skill3;


       

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
    public string whoskill;
    public string skillName;
    public int level;
   
  
}

