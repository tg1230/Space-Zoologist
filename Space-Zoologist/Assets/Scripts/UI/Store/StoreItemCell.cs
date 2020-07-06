using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreItemCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item { get; private set; }

    [SerializeField] Image itemImage = default;
    [SerializeField] Image highlightImage = default;

    public delegate void ItemSelectedHandler(Item item);
    public event ItemSelectedHandler onSelected;

    public void Initialize(Item item, ItemSelectedHandler itemSelectedHandler)
    {
        this.item = item;
        this.itemImage.sprite = item.Icon;
        this.onSelected += itemSelectedHandler;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onSelected.Invoke(item);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightImage.enabled = false;
    }
}