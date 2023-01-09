using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : GenericSingletonClass<GameManager>
{
    [SerializeField] private string interactableTag = "Interactable";
    
    public RaycastHit Hit;
    private Transform player;
    public TextMeshProUGUI tmpPrompt;
    public int Stamina = 100;
    public int MaxStamina = 100;
    public GameObject[] GardenBeds;
    public GameObject[] Hives;
    public GameObject[] ChickenCoops;
    public GameObject Cat;

    private int[] GardenBedPrices = { 200, 400, 800 };
    private int CatPrice = 250;
    private int[] HivePrices = { 200, 400 };
    private int[] ChickenCoopPrices = { 500, 200, 200, 200, 200, 200 };

    private int gardenBedLevel = 0;
    private int hiveLevel = 0;
    private int chickenCoopLevel = 0;

    public TextMeshProUGUI GardenBedButtonText;
    public TextMeshProUGUI CatButtonText;
    public TextMeshProUGUI HiveButtonText;
    public TextMeshProUGUI ChickenCoopButtonText;

    
    public static event Action EndDayEvent;

    [FormerlySerializedAs("_selection")] public Transform Selection;

    private void Update()
    {
        if (InventoryManager.Instance.openInventory) return;
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        
        if (Selection != null)
        {
            OnDeselect(Selection);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Selection = null;
        if (Physics.Raycast(ray, out Hit))
        {
            var selection = Hit.transform;
            if (selection.CompareTag(interactableTag) && Vector3.Distance(selection.position, player.position) < 3.5)
            {
                /*Debug.Log(selection.name);*/
                Selection = selection;
            }
        }

        if (Selection != null)
        {
            OnSelect(Selection);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Selection != null)
            {
                var interactable = Selection.GetComponent<IInteractable>();
                bool interacted = interactable.TryInteract();
            }
        }
    }
    
    public void OnSelect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
        }

        var interactable = selection.GetComponent<IInteractable>();
        if(interactable != null)
        {
            tmpPrompt.text = interactable.InteractPrompt;
        }
    }

    public void OnDeselect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }

        tmpPrompt.text = "";
    }

    public void EndDay()
    {
        Debug.Log("Ending day.");
        EndDayEvent?.Invoke();
    }

    public void TryBuyGarden()
    {
        if (gardenBedLevel > GardenBedPrices.Length - 1) return;
        if (InventoryManager.Instance.money >= GardenBedPrices[gardenBedLevel])
        {
            GardenBeds[gardenBedLevel].gameObject.SetActive(true);
            InventoryManager.Instance.UpdateMoney(GardenBedPrices[gardenBedLevel] * -1);
            gardenBedLevel++;
            if (gardenBedLevel > GardenBedPrices.Length - 1)
                GardenBedButtonText.text = "No more garden upgrades.";
            else
                GardenBedButtonText.text = "Buy Garden Bed: " + GardenBedPrices[gardenBedLevel];
        }
    }

    public void TryBuyCat()
    {
        if (InventoryManager.Instance.money >= CatPrice)
        {
            Cat.gameObject.SetActive(true);
            InventoryManager.Instance.UpdateMoney(CatPrice * -1);
            CatButtonText.text = "No more cats. (sorry!)";
        }
    }

    public void TryBuyHive()
    {
        if (hiveLevel > HivePrices.Length - 1) return;
        if (InventoryManager.Instance.money >= HivePrices[hiveLevel])
        {
            Hives[hiveLevel].gameObject.SetActive(true);
            InventoryManager.Instance.UpdateMoney(HivePrices[hiveLevel] * -1);
            hiveLevel++;
            if (hiveLevel > HivePrices.Length - 1)
                HiveButtonText.text = "No more hive upgrades.";
            else
                HiveButtonText.text = "Buy Hive: " + GardenBedPrices[gardenBedLevel];
        }
    }

    public void TryBuyChickenCoop()
    {
        if (chickenCoopLevel > ChickenCoopPrices.Length - 1) return;
        if (InventoryManager.Instance.money >= ChickenCoopPrices[chickenCoopLevel])
        {
            ChickenCoops[chickenCoopLevel].gameObject.SetActive(true);
            InventoryManager.Instance.UpdateMoney( ChickenCoopPrices[chickenCoopLevel] * -1);
            chickenCoopLevel++;
            if (chickenCoopLevel > ChickenCoopPrices.Length - 1)
                ChickenCoopButtonText.text = "No more chicken coop upgrades.";
            else
                ChickenCoopButtonText.text = "Buy Chicken: " + ChickenCoopPrices[chickenCoopLevel];
        }
    }
}