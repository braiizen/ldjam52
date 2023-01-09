using UnityEngine;

public class Radio : MonoBehaviour , IInteractable
{
    public AudioClip[] songs;
    
    private bool turnedOn = true;
    public AudioSource music;
    private int index = 0;
    public string InteractPrompt => turnedOn ? "Turn off radio." : "Turn on radio.";
    public bool TryInteract(Item item = null)
    {
        if (turnedOn)
            music.Stop();
        else
        {
            music.clip = songs[index];
            ChangeIndex();
            music.Play();
        }

        return false;
    }

    private void ChangeIndex()
    {
        index++;
        if (index > songs.Length - 1)
            index = 0;
    }
}
