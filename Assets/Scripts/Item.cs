using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public enum Occupation
    {
        weapon,
        armor,
        specialWeapon,
        engine
    }
    public string itemName;
    public Sprite icon;
    public bool haveItem;
    public Occupation occupation;
}