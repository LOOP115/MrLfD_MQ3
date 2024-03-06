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

    // Array to hold references to all panel GameObjects you want to control
    public GameObject[] panels;

    // Method to deactivate all panels
    
    public void ActivateAllPanels()
    {
        foreach (var panel in panels)
        {
            if (!panel.activeSelf) // Check if the panel is active
            {
                panel.SetActive(true); // Deactivate the panel
            }
        }
    }
    
    public void DeactivateAllPanels()
    {
        foreach (var panel in panels)
        {
            if (panel.activeSelf) // Check if the panel is active
            {
                panel.SetActive(false); // Deactivate the panel
            }
        }
    }

    // URL to open
    // public string url = "https://www.google.com/";

    // // Call this method to open the URL in the default web browser
    // public void OpenURL()
    // {
    //     Application.OpenURL(url);
    // }

}
