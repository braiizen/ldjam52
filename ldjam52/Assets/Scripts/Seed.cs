using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Seed")]
public class Seed : ScriptableObject
{
    public int GrowthTime;
    public CropType cropType;
    public double value;
    public Sprite[] PlantSprites;
    public Item Crop;
    public Item FertilizedCrop;
}

public enum CropType
{
    Strawberry,
    Turnip,
    Corn,
    Sunflower,
    Carrot,
    Pumpkin
}