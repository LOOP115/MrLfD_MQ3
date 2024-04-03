using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.CtrlInterfaces;


public class RosConnector : MonoBehaviour
{
    private ROSConnection bridge;
    
    public readonly string topicUnityFrankaJoints = "/unity_franka_joints";
    public readonly string topicUnityTargetPose = "/unity_target_pose";
    public readonly string topicUnityCommand = "/unity_command";
    
    public readonly string topicFrankaJoints = "/franka_joints";


    void Start()
    {
        bridge = ROSConnection.GetOrCreateInstance();
        bridge.ShowHud = false;
        bridge.RegisterPublisher<PosTargetMsg>(topicUnityTargetPose);
        bridge.RegisterPublisher<FrankaJointsMsg>(topicUnityFrankaJoints);
        bridge.RegisterPublisher<UnityCommandMsg>(topicUnityCommand);
    }

    public ROSConnection GetBridge()
    {
        return bridge;
    }

}