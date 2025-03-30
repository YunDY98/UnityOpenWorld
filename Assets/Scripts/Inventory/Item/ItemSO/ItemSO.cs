using UnityEngine;




[CreateAssetMenu(fileName = "NewItem", menuName = "ItemSO/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField]
    private float _rate;
    public float Rate => _rate;

    [SerializeField]
    private int _gold;
    public int Gold => _gold;

    [SerializeField]
    private int _exp;
    public int Exp => _exp;

    [SerializeField]
    private ItemInfo _item;
    public ItemInfo Item => _item;

    
   
   
    
}

