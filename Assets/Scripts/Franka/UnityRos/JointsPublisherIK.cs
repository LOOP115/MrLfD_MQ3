using UnityEngine;
using RosMessageTypes.CtrlInterfaces;
using System;


public class JointsPublisherIK : MonoBehaviour
{
    // ROS Connector
    private RosConnector rosConnector;
    public GameObject baseLink;
    private BioIK.BioIK bioIK;
    private int updateCount = 0;

    // public float PublishHz = 20.0f;
    // private float PublishFrequency => 1.0f / PublishHz;
    // private float timeElapsed;

    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();
        bioIK = baseLink.GetComponent<BioIK.BioIK>();
    }

    private void FixedUpdate()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) == 0.0f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) == 0.0f)
        {
            return;
        }
        
        if (updateCount % 2 == 0) {

            var jointsMsg = new FrankaJointsMsg();

            for (var i = 0; i < FrankaConstants.NumJoints; i++)
            {
                jointsMsg.joints[i] = formatBioIKSolution((float)bioIK.Solution[i], i) * Mathf.Deg2Rad;
            }

            rosConnector.GetBridge().Publish(FrankaConstants.topicUnityFrankaJoints, jointsMsg);
        }
        
        updateCount = (updateCount + 1) % 2;
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
