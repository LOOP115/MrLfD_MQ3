using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Franka.Control;
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

    public GameObject removeToggle;
    public GameObject resetToggle;
    public GameObject baseLockToggle;
    public GameObject jointControllerToggle;
    public GameObject reachTargetToggle;
    public GameObject followTargetToggle;
    public GameObject followTrajectoryToggle;


    public GameObject invisibleToggle;
    public GameObject jointDialsToggle;
    
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
    private SliderManager sliderManager;
    
    private RosConnector rosConnector;
    // private float fixedUpdateFPS = 30.0f;

    void Start()
    {
        modeToggles = new Dictionary<string, GameObject>
        {
            { FrankaConstants.BaseLock, baseLockToggle },
            { FrankaConstants.JointController, jointControllerToggle },
            { FrankaConstants.ReachTarget, reachTargetToggle },
            { FrankaConstants.FollowTarget, followTargetToggle },
            { FrankaConstants.JointDials, jointDialsToggle },
            { FrankaConstants.Invisible, invisibleToggle },
            { FrankaConstants.FollowTrajectory, followTrajectoryToggle}
        };

        modeActions = new Dictionary<string, Action>
        {
            { FrankaConstants.BaseLock, toggleBaseLock },
            { FrankaConstants.JointController, toggleJointController },
            { FrankaConstants.ReachTarget, toggleReachTarget },
            { FrankaConstants.FollowTarget, toggleFollowTarget },
            { FrankaConstants.JointDials, toggleJointDials },
            { FrankaConstants.Invisible, toggleFrankaVisibility },
            { FrankaConstants.FollowTrajectory, toggleFollowTrajectory }
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
            sliderManager = franka.GetComponent<SliderManager>();
            
            
            isSpawned = true;

            // syncFromFranka.Subscribe();
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
        if (rosConnector != null)
        {
            rosConnector.GetBridge().Publish(FrankaConstants.topicUnityCommand, FrankaConstants.cmdMoveToStart);
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
                    DeactivateTogglesExcept(new List<GameObject> {resetToggle, jointControllerToggle, jointDialsToggle, invisibleToggle});
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
                    DeactivateTogglesExcept(new List<GameObject> {resetToggle, reachTargetToggle, jointDialsToggle, invisibleToggle});
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
                    DeactivateTogglesExcept(new List<GameObject> {resetToggle, followTargetToggle, jointDialsToggle, invisibleToggle});
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
                    DeactivateTogglesExcept(new List<GameObject> {followTrajectoryToggle, jointDialsToggle, invisibleToggle});
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

    private void toggleJointDials()
    {
        if (franka != null)
        {
            if (sliderManager != null)
            {
                ToggleImage toggleImage = jointDialsToggle.GetComponent<ToggleImage>();
                if (toggleImage.Image1isActive())
                {
                    // sliderManager.Subscribe(true);
                    sliderManager.ActivateSliders();
                    DeactivateToggle(removeToggle);
                }
                else
                {
                    // sliderManager.Unsubscribe(true);
                    sliderManager.DeactivateSliders();
                    ActivateToggle(removeToggle);
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
