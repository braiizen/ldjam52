using UnityEngine;

public class Soil : MonoBehaviour, IInteract
{
    public bool isWatered;
    public bool isFertilized;
    public bool isTilled;
    public HarvestableCrop crop;
    public Seed plantedSeed;

    private void Awake()
    {
        Debug.Log("Adding soil to list: " + this.name);
        GameManager.Instance.AddSoil(this);
    }

    public void PlantCrop(Seed seed)
    {
        plantedSeed = seed;
        crop = new HarvestableCrop(seed, isFertilized);
        isFertilized = false;
    }

    public void UpdateDay()
    {
        if (isWatered)
        {
            {
                if (isFertilized)
                {
                    crop.DaysLeft -= 2;
                }
                else
                {
                    crop.DaysLeft -= 1;
                }
            }
        }

        isWatered = false;
    }

    public bool Harvest()
    {
        bool harvested = InventoryManager.Instance.AddItem(crop.isFertilized
            ? plantedSeed.FertilizedCrop
            : plantedSeed.Crop);

        if (harvested)
        {
            plantedSeed = null;
            crop = null;
        }

        return harvested;
    }


    public bool TryInteract(Item item)
    {
        if (item.type == ItemType.Seed)
        {
            if (crop != null) return false;
            if(isTilled)
                PlantCrop(item.seedSO);
        } else if (item.type == ItemType.Fertilizer)
        {
            if (crop != null) return false;
            isFertilized = true;
        }
        else
        {
            switch (item.actionType)
            {
                case ActionType.Dig:
                    if (crop != null)
                        return false;
                    isTilled = true;
                    return true;
                
                case ActionType.Water:
                    isWatered = true;
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

public interface IInteract
{
    public bool TryInteract(Item item);
}
