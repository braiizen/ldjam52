using UnityEngine;

public class Soil : MonoBehaviour
{
    public bool isWatered;
    public bool isFertilized;
    public HarvestableCrop crop;

    private void Awake()
    {
        Debug.Log("Adding soil to list: " + this.name);
        GameManager.Instance.AddSoil(this);
    }

    public void PlantCrop(HarvestableCrop crop)
    {
        this.crop = crop;
        crop.soil = this;
    }

    public void UpdateDay()
    {
        if(isWatered) {
        {
            if (isFertilized)
            {
                crop.daysLeft -= 2;
            }
            else
            {
                crop.daysLeft -= 1;
            }
        }}

        isWatered = false;
    }
}
