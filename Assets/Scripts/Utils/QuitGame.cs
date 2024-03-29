using UnityEngine;
using TMPro;


public class QuitGame : MonoBehaviour
{
    
    public TextMeshProUGUI textComponent;

    void Update()
    {
        textComponent.text = "Press A to exit.\nPress B to cancel.";
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            textComponent.text = "";
            Quit();
        } else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            textComponent.text = "";
            gameObject.SetActive(false);
        }
    }
    

    // Method to quit the game
    public void Quit()
    {
        // If we are running in a standalone build of the game
        #if UNITY_STANDALONE
        Application.Quit();
        #endif

        // If we are running in the editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
