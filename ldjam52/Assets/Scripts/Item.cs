using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    public ItemType type;
    public ActionType actionType;

    [Header("Only UI")] 
    public bool stackable = true;
    public int maxStack = 1;
    
    [Header("Both")]
    public Sprite image;
}

public enum ItemType
{
    Tool,
    Seed,
    Sellable,
    Consumable
}

public enum ActionType
{
    Dig, 
    Water,
    Break,
    Harvest
}
