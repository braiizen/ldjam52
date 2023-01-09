using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour ,IInteractable
{
    public string InteractPrompt => "Press E to go to next day.";

    public bool TryInteract(Item item)
    {

        Debug.Log("Going to next day.");
        GameManager.Instance.EndDay();
        return false;
    }
}
