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

    public Item EatenCrop;

    private void Awake()
    {
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
                int daysReduced = 1;
                crop.DecreaseDaysLeft(daysReduced);
                plantSprite.sprite = plantedSeed.PlantSprites[plantedSeed.GrowthTime-crop.DaysLeft];
            }

            Debug.Log(name + ": " + crop.cropType + " has " + crop.DaysLeft + " days left.");
            isFertilized = false;
        }
        
        isWatered = false;
        updatePrompt();
    }


    public bool Harvest()
    {
        Item cropToHarvest;

        if (crop.wasAttacked)
        {
            cropToHarvest = EatenCrop;
        } else if (crop.isFertilized)
        {
            cropToHarvest = plantedSeed.FertilizedCrop;
        }
        else
        {
            cropToHarvest = plantedSeed.Crop;
        }
        
        bool harvested = InventoryManager.Instance.AddItem(cropToHarvest);

        if (harvested)
        {
            Debug.Log(name + " harvested.");
            AudioManager.Instance.PlayHarvestClip();
            plantedSeed = null;
            crop = null;
            plantSprite.enabled = false;
            updatePrompt();
        }

        return harvested;
    }

    public void Attack()
    {
        crop.wasAttacked = true;
        updatePrompt();
    }

    private void updatePrompt()
    {
        if (crop != null)
        {
            if (crop.wasAttacked)
            {
                _prompt = "Plant looks half eaten...did the rats eat it at night?";
            }
            else
            {
                _prompt = @$"{CropString(crop.cropType)}
Days till mature: {crop.DaysLeft}
Watered: {isWatered}";
            }
        }
        else
        {
            _prompt = $@"Tilled: {isTilled}
Fertilized: {isFertilized}
Watered: {isWatered}";
        }
    }

    public string CropString(CropType cropType)
    {
        switch (cropType)
        {
            case CropType.Turnip:
                return "Turnip";
            case CropType.Carrot:
                return "Carrot";
            case CropType.Corn:
                return "Corn";
            case CropType.Pumpkin:
                return "Pumpkin";
            case CropType.Strawberry:
                return "Strawberry";
            case CropType.Sunflower:
                return "Sunflower";
        }

        return null;
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
                    Dig();
                    return true;
                
                case ActionType.Water:
                    isWatered = true;
                    updatePrompt();
                    AudioManager.Instance.PlayPouringClip();
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

    private void Dig()
    {
        isTilled = true;
        updatePrompt();
        AudioManager.Instance.PlayDigClip();
        Debug.Log(name + " tilled.");
    }
}

public interface IInteractable
{
    public string InteractPrompt { get; }
    public bool TryInteract(Item item = null);
}
