using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour , IInteractable
{
    public string InteractPrompt => "Press E to use Laptop";
    public bool TryInteract(Item item = null)
    {
        throw new System.NotImplementedException();
    }
}
