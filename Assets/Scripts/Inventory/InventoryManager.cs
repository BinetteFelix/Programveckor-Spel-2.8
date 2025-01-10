using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemProfile> Items = new List<ItemProfile>(); // Public list of items

    public Transform ItemContent; // Item grid
    public GameObject InventoryItemBase; // The base prefab for inventory items (Button, text, etc.)

    public void Awake()
    {
        Instance = this;
    }
    public void Add(ItemProfile item)
    {
        Items.Add(item); // add Item into inventory list
    }
    public void ArrangeItems()
    {
        foreach (Transform item in ItemContent)  // finds each item in inventory on every load
        {
            Destroy(item.gameObject); // deletes last saved item and deletes it so there are no duplicates
        }
        foreach (var item in Items) // finds each item in the list of item
        {
            GameObject ItemObject = Instantiate(InventoryItemBase, ItemContent); //adds/re-adds all items into inventory grid

            var itemIcon = ItemObject.transform.Find("ItemImage").GetComponent<Image>(); // finds the image child in the specified item: 'item'
            var itemName = ItemObject.transform.Find("ItemName").GetComponent<TextMeshProUGUI>(); // finds the name child in the specified item: 'item'

            itemIcon.sprite = item.InventoryImage; // sets the sprite/image of the inventory item
            itemName.text = item.Name; // sets the text/name of the inventory item
        }
    }
}
