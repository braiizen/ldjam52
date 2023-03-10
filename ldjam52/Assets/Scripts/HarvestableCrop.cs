

using UnityEngine;

public class HarvestableCrop
{
    private double value;
    public bool isFertilized;
    public int DaysLeft;
    public CropType cropType;
    public Seed seed;
    public bool wasAttacked;

    public HarvestableCrop(Seed seed, bool IsFertilized)
    {
        this.seed = seed;
        DaysLeft = seed.GrowthTime;
        isFertilized = IsFertilized;
        cropType = seed.cropType;
        Debug.Log("Planted crop: " + cropType);
    }
    
    public double Value => isFertilized ? value * 1.5 : value;

    public void DecreaseDaysLeft(int noOfDays)
    {
        DaysLeft -= noOfDays;
        if (DaysLeft < 1) DaysLeft = 0;
    }
}
