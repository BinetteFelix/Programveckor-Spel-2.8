using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject Information;
    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        Information.SetActive(true);
    }
    // When not highlighted with mouse
    public void OnPointerExit(PointerEventData eventData)
    {
        Information.SetActive(false);
    }
}
