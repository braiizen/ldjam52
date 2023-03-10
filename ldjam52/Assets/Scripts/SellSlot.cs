using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        InventoryManager.Instance.SellItem(item);
    }
}
