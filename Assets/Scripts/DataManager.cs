using UnityEngine;
using System.IO;
using TMPro;



public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager dataManager { get { return _instance; } }

    private string dataFilePath;

    private string keyWord = "wutheringwaves";

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
        dataFilePath = $"{Application.persistentDataPath}/data.json";
    }
   
   

    // 데이터 저장
    public void SavePlayerData()
    { 
        print("save");
        PlayerData _pd = new PlayerData();

        

        _pd.level = PlayerStats.playerStats.Level;
        _pd.exp = PlayerStats.playerStats.Exp;
        _pd.gold = PlayerStats.playerStats.Gold;
        
        
        // 스킬 총 갯수 
        int _skillCount = PlayerStats.playerStats.skillDictionary.Count;
        // 아이템 갯수 
        int _itemCount = InventorySystem.inventorySystem.items.Count;

        int _weaponCount = PlayerStats.playerStats.weaponDictionary.Count;

        _pd.skills = new Skill[_skillCount];
        _pd.items = new SaveItemInfo[_itemCount];
        _pd.weapons = new WeaponInfo[_weaponCount];
       
        int _index = 0;

        foreach(var _skill in PlayerStats.playerStats.skillDictionary)
        {
            _pd.skills[_index++] = _skill.Value;
            
        }
        _index = 0;
        
        foreach(var _item in InventorySystem.inventorySystem.items)
        {
            
            _pd.items[_index++] = new SaveItemInfo(_item.Key, _item.Value.quantity);
            
    
        }
        _index = 0;
        foreach(var _weapon in PlayerStats.playerStats.weaponDictionary)
        {
            WeaponInfo _weaponInfo = new(_weapon.Key,_weapon.Value);
           
            _pd.weapons[_index++] = _weaponInfo;
            
        }


        // 데이터를 JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(_pd);

        // JSON 데이터를 파일로 저장
        //File.WriteAllText(dataFilePath, EncryptAndDecrypt(jsonData));
        File.WriteAllText(dataFilePath, (jsonData));

    }

  

    // 데이터 불러오기
    public PlayerData LoadPlayerData()
    {
        if (File.Exists(dataFilePath))
        {
            // 파일에서 JSON 데이터 읽기
            string jsonData = File.ReadAllText(dataFilePath);

            // JSON 데이터를 역직렬화하여 객체로 변환

            //return JsonUtility.FromJson<PlayerData>(EncryptAndDecrypt(jsonData));
            return JsonUtility.FromJson<PlayerData>((jsonData));
        }
        else
        {
            Debug.LogError("데이터 파일이 존재하지 않습니다.");
            return null;
        }
    }

    private string EncryptAndDecrypt(string data)
    {
        string result = "";

        for(int i=0;i<data.Length;++i)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
        }

        return result;
    }   


   
}





[System.Serializable]
public class PlayerData
{
    public int level;
    public int exp;
    public int gold;
    public SaveItemInfo[] items;
    public Skill[] skills;

    public WeaponInfo[] weapons;

}

[System.Serializable]
public class ItemInfo
{
    public string itemName;
    public int quantity;

    public Sprite sprite;
    public TextMeshProUGUI text;
    
    public ItemInfo(string itemName, int quantity, Sprite sprite = null, TextMeshProUGUI text = null)
    {
        this.itemName = itemName;
        
        this.quantity = quantity;

        this.sprite = sprite;

        this.text = text;
    }
   
}
[System.Serializable]
public class SaveItemInfo
{
    public string itemName;
    public int quantity;

   
    public SaveItemInfo(string itemName, int quantity)
    {
        this.itemName = itemName;
        
        this.quantity = quantity;
    }
   
}

[System.Serializable]
public class WeaponInfo
{
    public SelectCharacter whoWeapon;
    public int level;
    public WeaponInfo(SelectCharacter whoWeapon, int level)
    {
        this.whoWeapon = whoWeapon;
        this.level = level;
    }
}



[System.Serializable]
public class Skill
{
    public string whoSkill;
    public string skillName;
    public int level;

    


    public Skill(string whoSKill,string skillName, int level)
    {
        this.whoSkill = whoSKill;
        this.skillName = skillName;
        this.level = level;
    }
   
  
}


public enum SelectCharacter
{
    Masa,
    Soldier,
}
    


