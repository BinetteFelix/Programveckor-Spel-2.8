using UnityEngine;

public class InteractItem : MonoBehaviour
{
    bool _IsInteractable = false; // Bool for collision type
    bool _IsActive = false; // Bool for collision states

    private GameObject Player; // Player object
    private POPUP Interactable; // Pop-up text for interactables
    private PickUpItem pickup; // Object to reference picking up items

    // Update is called once per frame
    void Update()
    {
        if (_IsActive) // If colliding with object
        {
            if (Input.GetKeyUp(KeyCode.E)) // If Player hits the 'E' key
            {
                _IsActive = false; // Reset collision state to false
                if (_IsInteractable) // If the collision is with an interactable
                {
                    pickup.PickUp(); // Pick up item
                    Interactable.ClosePopUp(); // Destroy interactable text
                    _IsInteractable = false; // Reset collision type
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player = collision.gameObject; // Sets Player object to collision with gameObject
        Interactable = Player.GetComponent<POPUP>(); // Sets interactable object to collision with gameObject
        pickup = Player.GetComponent<PickUpItem>(); // Sets pickup object to collision with gamObject
        if (!_IsActive) // If there isn't already a collision
        {
            if (Interactable != null) // Íf collision is with Interactable
            {
                Debug.Log("Was Interacted with!");
                Interactable.OpenPopUp(); // Activate pop-up
                _IsInteractable = true; // Set Collision type to Interactable
            }
        }
        _IsActive = true; // Active collision
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player = collision.gameObject; // Sets Player object to collision with gameObject
        Interactable = Player.GetComponent<POPUP>(); // Sets interactable object to collision with gameObject
        if (_IsActive) // If there was a collision
        {
            if (Interactable != null) // If collision was with a Interactable
            {
                Interactable.ClosePopUp(); // Deactivate pop-up
                _IsInteractable = false; // Reset collision type
            }
        }
        _IsActive = false; // No active collision
    }
}
