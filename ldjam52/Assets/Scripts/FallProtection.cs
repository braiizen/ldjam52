using System;
using UnityEngine;

public class FallProtection : MonoBehaviour
{
    public GameObject respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGERED: " + other.gameObject.name);
        other.gameObject.transform.position = new Vector3(0f, 40f, 0f);
    }

}
