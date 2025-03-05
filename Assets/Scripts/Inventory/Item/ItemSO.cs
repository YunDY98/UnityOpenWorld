using UnityEngine;




[CreateAssetMenu(fileName = "NewItem", menuName = "ItemSO/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField]
    private float rate;
    public float Rate => rate;

    [SerializeField]
    private int gold;
    public int Gold => gold;

    [SerializeField]
    private int exp;
    public int Exp => exp;

    [SerializeField]
    private ItemInfo item;
    public ItemInfo Item => item;


   

   
    
}

