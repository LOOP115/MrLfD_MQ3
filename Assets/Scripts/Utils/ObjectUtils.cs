using UnityEngine;

public class ObjectUtils : MonoBehaviour
{
    public GameObject targetObject; // Assign the target GameObject in the Inspector

    // Call this method to activate the target GameObject
    public void ActivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Target object is not assigned.");
        }
    }

    // Call this method to deactivate the target GameObject
    public void DeactivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Target object is not assigned.");
        }
    }

    // // Call this method to pause an game object
    // public void PauseObject()
    // {
    //     if (targetObject != null)
    //     {
    //         targetObject.
    //     }
    //     else
    //     {
    //         Debug.LogError("Target object is not assigned.");
    //     }
    // }

}
