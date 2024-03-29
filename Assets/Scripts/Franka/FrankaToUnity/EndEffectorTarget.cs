using System.Collections;
using UnityEngine;
using RosMessageTypes.CtrlInterfaces;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;


public class EndEffectorTarget : MonoBehaviour
{
    public GameObject frankaLeftFinger;
    public GameObject frankaRightFinger;
    public GameObject endEffectorTargetPrefab;
    public GameObject world;
    private GameObject endEffectorTarget;
    private Vector3 lastTargetPosition;

    public string topicName = "/unity_target_pose";

    private RosConnector rosConnector;
    // public float publishHz = 20.0f;
    // private float publishFrequency => 1.0f / publishHz;
    // private float timeElapsed;
    

    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();
        StartCoroutine(DelaySpawnEndEffectorTarget());
    }

    private void Update()
    {
        if (endEffectorTarget != null && endEffectorTarget.transform.position != lastTargetPosition)
        {
            var targetPosition = endEffectorTarget.transform.localPosition.To<FLU>();
            var targetRotation = endEffectorTarget.transform.localRotation.To<FLU>();
            
            var targetPoseMsg = new PosTargetMsg
            {
                pos_x = targetPosition.x,
                pos_y = targetPosition.y,
                pos_z = targetPosition.z,
                rot_x = targetRotation.x,
                rot_y = targetRotation.y,
                rot_z = targetRotation.z,
                rot_w = targetRotation.w
            };

            rosConnector.GetBridge().Publish(topicName, targetPoseMsg);
            lastTargetPosition = endEffectorTarget.transform.position;
        }

        // Teleport target
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (endEffectorTarget != null)
            {
                Vector3 handPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
                handPosition.y += 0.05f;
                endEffectorTarget.transform.position = handPosition;
            }
        }

    }


    IEnumerator DelaySpawnEndEffectorTarget()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnEndEffectorTarget();
    }

    private void SpawnEndEffectorTarget()
    {
        if (frankaLeftFinger != null && frankaRightFinger != null)
        {
            Vector3 leftFingerPosition = frankaLeftFinger.transform.position;
            Vector3 rightFingerPosition = frankaRightFinger.transform.position;
            Vector3 endEffectorTargetPosition = (leftFingerPosition + rightFingerPosition) / 2;
            endEffectorTargetPosition.y -= 0.045f;
            endEffectorTarget = Instantiate(endEffectorTargetPrefab, endEffectorTargetPosition, Quaternion.Euler(-180, 0, 0));
            lastTargetPosition = endEffectorTargetPosition;

            if (world != null)
            {
                endEffectorTarget.transform.SetParent(world.transform, true);
            }
        }
        else
        {
            Debug.LogError("Left and Right Finger GameObjects are not set.");
        }
    }

}
