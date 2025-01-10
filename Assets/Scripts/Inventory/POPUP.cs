using TMPro;
using UnityEngine;

public class POPUP : MonoBehaviour
{
    public TextMeshProUGUI PopUpText; // Pop-up text prefab
    private TextMeshProUGUI popuptext; // Private reference object

    Vector3 SpawnOffset = new Vector3(5, 5, 0); // Spawn offset for pop-up text

    bool _IsActive = false; // bool for pop-up state

    // Update is called once per frame
    void Update()
    {
        if (_IsActive) // If the pop-up is active
        {
            popuptext.transform.position = Camera.main.WorldToScreenPoint(transform.position) + SpawnOffset; // set pop-up's position to the item object + an offset
        }
    }
    public void OpenPopUp()
    {
        _IsActive = true; // Say that the pop-up is active
        popuptext = Instantiate(PopUpText, FindAnyObjectByType<Canvas>().transform).GetComponent<TextMeshProUGUI>(); // Instantiate pop-up prefab
    }
    public void ClosePopUp()
    {
        _IsActive = false; // Sa that the pop-up is inactive
        Destroy(gameObject);
    }
}
