using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrankaManager : MonoBehaviour
{
    // public bool isPaused = false;
    public GameObject frankaPrefab;
    public TextMeshProUGUI textComponent;
    private List<GameObject> frankas = new List<GameObject>();


    void Update()
    {
        // if (isPaused)
        // {
        //     return;
        // }
        
        textComponent.text = "Move your right hand and press A to\nspawn Franka at the position of your right controller.";

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SpawnFranka();
            // Deactivate this GameObject after the spawn operation is complete
            textComponent.text = "";
            DeactivateManager();
        }
    }

    private void SpawnFranka()
    {
        if (frankaPrefab != null)
        {
            Vector3 handPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            GameObject franka = Instantiate(frankaPrefab, handPosition, Quaternion.identity);
            frankas.Add(franka);
        }
        else
        {
            Debug.LogError("Prefab or RightHandAnchor is not set.");
        }
    }

    public void DeleteLastFranka()
    {
        if (frankas.Count > 0)
        {
            GameObject toDelete = frankas[frankas.Count - 1];
            frankas.RemoveAt(frankas.Count - 1);
            Destroy(toDelete);
        }
    }

    public void ClearAllFrankas()
    {
        foreach (GameObject franka in frankas)
        {
            Destroy(franka);
        }
        frankas.Clear(); // Clear the list after destroying all spheres
    }
    
    private void DeactivateManager()
    {
        gameObject.SetActive(false);
    }
}
