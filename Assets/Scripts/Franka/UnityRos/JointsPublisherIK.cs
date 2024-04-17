using UnityEngine;
using RosMessageTypes.CtrlInterfaces;
using System;


public class JointsPublisherIK : MonoBehaviour
{
    // ROS Connector
    private RosConnector rosConnector;
    public GameObject baseLink;
    private BioIK.BioIK bioIK;

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
        var jointsMsg = new FrankaJointsMsg();

        for (var i = 0; i < FrankaConstants.NumJoints; i++)
        {
            jointsMsg.joints[i] = formatBioIKSolution((float)bioIK.Solution[i], i) * Mathf.Deg2Rad;
        }

        rosConnector.GetBridge().Publish(FrankaConstants.topicUnityFrankaJoints, jointsMsg);
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
