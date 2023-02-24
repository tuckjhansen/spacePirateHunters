using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemType;
    public Sprite itemIcon;
    public float itemDamage;
    public float itemArmor;
}