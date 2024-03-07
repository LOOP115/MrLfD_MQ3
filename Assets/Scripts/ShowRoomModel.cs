using System.Collections.Generic;
using UnityEngine;

public class ShowRoomModel : MonoBehaviour
{
    // List of GameObjects to toggle active status
    public List<GameObject> gameObjectsToToggle = new List<GameObject>();
    
    // List of GameObjects to toggle Mesh Renderer status
    public List<GameObject> renderersToToggle = new List<GameObject>();

    // Toggles the active state of all GameObjects in the gameObjectsToToggle list
    public void ToggleActiveStatus()
    {
        foreach (GameObject obj in gameObjectsToToggle)
        {
            if (obj != null)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
    }

    // Toggles the enabled state of the Mesh Renderer component of all GameObjects in the renderersToToggle list
    public void ToggleRendererStatus()
    {
        foreach (GameObject obj in renderersToToggle)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = !renderer.enabled;
            }
        }
    }
}
