using UnityEngine;
using UnityEngine.Profiling;

public class PickUpItem : MonoBehaviour
{
    public ItemProfile Item; // Specified item (Is specified per item)
    public void PickUp()
    {
        InventoryManager.Instance.Add(Item); // Add Item to inventory list
        Destroy(gameObject); // Delete Item from scene
    }
}
