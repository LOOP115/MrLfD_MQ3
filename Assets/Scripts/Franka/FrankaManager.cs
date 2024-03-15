using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Franka.Control;

public class FrankaManager : MonoBehaviour
{
    public GameObject frankaPrefab;
    public TextMeshProUGUI textComponent;
    private bool isSpawned = false;
    private GameObject franka;
    public List<GameObject> toggles;
    private MoveBase moveBase;
    private JointController jointController;

    

    void Update()
    {
        if (isSpawned)
        {
            textComponent.text = "Only one Franka is allowed for now.\nPress A to exit.";
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                textComponent.text = "";
                DeactivateManager();
            }
        } else
        {
            textComponent.text = "Move your right hand and press A to\nspawn Franka at the position of your right controller.";
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                SpawnFranka();
                textComponent.text = "";
                DeactivateManager();
            }
        }
    }

    private void SpawnFranka()
    {
        if (frankaPrefab != null)
        {
            Vector3 handPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            franka = Instantiate(frankaPrefab, handPosition, Quaternion.identity);
            moveBase = franka.GetComponent<MoveBase>();
            jointController = franka.GetComponent<JointController>();
            isSpawned = true;
            ActivateAllToggles();
        }
        else
        {
            Debug.LogError("Prefab or RightHandAnchor is not set.");
        }
    }

    public void RemoveFranka()
    {
        if (franka != null)
        {
            Destroy(franka);
            isSpawned = false;
            DeactivateAllToggles();
        }
    }

    // Function to activate all toggles
    private void ActivateAllToggles()
    {
        foreach (var toggle in toggles)
        {
            if (toggle != null)
            {
                Toggle toggleComponent = toggle.GetComponent<Toggle>();
                if (toggleComponent != null)
                {
                    toggleComponent.interactable = true;
                }
            }
        }
    }

    // Function to deactivate all toggles
    private void DeactivateAllToggles()
    {
        foreach (var toggle in toggles)
        {
            if (toggle != null)
            {
                Toggle toggleComponent = toggle.GetComponent<Toggle>();
                if (toggleComponent != null)
                {
                    toggleComponent.interactable = false;
                }
            }
        }
    }

    public void setBase()
    {
        if (franka != null)
        {
            if (moveBase != null)
            {
                moveBase = franka.GetComponent<MoveBase>();
                moveBase.enabled = !moveBase.enabled;
            }
        }
    }

    public void setJointController()
    {
        if (franka != null)
        {
            if (jointController != null)
            {
                jointController = franka.GetComponent<JointController>();
                jointController.enabled = !jointController.enabled;
            }
        }
    }

    private void DeactivateManager()
    {
        gameObject.SetActive(false);
    }
}
