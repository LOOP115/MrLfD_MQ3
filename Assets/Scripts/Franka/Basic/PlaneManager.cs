using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    // Path to the GameObject in the hierarchy
    private string planePath = "world/Plane";
    private GameObject plane;

    void Start()
    {
        // Find the GameObject at the start
        plane = transform.Find(planePath)?.gameObject;

        // Check if the object was found
        if (plane == null)
        {
            Debug.LogError("No GameObject found at path: " + planePath);
        }
    }

    // Call this function to activate the GameObject
    public void ActivatePlane()
    {
        if (plane != null)
        {
            plane.SetActive(true);
        }
        else
        {
            Debug.LogError("GameObject not found. Cannot activate.");
        }
    }

    // Call this function to deactivate the GameObject
    public void DeactivatePlane()
    {
        if (plane != null)
        {
            plane.SetActive(false);
        }
        else
        {
            Debug.LogError("GameObject not found. Cannot deactivate.");
        }
    }
}
