using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    public ItemType type;
    public ActionType actionType;
    public int Value;

    [Header("Only UI")] 
    public bool stackable = true;
    public bool consumable = true;
    public int maxStack = 1;
    
    [Header("Both")]
    public Sprite image;

    public Seed seedSO;
}

public enum ItemType
{
    Tool,
    Seed,
    Sellable,
    Consumable,
    Fertilizer
}

public enum ActionType
{
    Dig, 
    Water,
    Break,
    Harvest
}
