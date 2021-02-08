using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreItemCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item { get; private set; }

    [SerializeField] Image itemImage = default;
    [SerializeField] Image highlightImage = default;
    [SerializeField] GameObject Popup = default;
    [SerializeField] Text ItemInfo = default;
    ResourceManager resourceManager; // For debugging

    public delegate void ItemSelectedHandler(Item item);
    public event ItemSelectedHandler onSelected;

    public void Initialize(Item item, ItemSelectedHandler itemSelectedHandler, ResourceManager resourceManager) //resource manager added for debugging
    {
        this.item = item;
        this.itemImage.sprite = item.Icon;
        this.onSelected += itemSelectedHandler;
        this.resourceManager = resourceManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onSelected.Invoke(item);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightImage.enabled = true;
        this.Popup.SetActive(true);
        this.ItemInfo.text = this.item.ItemName + "  $" + this.item.Price + "\n" + this.resourceManager.CheckRemainingResource(this.item) + " Remaining"; //resource manager added for debugging
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightImage.enabled = false;
        this.Popup.SetActive(false);
    }
}
