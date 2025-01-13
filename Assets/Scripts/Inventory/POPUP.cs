using TMPro;
using UnityEngine;

public class POPUP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PopUpText; // Pop-up text prefab
    private TextMeshProUGUI popuptext; // Private reference object

    private Vector3 SpawnOffset = new Vector3(0, 20, 0); // Spawn offset for pop-up text

    private bool _IsActive = false; // bool for pop-up state

    // Update is called once per frame
    void Update()
    {
        if (_IsActive) // If the pop-up is active
        {
            popuptext.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + SpawnOffset; // set pop-up's position to the item object + an offset
        }
    }
    public void OpenPopUp()
    {
        if(!_IsActive)
        {
            _IsActive = true; // Say that the pop-up is active
            popuptext = Instantiate(PopUpText, FindAnyObjectByType<Canvas>().transform).GetComponent<TextMeshProUGUI>(); // Instantiate pop-up prefab
        }
    }
    public void ClosePopUp()
    {
        if (_IsActive)
        {
            _IsActive = false; // Say that the pop-up is inactive
            Destroy(popuptext);
        }
    }
}