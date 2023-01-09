using UnityEngine;

public class Laptop : MonoBehaviour , IInteractable
{
    public string InteractPrompt => "Press E to use Laptop";
    public bool TryInteract(Item item = null)
    {
        InventoryManager.Instance.fromLaptop = true;
        InventoryManager.Instance.openInventory = true;
        AudioManager.Instance.PlayTypingClip();
        return false;
    }
}
