using UnityEngine;
using UnityEngine.Serialization;

public class Nest : MonoBehaviour, IInteractable
{
    public Sprite[] nestSprite;
    public Item Egg; 
    public SpriteRenderer nestSpriteRenderer;
    private bool takenEgg = false;

    private void Awake()
    {
        GameManager.EndDayEvent += EndDay;
     
        nestSpriteRenderer.sprite = nestSprite[1];
    }

    private void EndDay()
    {
        nestSpriteRenderer.sprite = nestSprite[1];
        takenEgg = false;
    }

    public string InteractPrompt => takenEgg ? "Already harvested egg." : "Press E to take egg.";
    public bool TryInteract(Item item = null)
    {
        if (takenEgg) return false;
        
        bool added = InventoryManager.Instance.AddItem(Egg);
        if (added)
        {
            takenEgg = true;
            nestSpriteRenderer.sprite = nestSprite[0];
        }

        return false;
    }
}
