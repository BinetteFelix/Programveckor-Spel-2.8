using UnityEngine;

public class InteractItem : MonoBehaviour
{
    private bool _IsActive = false;
    private bool _IsInRange = false;

    private GameObject Player;

    POPUP popUpUI;
    PickUpItem pickUpItem;

    private void Update()
    {
        if (_IsActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _IsActive = false;
                if (_IsInRange)
                {
                    pickUpItem.PickUp();
                    popUpUI.ClosePopUp();
                    _IsInRange = false;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Player = other.gameObject;
        popUpUI = other.GetComponent<POPUP>();
        pickUpItem = other.GetComponent<PickUpItem>();

        if (popUpUI != null)
        {
            popUpUI.OpenPopUp();
            _IsInRange = true;
        }
        _IsActive = true;
    }
    private void OnTriggerExit(Collider other)
    {
        Player = other.gameObject;
        popUpUI = Player.GetComponent<POPUP>();
        pickUpItem = Player.GetComponent<PickUpItem>();

        //For when you Leave ItemObject's Range/Exit the IsTrigger collider
        if (popUpUI != null)
        {
            popUpUI.ClosePopUp();
            _IsInRange = false;
        }
        _IsActive = false;
    }

}
