using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : GenericSingletonClass<GameManager>
{
    [SerializeField] private string interactableTag = "Interactable";

    public List<Soil> Soils;
    public List<HarvestableCrop> HarvestedCrops;
    public RaycastHit Hit;
    private Transform player;
    public TextMeshProUGUI tmpPrompt;
    public int Stamina = 100;
    public static event Action EndDayEvent;

    [FormerlySerializedAs("_selection")] public Transform Selection;
    
    private void Start()
    {
        Soils = new List<Soil>();
        HarvestedCrops = new List<HarvestableCrop>();
    }

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
    
    public void AddSoil(Soil soil)
    {
        Soils.Add(soil);
    }

    public void EndDay()
    {
        Debug.Log("Ending day.");
        EndDayEvent?.Invoke();
    }
}