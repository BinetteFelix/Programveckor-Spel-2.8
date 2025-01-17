using UnityEngine;

[CreateAssetMenu(fileName = "ItemProfile", menuName = "Scriptable Objects/ItemProfile")]
public class ItemProfile : ScriptableObject
{
    public string Type;
    public string Name;
    public string Description;
    public Sprite InventoryImage;
}
