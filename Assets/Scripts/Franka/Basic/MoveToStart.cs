using System.Collections;
using UnityEngine;
using RosMessageTypes.CtrlInterfaces;
using TMPro;


public class MoveToStart : MonoBehaviour
{
    
    private ArticulationBody[] jointArticulationBodies; // Array of ArticulationBody components for each joint
    public float jointAssignmentWait = 0.001f; // Time to wait after setting each joint position
    private FrankaJointsMsg homeJointsMsg = new FrankaJointsMsg();

    private RosConnector rosConnector;
    
    private TextMeshProUGUI textComponent;


    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();
        
        jointArticulationBodies = new ArticulationBody[FrankaConstants.NumJoints];
        
        var linkName = string.Empty;
        for (var i = 0; i < FrankaConstants.NumJoints; i++)
        {
            linkName += FrankaConstants.LinkNames[i];
            jointArticulationBodies[i] = transform.Find(linkName).GetComponent<ArticulationBody>();
        }

        ResetUnityFranka();

        for (var i = 0; i < FrankaConstants.NumJoints; i++)
        {
            homeJointsMsg.joints[i] = FrankaConstants.StartJointPositionsRadians[i];
        }


        GameObject cameraRig = GameObject.FindGameObjectWithTag("TextCanvas");
        textComponent = cameraRig.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ResetUnityFranka()
    {
        StartCoroutine(MoveJointsToStart());
    }

    private IEnumerator MoveJointsToStart()
    {
        for (int jointIndex = 0; jointIndex < FrankaConstants.NumJoints; jointIndex++)
        {
            if (jointIndex < jointArticulationBodies.Length)
            {
                var jointXDrive = jointArticulationBodies[jointIndex].xDrive;
                jointXDrive.target = Mathf.Rad2Deg * FrankaConstants.StartJointPositionsRadians[jointIndex]; // Convert to degrees
                jointArticulationBodies[jointIndex].xDrive = jointXDrive;
            }
        }
        yield return new WaitForSeconds(jointAssignmentWait);
    }

    public FrankaJointsMsg getHomeJoints()
    {
        return homeJointsMsg;
    }

    public void SendMoveToStartCMD()
    {
        rosConnector.GetBridge().Publish(FrankaConstants.topicUnityCommand, FrankaConstants.cmdMoveToStart);
        textComponent.text = "Resetting Franka ...";
        StartCoroutine(ClearTextAfterDelay());
    }

    private IEnumerator ClearTextAfterDelay(int seconds = 3)
    {
        yield return new WaitForSeconds(seconds); // Wait for 3 seconds
        textComponent.text = ""; // Clear the text
    }
    
}
