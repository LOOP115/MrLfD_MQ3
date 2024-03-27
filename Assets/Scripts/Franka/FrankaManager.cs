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
    public List<GameObject> binaryToggles;

    private MoveBase moveBase;
    private MoveToStart moveToStart;
    private GripperController gripperController;
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
            franka = Instantiate(frankaPrefab, handPosition, Quaternion.Euler(0, 180, 0));

            moveBase = franka.GetComponent<MoveBase>();
            moveToStart = franka.GetComponent<MoveToStart>();
            gripperController = franka.GetComponent<GripperController>();
            jointController = franka.GetComponent<JointController>();

            isSpawned = true;

            // Reset all toggles
            ActivateAllToggles();
            ResetBinaryToggles();
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

    private void ResetBinaryToggles()
    {
        foreach (var toggle in binaryToggles)
        {
            ToggleImage toggleImage = toggle.GetComponent<ToggleImage>();
            if (toggleImage != null)
            {
                toggleImage.SetImage1Active();
            }
        }
    }

    private void DeactivateManager()
    {
        gameObject.SetActive(false);
    }

    public void setBase()
    {
        if (franka != null)
        {
            if (moveBase != null)
            {
                moveBase.enabled = !moveBase.enabled;
            }
        }
    }

    public void setMoveToStart()
    {
        if (franka != null)
        {
            if (moveToStart != null)
            {
                moveToStart.enabled = !moveToStart.enabled;
            }
        }
    }

    public void setGripper()
    {
        if (franka != null)
        {
            if (gripperController != null)
            {
                gripperController.enabled = !gripperController.enabled;
            }
        }
    }

    public void setJointController()
    {
        if (franka != null)
        {
            if (jointController != null)
            {
                jointController.setController();
            }
        }
    }


    public void ResetFranka()
    {
        if (franka != null)
        {
            if (moveToStart != null && gripperController != null)
            {
                moveToStart.Reset();
                gripperController.Open();
            }
        }
    }

    public void OpenGripper()
    {
        if (franka != null)
        {
            if (gripperController != null)
            {
                gripperController.Open();
            }
        }
    }

    public void CloseGripper()
    {
        if (franka != null)
        {
            if (gripperController != null)
            {
                gripperController.Close();
            }
        }
    }

}
