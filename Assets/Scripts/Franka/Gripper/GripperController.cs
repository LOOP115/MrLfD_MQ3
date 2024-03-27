using System.Collections;
using UnityEngine;

public class GripperController : MonoBehaviour
{
    
    private ArticulationBody[] jointArticulationBodies; // Array of ArticulationBody components for each joint
    public float jointAssignmentWait = 0.001f; // Time to wait after setting each joint position


    void Start()
    {
        jointArticulationBodies = new ArticulationBody[FrankaConstants.NumFingers];
        
        for (var i = 0; i < FrankaConstants.NumFingers; i++)
        {
            var linkName = FrankaConstants.FingerName[i];
            jointArticulationBodies[i] = transform.Find(linkName).GetComponent<ArticulationBody>();
        }
        // Open the gripper on start
        Open();
    }


    public void Open()
    {
        StartCoroutine(OpenGripper());
    }

    public void Close()
    {
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

}
