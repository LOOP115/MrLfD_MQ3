using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    // Reference to the panel GameObject
    public GameObject panel;

    // Function to toggle the panel's visibility
    public void TogglePanelVisibility()
    {
        if (panel != null)
        {
            // Toggle the active state of the panel
            panel.SetActive(!panel.activeSelf);
        }
    }
}
