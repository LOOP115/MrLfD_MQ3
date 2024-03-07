using UnityEngine;
using System.Collections.Generic;

public class ToggleBoxColliders : MonoBehaviour
{
    // List to hold the GameObjects with BoxCollider you want to toggle
    public List<GameObject> colliders = new List<GameObject>();

    // Method to toggle the enabled state of the BoxColliders
    public void ToggleColliders()
    {
        foreach (GameObject obj in colliders)
        {
            // Check if the GameObject has a BoxCollider component
            BoxCollider collider = obj.GetComponent<BoxCollider>();
            if (collider != null)
            {
                // Toggle the collider's enabled state
                collider.enabled = !collider.enabled;
            }
        }
    }
}
