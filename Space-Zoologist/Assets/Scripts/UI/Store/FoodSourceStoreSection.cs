using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Store section for food source items.
/// </summary>
public class FoodSourceStoreSection : StoreSection
{
    //[Header("Food Source Store Section")]
    //[SerializeField] FoodSourceManager foodSourceManager = default;

    /// <summary>
    /// Handles the click release on the cursor item.
    /// </summary>
    public override void OnCursorPointerUp(PointerEventData eventData)
    {
        base.OnCursorPointerUp(eventData);
        if (UIUtility.ins.IsCursorOverUI(eventData))
        {
            base.OnItemSelectionCanceled();
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            FoodSourceManager.ins.CreateFoodSource(selectedItem.ID, Camera.main.ScreenToWorldPoint(eventData.position));
            playerBalance.RuntimeValue -= selectedItem.Price;
        }
    }
}
