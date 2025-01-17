using System.Collections.Generic;
using UnityEngine;

public class EquipedItemManager : MonoBehaviour
{
    public static EquipedItemManager Instance;
    public List<ItemProfile> Items = new List<ItemProfile>(); // Public list of items

    [SerializeField] Transform EquippedGrid; // Item grid
    [SerializeField] GameObject InventoryEquippable; // The base prefab for inventory items (Button, text, etc.)

    private void Awake()
    {
        Instance = this;
    }
    public void Add(ItemProfile item)
    {
        Items.Add(item); // add Item into equipped list
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
