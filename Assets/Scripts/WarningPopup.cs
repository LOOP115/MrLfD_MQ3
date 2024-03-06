using UnityEngine;

public class WarningPopup : MonoBehaviour
{
    public GameObject popup;
    public Transform playerHead;

    private void Start()
    {
        // Initially hide the popup
        popup.SetActive(false);
    }

    public void ShowWarning()
    {
        // Show the popup
        popup.SetActive(true);

        // Ensure the popup always faces the player
        if(playerHead != null)
        {
            // popup.transform.LookAt(playerHead);
            // Optionally, adjust the rotation so it faces directly at the player
            popup.transform.rotation = Quaternion.LookRotation(popup.transform.position - playerHead.position);
        }
    }

    private void Update()
    {
        // Optionally, make the popup always face the player even if they move
        if (popup.activeSelf && playerHead != null)
        {
            popup.transform.LookAt(playerHead);
            // Adjust as above if needed
        }
    }
}
