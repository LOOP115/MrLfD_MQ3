using System.Collections;
using UnityEngine;

public class GripperController : MonoBehaviour
{
    
    private ArticulationBody[] jointArticulationBodies; // Array of ArticulationBody components for each joint
    public float jointAssignmentWait = 0.001f; // Time to wait after setting each joint position

    private bool isGripperClosed = false;
    private bool controllerIsActive = false;

    private RosConnector rosConnector;


    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();

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
        StartCoroutine(OpenGripper());
        rosConnector.GetBridge().Publish(FrankaConstants.topicUnityCommand, FrankaConstants.cmdGripperHome);
    }

    public void Close()
    {
        StartCoroutine(CloseGripper());
        rosConnector.GetBridge().Publish(FrankaConstants.topicUnityCommand, FrankaConstants.cmdGripperGrasp);
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
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (isGripperClosed)
            {
                Open();
                isGripperClosed = false;
            }
            else
            {
                Close();
                isGripperClosed = true;
            }
        }
    }
    
}
