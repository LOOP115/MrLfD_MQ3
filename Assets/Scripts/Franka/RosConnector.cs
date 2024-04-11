using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.CtrlInterfaces;


public class RosConnector : MonoBehaviour
{
    private ROSConnection bridge;
    
    void Start()
    {
        bridge = ROSConnection.GetOrCreateInstance();
        bridge.ShowHud = false;
        bridge.RegisterPublisher<PosTargetMsg>(FrankaConstants.topicUnityTargetPose);
        bridge.RegisterPublisher<UnityCommandMsg>(FrankaConstants.topicUnityCommand);
        bridge.RegisterPublisher<FrankaJointsMsg>(FrankaConstants.topicUnityFrankaJoints);
    }

    public ROSConnection GetBridge()
    {
        return bridge;
    }

}