using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Franka.Control;
using System;
using UnityEditor;

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


    public GameObject removeToggle;
    public GameObject resetToggle;

    public GameObject baseLockToggle;
    public GameObject gripperControllerToggle;
    public GameObject jointControllerToggle;
    public GameObject reachTargetToggle;
    public GameObject followTargetToggle;
    public GameObject followTrajectoryToggle;

    public GameObject invisibleToggle;
    public GameObject planeToggle;

    public GameObject JointPositionsToggle;
    public GameObject ManipulabilityToggle;
    

    private List<GameObject> togglesKeepActive;


    private Dictionary<string, Action> modeActions;
    private Dictionary<string, GameObject> modeToggles;


    private JointController jointController;
    private GripperController gripperController;
    private MoveToStart moveToStart;
    private SyncFromFranka syncFromFranka;
    
    private MoveBase moveBase;
    private ReachTarget reachTarget;
    private FollowTarget followTarget;
    private FollowTrajectory followTrajectory;
    // private JointsPublisher jointsPublisher;
    
    private InvisibleFranka invisibleFranka;
    private PlaneManager planeManager;

    private VisualiseJointPositions visJointPos;
    private VisualiseManipulability visManip;
    
    private RosConnector rosConnector;
    // private float fixedUpdateFPS = 30.0f;

    void Start()
    {
        modeToggles = new Dictionary<string, GameObject>
        {
            { FrankaConstants.BaseLock, baseLockToggle },
            { FrankaConstants.GripperController, gripperControllerToggle },
            { FrankaConstants.JointController, jointControllerToggle },
            { FrankaConstants.ReachTarget, reachTargetToggle },
            { FrankaConstants.FollowTarget, followTargetToggle },
            { FrankaConstants.FollowTrajectory, followTrajectoryToggle},
            { FrankaConstants.Invisible, invisibleToggle },
            { FrankaConstants.Plane, planeToggle},
            { FrankaConstants.JointPostions, JointPositionsToggle },
            { FrankaConstants.Manipulability, ManipulabilityToggle }
        };

        modeActions = new Dictionary<string, Action>
        {
            { FrankaConstants.BaseLock, toggleBaseLock },
            { FrankaConstants.GripperController, toggleGripperController },
            { FrankaConstants.JointController, toggleJointController },
            { FrankaConstants.ReachTarget, toggleReachTarget },
            { FrankaConstants.FollowTarget, toggleFollowTarget },
            { FrankaConstants.FollowTrajectory, toggleFollowTrajectory },
            { FrankaConstants.Invisible, toggleFrankaVisibility },
            { FrankaConstants.Plane, togglePlane },
            { FrankaConstants.JointPostions, toggleJointPositionsVisual },
            { FrankaConstants.Manipulability, toggleManipulabilitVisual }
        };

        togglesKeepActive = new List<GameObject> 
        {
            resetToggle,
            gripperControllerToggle,
            invisibleToggle,
            planeToggle,
            JointPositionsToggle,
            ManipulabilityToggle
        };

        rosConnector = FindObjectOfType<RosConnector>();
        // Time.fixedDeltaTime = 1.0f / fixedUpdateFPS;
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
            handPosition.x -= 0.0033f;
            handPosition.y -= 0.1474954f;
            handPosition.z -= 0.104012f;

            franka = Instantiate(frankaPrefab, handPosition, Quaternion.Euler(0, 180, 0));

            jointController = franka.GetComponent<JointController>();
            gripperController = franka.GetComponent<GripperController>();
            moveToStart = franka.GetComponent<MoveToStart>();
            syncFromFranka = franka.GetComponent<SyncFromFranka>();
            
            moveBase = franka.GetComponent<MoveBase>();
            reachTarget = franka.GetComponent<ReachTarget>();
            followTarget = franka.GetComponent<FollowTarget>();
            followTrajectory = franka.GetComponent<FollowTrajectory>();
            // jointsPublisher = franka.GetComponent<JointsPublisher>();

            invisibleFranka = franka.GetComponent<InvisibleFranka>();
            planeManager = franka.GetComponent<PlaneManager>();

            visJointPos = franka.GetComponent<VisualiseJointPositions>();
            visManip = franka.GetComponent<VisualiseManipulability>();
            
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
            visManip.Unsubscribe();
            Destroy(franka);
            isSpawned = false;
            DeactivateTogglesExcept();
            ResetBinaryToggles();
        }
    }

    public void ResetFranka()
    {
        if (rosConnector != null)
        {
            moveToStart.SendMoveToStartCMD();
            // rosConnector.GetBridge().Publish(FrankaConstants.topicUnityFrankaJoints, moveToStart.getHomeJoints());
        }
    }

    public void ResetUnityFranka()
    {
        if (franka != null)
        {
            if (moveToStart != null && gripperController != null)
            {
                // moveToStart.Reset();
                gripperController.Open();
            }
        }
    }

    private void ActivateToggle(GameObject toggle)
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

    private void DeactivateToggle(GameObject toggle)
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

    private void toggleGripperController()
    {
        if (franka != null)
        {
            if (gripperControllerToggle != null)
            {
                ToggleImage toggleImage = gripperControllerToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    gripperController.SetControllerStatus(true);
                }
                else
                {
                    gripperController.SetControllerStatus(false);
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
                    syncFromFranka.Unsubscribe();
                    jointController.setControllerState(true);
                    DeactivateTogglesExcept(new List<GameObject> (togglesKeepActive) {jointControllerToggle});
                }
                else
                {
                    syncFromFranka.Subscribe();
                    jointController.setControllerState(false);
                    ActivateToggles();
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }

    private void toggleReachTarget()
    {
        if (franka != null)
        {
            if (reachTargetToggle != null)
            {
                ToggleImage toggleImage = reachTargetToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    startReachTarget();
                    DeactivateTogglesExcept(new List<GameObject> (togglesKeepActive) {reachTargetToggle});
                }
                else
                {
                    stopReachTarget();
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
                    DeactivateTogglesExcept(new List<GameObject> (togglesKeepActive) {followTargetToggle});
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

    private void toggleFollowTrajectory()
    {
        if (franka != null)
        {
            if (followTrajectoryToggle != null)
            {
                ToggleImage toggleImage = followTrajectoryToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    startFollowTrajectory();
                    DeactivateTogglesExcept(new List<GameObject> (togglesKeepActive) {followTrajectoryToggle});
                }
                else
                {
                    stopFollowTrajectory();
                    ActivateToggles();
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }

    private void toggleFrankaVisibility()
    {
        if (franka != null)
        {
            if (invisibleFranka != null)
            {
                ToggleImage toggleImage = invisibleToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    invisibleFranka.SetVisibility(false);
                }
                else
                {
                    invisibleFranka.SetVisibility(true);
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }

    private void togglePlane()
    {
        if (franka != null)
        {
            if (planeManager != null)
            {
                ToggleImage toggleImage = planeToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    planeManager.ActivatePlane();
                }
                else
                {
                    planeManager.DeactivatePlane();
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }

    private void toggleJointPositionsVisual()
    {
        if (franka != null)
        {
            if (visJointPos != null)
            {
                ToggleImage toggleImage = JointPositionsToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    // sliderManager.Subscribe(true);
                    visJointPos.ActivateSliders();
                    // DeactivateToggle(removeToggle);
                }
                else
                {
                    // sliderManager.Unsubscribe(true);
                    visJointPos.DeactivateSliders();
                    // ActivateToggle(removeToggle);
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }

    public void toggleManipulabilitVisual()
    {
        if (franka != null)
        {
            if (visManip != null)
            {
                ToggleImage toggleImage = ManipulabilityToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    visManip.ActivateSliders();
                }
                else
                {
                    visManip.DeactivateSliders();
                }
                toggleImage.SwitchToggleImage();
            }
        }
    }


    private void startReachTarget()
    {
        if (franka != null)
        {
            if (reachTarget != null && syncFromFranka != null)
            {
                reachTarget.enabled = true;
                // syncFromFranka.Subscribe();
            }
        }
    }

    private void stopReachTarget()
    {
        if (franka != null)
        {
            if (reachTarget != null && syncFromFranka != null)
            {
                reachTarget.RemoveTarget();
                reachTarget.enabled = false;
                // syncFromFranka.Unsubscribe();
            }
        }
    }

    private void startFollowTarget()
    {
        if (franka != null)
        {
            if (followTarget != null && syncFromFranka != null)
            {
                followTarget.enabled = true;
                // syncFromFranka.Subscribe();
            }
        }
    }

    private void stopFollowTarget()
    {
        if (franka != null)
        {
            if (followTarget != null && syncFromFranka != null)
            {
                followTarget.RemoveTarget();
                followTarget.enabled = false;
                // syncFromFranka.Unsubscribe();
            }
        }
    }


    private void startFollowTrajectory()
    {
        if (franka != null)
        {
            if (followTrajectory != null)
            {
                followTrajectory.SpawnFrankaIK();
                // syncFromFranka.Subscribe();
            }
        }
    }

    private void stopFollowTrajectory()
    {
        if (franka != null)
        {
            if (followTrajectory != null)
            {
                followTrajectory.RemoveFrankaIK();
                // syncFromFranka.Unsubscribe();
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
