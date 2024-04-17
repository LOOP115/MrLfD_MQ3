using UnityEngine;
using System.Collections;


public class FrankaIK : MonoBehaviour
{
    private ArticulationBody[] jointArticulationBodies;

    // public float topicHz = 200.0f; // Frequency to check for new messages
    private float jointAssignmentWait = 0.001f; // Time to wait after setting each joint position
    
    
    public GameObject ikBaseLink;
    private BioIK.BioIK bioIK;

    // public float PublishHz = 20.0f;
    // private float PublishFrequency => 1.0f / PublishHz;
    // private float timeElapsed;

    void Start()
    {
        bioIK = ikBaseLink.GetComponent<BioIK.BioIK>();

        jointArticulationBodies = new ArticulationBody[FrankaConstants.NumJoints];
        
        var linkName = string.Empty;
        for (var i = 0; i < FrankaConstants.NumJoints; i++)
        {
            linkName += FrankaConstants.LinkNames[i];
            jointArticulationBodies[i] = transform.Find(linkName).GetComponent<ArticulationBody>();
        }

    }

    private void Update()
    {
        // timeElapsed += Time.deltaTime;
        UpdateJointPositions(bioIK);
        // timeElapsed = 0;
    }


    void UpdateJointPositions(BioIK.BioIK bioIK)
    {
        StartCoroutine(MoveJointsToTargetPositions(bioIK));
    }

    private IEnumerator MoveJointsToTargetPositions(BioIK.BioIK bioIK)
    {
        for (int jointIndex = 0; jointIndex < FrankaConstants.NumJoints; jointIndex++)
        {
            if (jointIndex < jointArticulationBodies.Length)
            {
                var jointXDrive = jointArticulationBodies[jointIndex].xDrive;
                jointXDrive.target = formatBioIKSolution((float)bioIK.Solution[jointIndex], jointIndex); // Convert to degrees
                jointArticulationBodies[jointIndex].xDrive = jointXDrive;
            }
        }
        yield return new WaitForSeconds(jointAssignmentWait);
    }

    private float formatBioIKSolution(float solution, int jointIndex)
    {
        float res = solution * Mathf.Rad2Deg;
        if (jointIndex ==  1)
        {
            return res -45f;
        }
        if (jointIndex ==  3)
        {
            return res -= 135f;
        }
        if (jointIndex ==  5)
        {
            return res += 90f;
        }
        if (jointIndex ==  6)
        {
            return res += 45f;
        }
        return res;
    }
    
}
