using System.Collections;
using UnityEngine;

public class OpenGripper : MonoBehaviour
{
    
    public GameObject Franka;
    private ArticulationBody[] jointArticulationBodies; // Array of ArticulationBody components for each joint
    public float jointAssignmentWait = 0.1f; // Time to wait after setting each joint position


    void Start()
    {
        jointArticulationBodies = new ArticulationBody[FrankaConstants.NumFingers];
        
        for (var i = 0; i < FrankaConstants.NumFingers; i++)
        {
            var linkName = FrankaConstants.FingerName[i];
            jointArticulationBodies[i] = Franka.transform.Find(linkName).GetComponent<ArticulationBody>();
        }

        // Start moving the joints
        StartCoroutine(MoveJointsToTargetPositions());
    }

    private IEnumerator MoveJointsToTargetPositions()
    {
        for (int jointIndex = 0; jointIndex < FrankaConstants.NumFingers; jointIndex++)
        {
            if (jointIndex < jointArticulationBodies.Length)
            {
                var jointXDrive = jointArticulationBodies[jointIndex].xDrive;
                jointXDrive.target = FrankaConstants.FingerOpen; // Convert to degrees
                jointArticulationBodies[jointIndex].xDrive = jointXDrive;

                // Wait for a bit before moving to the next joint
                yield return new WaitForSeconds(jointAssignmentWait);
            }
        }
    }
}
