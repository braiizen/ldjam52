using UnityEngine;

public class Cat : MonoBehaviour, IInteractable
{
    private string[] prompts = { "Who's a good kitty", "Pet", "Meow" };
    private int index;
    public AudioSource meow;
    
    private string ChangePrompt()
    {
        index++;
        if (index > prompts.Length - 1)
            index = 0;
        return prompts[index];
    }

    public string InteractPrompt => prompts[index];
    public bool TryInteract(Item item = null)
    {
        meow.pitch = Random.Range(0.5f, 1.5f);
        meow.Play();
        ChangePrompt();
        return false;
    }
}
