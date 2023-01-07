using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableCrop : MonoBehaviour
{
    private double value;
    private int daysLeft;
    private bool isFertilized;
    private bool isWatered;

    public double Value => isFertilized ? value * 1.5 : value;
    public double DaysLeft => daysLeft;

    public void SetupCrop(double value, int daysLeft, bool isFertilized)
    {
        this.value = value;
        this.daysLeft = daysLeft;
        this.isFertilized = isFertilized;
    }
    
    public void UpdateCrop()
    {
        if(isWatered) {
        {
            if (isFertilized)
            {
                daysLeft -= 2;
            }
            else
            {
                daysLeft -= 1;
            }
        }}

        isWatered = false;
    }
}
