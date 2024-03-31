using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Franka.Control;
using Unity.VisualScripting;
using System;

public class FrankaManager : MonoBehaviour
{
    public GameObject frankaPrefab;
    private GameObject franka;
    private bool isSpawned = false;
    public TextMeshProUGUI textComponent;
    public List<GameObject> monoToggles;
    public List<GameObject> dualToggles;
    
    private List<GameObject> toggles
    {
        get
        {
            List<GameObject> toggles = new List<GameObject>(monoToggles);
            toggles.AddRange(dualToggles);
            return toggles;
        }
    }

    public GameObject resetToggle;
    public GameObject baseLockToggle;
    public GameObject jointControllerToggle;
    public GameObject followTargetToggle;

    private Dictionary<string, Action> modeActions;
    private Dictionary<string, GameObject> modeToggles;

    private JointController jointController;
    private GripperController gripperController;
    private MoveToStart moveToStart;
    private MoveBase moveBase;
    private EndEffectorTarget endEffectorTarget;
    private SyncFromFranka syncFromFranka;
    private JointsPublisher jointsPublisher;


    void Start()
    {
        modeToggles = new Dictionary<string, GameObject>
        {
            { FrankaConstants.BaseLock, baseLockToggle },
            { FrankaConstants.JointController, jointControllerToggle },
            { FrankaConstants.FollowTarget, followTargetToggle }
        };

        modeActions = new Dictionary<string, Action>
        {
            { FrankaConstants.BaseLock, toggleBaseLock },
            { FrankaConstants.JointController, toggleJointController },
            { FrankaConstants.FollowTarget, toggleFollowTarget }
        };
    }
    
    
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

            ActivateToggles();
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
            DeactivateTogglesExcept();
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
    private void ActivateToggles()
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
    private void DeactivateTogglesExcept(List<GameObject> togglesToMaintain = null)
    {
        foreach (var toggle in toggles)
        {
            if (toggle != null)
            {
                if (togglesToMaintain == null || !togglesToMaintain.Contains(toggle))
                {
                    Toggle toggleComponent = toggle.GetComponent<Toggle>();
                    if (toggleComponent != null)
                    {
                        toggleComponent.interactable = false;
                    }
                }
            }
        }
    }

    private void ResetBinaryToggles()
    {
        foreach (var toggle in dualToggles)
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


    public void toggleMode(string mode)
    {
        if (franka == null)
        {
            return;
        }

        if (modeActions.TryGetValue(mode, out Action toggleAction))
        {
            // turnOffModesExcept(new List<string> {mode});
            toggleAction.Invoke();
        }
        else
        {
            Debug.LogError("Invalid mode: " + mode);
        }
    }

    private void turnOffModesExcept(List<string> modesToMaintain)
    {
        foreach (var mode in FrankaConstants.Modes)
        {
            if (!modesToMaintain.Contains(mode))
            {
                turnOffMode(mode);
            }
        }
    }

    private void turnOffMode(string mode)
    {
        if (modeToggles.TryGetValue(mode, out GameObject toggleObject) && modeActions.TryGetValue(mode, out Action toggleAction))
        {
            ToggleImage toggleImage = toggleObject.GetComponent<ToggleImage>();
            if (!toggleImage.Image1isActive())
            {
                toggleAction.Invoke();
            }
        }
        else
        {
            Debug.LogError("Invalid mode: " + mode);
        }
    }

    private void toggleBaseLock()
    {
        if (franka != null)
        {
            if (baseLockToggle != null)
            {
                ToggleImage toggleImage = baseLockToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    moveBase.enabled = true;
                    DeactivateTogglesExcept(new List<GameObject> {baseLockToggle});
                }
                else
                {
                    moveBase.enabled = false;
                    ActivateToggles();
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }

    private void toggleJointController()
    {
        if (franka != null)
        {
            if (jointControllerToggle != null)
            {
                ToggleImage toggleImage = jointControllerToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    jointController.setControllerState(true);
                    DeactivateTogglesExcept(new List<GameObject> {resetToggle, jointControllerToggle});
                }
                else
                {
                    jointController.setControllerState(false);
                    ActivateToggles();
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }

    private void toggleFollowTarget()
    {
        if (franka != null)
        {
            if (followTargetToggle != null)
            {
                ToggleImage toggleImage = followTargetToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    startFollowTarget();
                    DeactivateTogglesExcept(new List<GameObject> {followTargetToggle});
                }
                else
                {
                    stopFollowTarget();
                    ActivateToggles();
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
