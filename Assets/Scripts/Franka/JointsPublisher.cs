using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.UrdfImporter;
using RosMessageTypes.CtrlInterfaces;


public class JointsPublisher : MonoBehaviour
{
    // Variables required for ROS communication
    [SerializeField]
    string m_TopicName = "/unity_franka_joints";

    [SerializeField]
    GameObject m_Franka;

    // Robot Joints
    UrdfJointRevolute[] m_JointArticulationBodies;

    // ROS Connector
    ROSConnection m_Ros;

    
    public float PublishHz = 20.0f;
    private float PublishFrequency => 1.0f / PublishHz;

    private float timeElapsed;

    void Start()
    {
        // Get ROS connection static instance
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<FrankaJointsMsg>(m_TopicName);

        m_JointArticulationBodies = new UrdfJointRevolute[FrankaConstants.NumJoints];

        var linkName = string.Empty;
        for (var i = 0; i < FrankaConstants.NumJoints; i++)
        {
            linkName += FrankaConstants.LinkNames[i];
            m_JointArticulationBodies[i] = m_Franka.transform.Find(linkName).GetComponent<UrdfJointRevolute>();
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
                jointsMsg.joints[i] = m_JointArticulationBodies[i].GetPosition();
            }

            // Finally send the message to server_endpoint.py running in ROS
            m_Ros.Publish(m_TopicName, jointsMsg);

            timeElapsed = 0;
        }
    }
    
}
