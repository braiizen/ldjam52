using UnityEngine;

public class Soil : MonoBehaviour, IInteractable
{
    public bool isWatered;
    public bool isFertilized;
    public bool isTilled;
    public HarvestableCrop crop;
    public Seed plantedSeed;
    private SpriteRenderer plantSprite;
    private string _prompt;

    private void Awake()
    {
        Debug.Log("Adding soil to list: " + this.name);
        GameManager.Instance.AddSoil(this);
        plantSprite = GetComponentInChildren<SpriteRenderer>();
        updatePrompt();
        GameManager.EndDayEvent += UpdateDay;
    }

    public void PlantCrop(Seed seed)
    {
        plantedSeed = seed;
        plantSprite.enabled = true;
        plantSprite.sprite = seed.PlantSprites[0];
        crop = new HarvestableCrop(seed, isFertilized);
        isFertilized = false;
        isTilled = false;
        updatePrompt();
    }

    public void UpdateDay()
    {
        if (crop != null)
        {
            if (isWatered)
            {
                int daysReduced = crop.isFertilized ? 2 : 1;
            
                crop.DecreaseDaysLeft(daysReduced);
            }

            Debug.Log(name + ": " + crop.cropType + " has " + crop.DaysLeft + " days left.");
            isFertilized = false;
        }
        
        isWatered = false;
        updatePrompt();
    }


    public bool Harvest()
    {
        bool harvested = InventoryManager.Instance.AddItem(crop.isFertilized
            ? plantedSeed.FertilizedCrop
            : plantedSeed.Crop);

        if (harvested)
        {
            Debug.Log(name + " harvested.");
            plantedSeed = null;
            crop = null;
            plantSprite.enabled = false;
            updatePrompt();
        }

        return harvested;
    }

    private void updatePrompt()
    {
        if (crop != null)
        {
            _prompt = @$"{crop.cropType.ToString()}
Days till mature: {crop.DaysLeft}
Watered: {isWatered}";
        }
        else
        {
            _prompt = $@"Tilled: {isTilled}
Fertilized: {isFertilized}
Watered: {isWatered}";
        }
    }


    public string InteractPrompt => _prompt;

    public bool TryInteract(Item item)
    {
        if (item == null) return false;
        
        if (item.type == ItemType.Seed)
        {
            if (crop != null) return false;
            if(isTilled)
                PlantCrop(item.seedSO);
        } else if (item.type == ItemType.Fertilizer)
        {
            if (crop != null) return false;
            isFertilized = true;
            updatePrompt();
        }
        else
        {
            switch (item.actionType)
            {
                case ActionType.Dig:
                    if (crop != null)
                        return false;
                    isTilled = true;
                    updatePrompt();
                    Debug.Log(name + " tilled.");
                    return true;
                
                case ActionType.Water:
                    isWatered = true;
                    updatePrompt();
                    Debug.Log(name + " watered.");
                    return true;
                
                case ActionType.Harvest:
                    if (crop.DaysLeft > 0)
                        return false;
                    return Harvest();
            }
        }

        return false;
    }
}

public interface IInteractable
{
    public string InteractPrompt { get; }
    public bool TryInteract(Item item = null);
}
