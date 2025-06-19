using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;          
    public int value;                // Skor untuk PickUp atau index untuk Collectible
    public Sprite icon;              
    public string soundEffect;       
    public bool isKeyItem;           // True untuk CollectibleItem, false untuk PickUp
}