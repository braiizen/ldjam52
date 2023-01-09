using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour, IInteractable
{
    private bool harvestedHoney = false;
    public Item honey;
    public string InteractPrompt => harvestedHoney ? "Already harvested today." : "Press E to harvest honey.";

    private void Awake()
    {
        GameManager.EndDayEvent += EndDay;
    }

    private void EndDay()
    {
        harvestedHoney = false;
    }

    public bool TryInteract(Item item = null)
    {
        if (harvestedHoney) return false;
        harvestedHoney = InventoryManager.Instance.AddItem(honey);
        
        return harvestedHoney;
    }
}
