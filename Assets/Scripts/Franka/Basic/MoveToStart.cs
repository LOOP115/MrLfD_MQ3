using System.Collections;
using UnityEngine;

public class MoveToStart : MonoBehaviour
{
    
    private ArticulationBody[] jointArticulationBodies; // Array of ArticulationBody components for each joint
    public float jointAssignmentWait = 0.001f; // Time to wait after setting each joint position


    void Start()
    {
        jointArticulationBodies = new ArticulationBody[FrankaConstants.NumJoints];
        
        var linkName = string.Empty;
        for (var i = 0; i < FrankaConstants.NumJoints; i++)
        {
            linkName += FrankaConstants.LinkNames[i];
            jointArticulationBodies[i] = transform.Find(linkName).GetComponent<ArticulationBody>();
        }

        // Start moving the joints
        Reset();
    }

    public void Reset()
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

}
