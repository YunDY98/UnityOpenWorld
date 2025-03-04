using UnityEngine;




[CreateAssetMenu(fileName = "NewItem", menuName = "ItemSO/Item")]
public class ItemSO : ScriptableObject
{
    public float rate;
    public int gold;
    public int exp;

    public ItemInfo item; // DataManager


   
    
}

