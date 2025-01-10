using UnityEngine;

public class InteractItem : MonoBehaviour
{
    bool _IsItem = false; // Bool for collision type
    bool _IsActive = false; // Bool for collision states

    private GameObject Player; // Player object
    private POPUP Item; // Pop-up text for Item
    private PickUpItem pickup; // Object to reference picking up items

    // Update is called once per frame
    void Update()
    {
        if (_IsActive) // If colliding with object
        {
            if (Input.GetKeyDown(KeyCode.E)) // If Player hits the 'E' key
            {
                _IsActive = false; // Reset collision state to false
                if (_IsItem) // If the collision is with an Item
                {
                    pickup.PickUp(); // Pick up item
                    Item.ClosePopUp(); // Destroy PopUp text
                    _IsItem = false; // Reset collision type
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player = collision.gameObject; // Sets Player object to collision with gameObject
        Item = Player.GetComponent<POPUP>(); // Sets interactable object to collision with gameObject
        pickup = Player.GetComponent<PickUpItem>(); // Sets pickup object to collision with gamObject

        if (Item != null) // Íf collision is with Item
        {
            Debug.Log("Was Interacted with!");
            Item.OpenPopUp(); // Activate pop-up
            _IsItem = true; // Set Collision type to Item
        }
        _IsActive = true; // Active collision
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player = collision.gameObject; // Sets Player object to collision with gameObject
        Item = Player.GetComponent<POPUP>(); // Sets Item object to collision with gameObject

        if (Item != null) // If collision was with an Item
        {
            Item.ClosePopUp(); // Deactivate pop-up
            _IsItem = false; // Reset collision type
        }
        _IsActive = false; // No active collision
    }
}
