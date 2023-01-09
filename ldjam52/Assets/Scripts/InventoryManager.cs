using System;
using TMPro;
using UnityEngine;

public class InventoryManager : GenericSingletonClass<InventoryManager>
{
   public Item[] startItems;
   public GameObject inventoryItemPrefab;
   public GameObject InventoryUI;
   public GameObject LaptopUI;
   public GameObject PauseUI;
   public GameObject crosshair;
   public bool openInventory = false;
   public bool fromLaptop = false;
   public int money = 9999;
   public static event Action UpdatedMoneyEvent;
   
   public InventorySlot[] InventorySlots;

   private int selectedSlot = -1;

   private void Start()
   {
      ChangeSelectedSlot(0);
      foreach (Item item in startItems)
      {
         AddItem(item);
      }
   }

   private void Update()
   {
      Cursor.visible = openInventory;
      if (Input.inputString != null)
      {
         bool isNumber = int.TryParse(Input.inputString, out int number);
         if (isNumber && number > 0 && number < 8)
            ChangeSelectedSlot(number - 1);
      }
      
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         openInventory = !openInventory;
         if (openInventory)
         {
            PauseUI.SetActive(true);
         }
         fromLaptop = false;
      }
      
      crosshair.SetActive(!openInventory);
      InventoryUI.SetActive(openInventory);
      LaptopUI.SetActive(fromLaptop);
      if (!fromLaptop && openInventory)
      {
         PauseUI.SetActive(true);
      } else {
         PauseUI.SetActive(false);
      }
      
      Cursor.lockState = openInventory ? CursorLockMode.None : CursorLockMode.Locked;
      Time.timeScale = openInventory ? 0f : 1f;
      
      if (Input.GetMouseButtonDown(0))
      {
         if (GameManager.Instance.Selection != null)
         {
            var heldItem = GetSelectedItem(false);
            var interactable = GameManager.Instance.Selection.GetComponent<IInteractable>();
            bool interacted = interactable.TryInteract(heldItem);
            if (interacted)
            {
               GetSelectedItem(true);
               GameManager.Instance.Stamina -= 5;
            }
         }
      }
   }

   void ChangeSelectedSlot(int newValue)
   {
      if (selectedSlot >= 0)
      {
         InventorySlots[selectedSlot].Deselect();
      }
      
      InventorySlots[newValue].Select();
      selectedSlot = newValue;
   }

   public Item GetSelectedItem(bool use)
   {
      InventorySlot slot = InventorySlots[selectedSlot];
      InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
      if (itemInSlot != null)
      {
         Item item = itemInSlot.item;
         if (use && item.consumable)
         {
            itemInSlot.count--;
            if (itemInSlot.count <= 0)
            {
               Destroy(itemInSlot.gameObject);
            }
            else
            {
               itemInSlot.RefreshCount();
            }
         }

         return item;
      }
      
      return null;
   }
   
   public bool AddItem(Item item)
   {
      foreach (InventorySlot slot in InventorySlots)
      {
         InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
         if (itemInSlot != null && itemInSlot.item == item && itemInSlot.item.stackable && itemInSlot.count < itemInSlot.item.maxStack)
         {
            itemInSlot.count++;
            itemInSlot.RefreshCount();
            return true;
         }
      }
      
      foreach (InventorySlot slot in InventorySlots)
      {
         InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
         if (itemInSlot == null)
         {
            SpawnNewItem(item,slot);
            return true;
         }
      }

      return false;
   }

   public void BuyItem(Item item)
   {
      if (money >= item.Value * 2)
      {
         bool added = AddItem(item);
         if (added)
         {
            money -= item.Value * 2;
            UpdatedMoneyEvent?.Invoke();
         }
      }
   }

   public void UpdateMoney(int updateAmount)
   {

      money += updateAmount;
      UpdatedMoneyEvent?.Invoke();
   }

   public void SellItem(InventoryItem invItem)
   {
      money += (invItem.item.Value * invItem.count);
      UpdatedMoneyEvent?.Invoke();
      Destroy(invItem.gameObject);
   }

   private void SpawnNewItem(Item item, InventorySlot inventorySlot)
   {
      GameObject newItemGo = Instantiate(inventoryItemPrefab, inventorySlot.transform);
      InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
      inventoryItem.InitialiseItem(item);
   }
}
