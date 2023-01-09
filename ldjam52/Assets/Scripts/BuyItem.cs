using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    public Item item;
    public Image image;
    public TextMeshProUGUI text;

    private void Awake()
    {
        image.sprite = item.image;
        text.text = (item.Value * 2).ToString();
    }

    public void TryPurchaseItem()
    {
        InventoryManager.Instance.BuyItem(item);
    }
}
