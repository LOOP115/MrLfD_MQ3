using UnityEngine;
using RosMessageTypes.CtrlInterfaces;


public class JointsPublisherIK : MonoBehaviour
{
    // ROS Connector
    private RosConnector rosConnector;
    public GameObject baseLink;
    private BioIK.BioIK bioIK;

    public float PublishHz = 20.0f;
    private float PublishFrequency => 1.0f / PublishHz;
    private float timeElapsed;

    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();
        bioIK = baseLink.GetComponent<BioIK.BioIK>();

        // Get bioIK joints

    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > PublishFrequency)
        {
            var jointsMsg = new FrankaJointsMsg();

            for (var i = 0; i < FrankaConstants.NumJoints; i++)
            {
                jointsMsg.joints[i] = bioIK.Solution[i];
            }

            // Finally send the message to server_endpoint.py running in ROS
            rosConnector.GetBridge().Publish(rosConnector.topicUnityFrankaJoints, jointsMsg);

            timeElapsed = 0;
        }
    }
    
}
