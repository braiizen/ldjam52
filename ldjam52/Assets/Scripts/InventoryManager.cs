using UnityEngine;

public class InventoryManager : GenericSingletonClass<InventoryManager>
{
   public Item[] startItems;
   public GameObject inventoryItemPrefab;
   public GameObject InventoryUI;
   public GameObject crosshair;
   public bool openInventory = false;
   
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
      Cursor.visible = !openInventory;
      if (Input.inputString != null)
      {
         bool isNumber = int.TryParse(Input.inputString, out int number);
         if (isNumber && number > 0 && number < 8)
            ChangeSelectedSlot(number - 1);
      }
      
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         openInventory = !openInventory;
         crosshair.SetActive(!openInventory);
         InventoryUI.SetActive(openInventory);
         Cursor.lockState = openInventory ? CursorLockMode.None : CursorLockMode.Locked;
         Time.timeScale = openInventory ? 0f : 1f;
      }
      
      if (Input.GetMouseButtonDown(0))
      {
         if (GameManager.Instance.Selection != null)
         {
            var heldItem = GetSelectedItem(false);
            var interactable = GameManager.Instance.Selection.GetComponent<IInteractable>();
            bool interacted = interactable.TryInteract(heldItem);
            if(interacted)
               GetSelectedItem(true);
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

   private void SpawnNewItem(Item item, InventorySlot inventorySlot)
   {
      GameObject newItemGo = Instantiate(inventoryItemPrefab, inventorySlot.transform);
      InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
      inventoryItem.InitialiseItem(item);
   }
}
