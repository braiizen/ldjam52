using UnityEngine;

public enum CropType
{
    Strawberry
}

public class HarvestableCrop : MonoBehaviour
{
    public Soil soil;
    private double value;
    public int daysLeft;
    public int MaxDays;
    public CropType cropType;
    
    public double Value => soil.isFertilized ? value * 1.5 : value;
    public double DaysLeft => daysLeft;

    public void HarvestCrop()
    {
        GameManager.Instance.HarvestedCrops.Add(this);
        soil.crop = null;
        enabled = false;
    }
}
