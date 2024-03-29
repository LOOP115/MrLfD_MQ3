using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Franka.Control;
using Unity.VisualScripting;

public class FrankaManager : MonoBehaviour
{
    public GameObject frankaPrefab;
    public TextMeshProUGUI textComponent;
    public List<GameObject> toggles;
    public List<GameObject> binaryToggles;
    
    public Toggle moveBaseToggle;
    public Toggle jointControllerToggle;
    public Toggle followTargetToggle;
    
    private GameObject franka;
    private bool isSpawned = false;

    private JointController jointController;
    private GripperController gripperController;
    private MoveToStart moveToStart;
    private MoveBase moveBase;
    private EndEffectorTarget endEffectorTarget;
    private SyncFromFranka syncFromFranka;
    private JointsPublisher jointsPublisher;

    
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

            jointController = franka.GetComponent<JointController>();
            gripperController = franka.GetComponent<GripperController>();
            moveToStart = franka.GetComponent<MoveToStart>();
            moveBase = franka.GetComponent<MoveBase>();
            endEffectorTarget = franka.GetComponent<EndEffectorTarget>();
            syncFromFranka = franka.GetComponent<SyncFromFranka>();
            jointsPublisher = franka.GetComponent<JointsPublisher>();
            
            isSpawned = true;

            // Reset all toggles
            ActivateAllToggles();
            // ResetBinaryToggles();
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
            syncFromFranka.Unsubscribe();
            Destroy(franka);
            isSpawned = false;
            DeactivateAllToggles();
            ResetBinaryToggles();
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

    
    // private void stopAllControllersExcept(Toggle maintain)
    // {
    //     if (franka != null)
    //     {
    //         if (jointController != null)
    //         {
    //             jointController.setControllerState(false);
    //         }
    //         if (moveBase != null)
    //         {
    //             moveBase.enabled = false;
    //         }
    //         if (endEffectorTarget != null)
    //         {
    //             endEffectorTarget.RemoveTarget();
    //             endEffectorTarget.enabled = false;
    //             syncFromFranka.Unsubscribe();
    //         }
    //     }
    // }


    public void toggleMoveBase()
    {
        if (franka != null)
        {
            if (moveBaseToggle != null)
            {
                ToggleImage toggleImage = moveBaseToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    moveBase.enabled = true;
                }
                else
                {
                    moveBase.enabled = false;
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }

    public void toggleJointController()
    {
        if (franka != null)
        {
            if (jointControllerToggle != null)
            {
                ToggleImage toggleImage = jointControllerToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    jointController.setControllerState(true);
                }
                else
                {
                    jointController.setControllerState(false);
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }

    public void toggleFollowTarget()
    {
        if (franka != null)
        {
            if (followTargetToggle != null)
            {
                ToggleImage toggleImage = followTargetToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    startFollowTarget();
                }
                else
                {
                    stopFollowTarget();
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }
    
    private void startFollowTarget()
    {
        if (franka != null)
        {
            if (endEffectorTarget != null && syncFromFranka != null)
            {
                endEffectorTarget.enabled = true;
                syncFromFranka.Subscribe();
            }
        }
    }

    private void stopFollowTarget()
    {
        if (franka != null)
        {
            if (endEffectorTarget != null && syncFromFranka != null)
            {
                endEffectorTarget.RemoveTarget();
                endEffectorTarget.enabled = false;
                syncFromFranka.Unsubscribe();
            }
        }
    }


    // public void OpenGripper()
    // {
    //     if (franka != null)
    //     {
    //         if (gripperController != null)
    //         {
    //             gripperController.Open();
    //         }
    //     }
    // }

    // public void CloseGripper()
    // {
    //     if (franka != null)
    //     {
    //         if (gripperController != null)
    //         {
    //             gripperController.Close();
    //         }
    //     }
    // }

}
