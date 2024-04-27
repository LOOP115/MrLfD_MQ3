using System.Collections;
using UnityEngine;
using TMPro;

public class GripperController : MonoBehaviour
{
    
    private ArticulationBody[] jointArticulationBodies; // Array of ArticulationBody components for each joint
    public float jointAssignmentWait = 0.001f; // Time to wait after setting each joint position

    private bool isGripperClosed = false;
    private bool controllerIsActive = false;

    

    private RosConnector rosConnector;
    
    private TextMeshProUGUI textComponent;


    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();

        GameObject cameraRig = GameObject.FindGameObjectWithTag("TextCanvas");
        textComponent = cameraRig.GetComponentInChildren<TextMeshProUGUI>();

        jointArticulationBodies = new ArticulationBody[FrankaConstants.NumFingers];
        
        for (var i = 0; i < FrankaConstants.NumFingers; i++)
        {
            var linkName = FrankaConstants.FingerName[i];
            jointArticulationBodies[i] = transform.Find(linkName).GetComponent<ArticulationBody>();
        }
        // Open the gripper on start
        Open();
    }


    void Update()
    {
        if (controllerIsActive)
        {
            toggleGripper();
        }
        
    }


    public void SetControllerStatus(bool status)
    {
        controllerIsActive = status;
    }


    public void Open()
    {
        rosConnector.GetBridge().Publish(FrankaConstants.topicUnityCommand, FrankaConstants.cmdGripperHome);
        StartCoroutine(OpenGripper());
    }

    public void Close()
    {
        rosConnector.GetBridge().Publish(FrankaConstants.topicUnityCommand, FrankaConstants.cmdGripperGrasp);
        StartCoroutine(CloseGripper());
    }
    

    private IEnumerator OpenGripper()
    {
        for (int jointIndex = 0; jointIndex < FrankaConstants.NumFingers; jointIndex++)
        {
            if (jointIndex < jointArticulationBodies.Length)
            {
                var jointXDrive = jointArticulationBodies[jointIndex].xDrive;
                jointXDrive.target = FrankaConstants.FingerOpen;
                jointArticulationBodies[jointIndex].xDrive = jointXDrive;
            }
        }
        yield return new WaitForSeconds(jointAssignmentWait);
    }


    private IEnumerator CloseGripper()
    {
        for (int jointIndex = 0; jointIndex < FrankaConstants.NumFingers; jointIndex++)
        {
            if (jointIndex < jointArticulationBodies.Length)
            {
                var jointXDrive = jointArticulationBodies[jointIndex].xDrive;
                jointXDrive.target = FrankaConstants.FingerClose;
                jointArticulationBodies[jointIndex].xDrive = jointXDrive;
            }
        }
        yield return new WaitForSeconds(jointAssignmentWait);
    }

    private void toggleGripper()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.0f || OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.0f)
            {
                textComponent.text = "Please release your hand trigger before using the gripper!";
                // StartCoroutine(ClearTextAfterDelay(1)); // Start the coroutine to clear text
                return;
            }
            if (isGripperClosed)
            {
                Open();
                isGripperClosed = false;
                textComponent.text = "Gripper Homing ...";
                StartCoroutine(ClearTextAfterDelay()); // Start the coroutine to clear text
            }
            else
            {
                Close();
                isGripperClosed = true;
                textComponent.text = "Gripper Grasp ...";
                StartCoroutine(ClearTextAfterDelay()); // Start the coroutine to clear text
            }
        }
    }

    private IEnumerator ClearTextAfterDelay(int seconds = 3)
    {
        yield return new WaitForSeconds(seconds); // Wait for 3 seconds
        textComponent.text = ""; // Clear the text
    }

    
}
