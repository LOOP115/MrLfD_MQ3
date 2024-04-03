using UnityEngine;
using Unity.Robotics.UrdfImporter;
using RosMessageTypes.CtrlInterfaces;


public class JointsPublisher : MonoBehaviour
{
    // Robot Joints
    private UrdfJointRevolute[] jointArticulationBodies;

    // ROS Connector
    private RosConnector rosConnector;

    public float PublishHz = 20.0f;
    private float PublishFrequency => 1.0f / PublishHz;

    private float timeElapsed;

    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();
        jointArticulationBodies = new UrdfJointRevolute[FrankaConstants.NumJoints];

        var linkName = string.Empty;
        for (var i = 0; i < FrankaConstants.NumJoints; i++)
        {
            linkName += FrankaConstants.LinkNames[i];
            jointArticulationBodies[i] = transform.Find(linkName).GetComponent<UrdfJointRevolute>();
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > PublishFrequency)
        {
            var jointsMsg = new FrankaJointsMsg();
            for (var i = 0; i < FrankaConstants.NumJoints; i++)
            {
                jointsMsg.joints[i] = jointArticulationBodies[i].GetPosition();
            }

            // Finally send the message to server_endpoint.py running in ROS
            rosConnector.GetBridge().Publish(rosConnector.topicUnityFrankaJoints, jointsMsg);

            timeElapsed = 0;
        }
    }
    
}
