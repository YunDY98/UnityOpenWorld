using UnityEngine;
using System.IO;
using TMPro;
using System.ComponentModel;
using UnityEngine.UIElements;
using System.Text;



public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance { get { return _instance; } }

    private string dataFilePath;
    private readonly string keyWord = "wutheringwaves";

    IInventoryModel inventory;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // 데이터 파일 경로 설정
        dataFilePath = $"{Application.persistentDataPath}/data.json";

        //inventory = FindObjectOfType<Inventory>();
    }

   
    public void SetInventory(IInventoryModel inventory)
    {   
        this.inventory = inventory;

    }

 
   
   

    // 데이터 저장
    public void SavePlayerData()
    { 
        print("save");

         // 스킬 총 갯수 
        int skillCount = PlayerStats.Instance.skillDictionary.Count;
        // 아이템 갯수 
        int itemCount = inventory.ItemTypeCount();

        int weaponCount = PlayerStats.Instance.weaponDictionary.Count;

        PlayerData pd = new()
        {
            level = PlayerStats.Instance.Level,
            exp = PlayerStats.Instance.Exp,
            gold = PlayerStats.Instance.Gold,
            skills = new Skill[skillCount],
            items = new ItemInfo[itemCount],
            weapons = new WeaponInfo[weaponCount],
            position = PlayerMove.position
        };

        int index = 0;

        foreach(var skill in PlayerStats.Instance.skillDictionary)
        {
            pd.skills[index++] = skill.Value;
            
        }
        index = 0;
        
        foreach(var item in inventory.GetItemsDictionary())
        {
           
            pd.items[index++] = item.Value.itemInfo;
            
    
        }
        index = 0;
        foreach(var weapon in PlayerStats.Instance.weaponDictionary)
        {
            WeaponInfo _weaponInfo = new(weapon.Key,weapon.Value);
           
            pd.weapons[index++] = _weaponInfo;
            
        }


        // 데이터를 JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(pd);

        // JSON 데이터를 파일로 저장
       // File.WriteAllText(dataFilePath, EncryptAndDecrypt(jsonData));
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
        
        StringBuilder sb = new();

        for(int i=0;i<data.Length;++i)
        {
           
            sb.Append((char)(data[i] ^ keyWord[i % keyWord.Length]));
        }
       
        return sb.ToString();
    }   


   
}





[System.Serializable]
public class PlayerData
{
    public int level;
    public int exp;
    public int gold;
    public ItemInfo[] items;
    public Skill[] skills;

    public Vector3 position;

    public WeaponInfo[] weapons;

}

[System.Serializable]
public class ItemData
{
    public ItemInfo itemInfo;
    public Sprite sprite;
    public TextMeshProUGUI text;

    public ItemData(ItemInfo itemInfo, Sprite sprite = null, TextMeshProUGUI text = null)
    {
        this.itemInfo = itemInfo;
        this.sprite = sprite;

        this.text = text;

        
    }
   
}
[System.Serializable]
public class ItemInfo
{
    public string itemName;
    public int quantity;

    public ItemType type;
    public ItemInfo(string itemName, int quantity,ItemType type)
    {
        this.itemName = itemName;
        
        this.quantity = quantity;

        this.type = type;
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

public enum ItemType
{
    None,
    Consumable,
    ETC,


}
    


