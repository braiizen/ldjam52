

public class HarvestableCrop
{
    private double value;
    public bool isFertilized;
    public int DaysLeft;
    public CropType cropType;
    public Seed seed;

    public HarvestableCrop(Seed seed, bool IsFertilized)
    {
        this.seed = seed;
        DaysLeft = seed.GrowthTime;
        isFertilized = IsFertilized;
    }
    
    public double Value => isFertilized ? value * 1.5 : value;
}
