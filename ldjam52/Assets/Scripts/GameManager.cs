using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : GenericSingletonClass<GameManager>
{
    [SerializeField] private string interactableTag = "Interactable";

    public List<Soil> Soils;
    public List<HarvestableCrop> HarvestedCrops;
    public RaycastHit Hit;
    private Transform player;
    
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
                Debug.Log(selection.name);
                Selection = selection;
            }
        }

        if (Selection != null)
        {
            OnSelect(Selection);
        }
    }
    
    public void OnSelect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    public void OnDeselect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
    
    public void AddSoil(Soil soil)
    {
        Soils.Add(soil);
    }

    public void EndDay()
    {
        foreach (Soil soil in Soils)
        {
            if (soil.crop != null)
            {
                soil.UpdateDay();
            }
        }
    }
}