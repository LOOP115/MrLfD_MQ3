using System.Collections;
using UnityEngine;
using RosMessageTypes.CtrlInterfaces;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;


public class ReachTarget : MonoBehaviour
{
    public GameObject frankaLeftFinger;
    public GameObject frankaRightFinger;
    public GameObject endEffectorTargetPrefab;
    public GameObject world;
    private GameObject endEffectorTarget;
    private Vector3 lastTargetPosition;
    private bool isSpawned = false;

    private RosConnector rosConnector;
    // public float publishHz = 20.0f;
    // private float publishFrequency => 1.0f / publishHz;
    // private float timeElapsed;
    
    private GripperController gripperController;
    private bool isGripperClosed = false;
    private GameObject rightHandAnchor;
    

    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();
        gripperController = FindObjectOfType<GripperController>();
        rightHandAnchor = GameObject.Find("RightHandAnchor");
    }

    private void Update()
    {
        if (!isSpawned && endEffectorTarget == null)
        {
            StartCoroutine(DelaySpawnCenterTarget());
            isSpawned = true;
            return;
        }
        publishTarget();
        changeTarget();
        toggleGripper();
    }


    IEnumerator DelaySpawnCenterTarget()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnCenterTarget();
    }

    // Spawn the target in the center of the two fingers of Franka
    private void SpawnCenterTarget()
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

    public void RemoveTarget()
    {
        if (endEffectorTarget != null)
        {
            Destroy(endEffectorTarget);
            isSpawned = false;
        }

    }

    private void publishTarget()
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

            rosConnector.GetBridge().Publish(rosConnector.topicUnityTargetPose, targetPoseMsg);
            lastTargetPosition = endEffectorTarget.transform.position;
        }
    }
    
    private void changeTarget()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (endEffectorTarget != null)
            {
                Vector3 handPosition = rightHandAnchor.transform.position;
                handPosition.y += 0.05f;
                endEffectorTarget.transform.position = handPosition;
            }
        }
    }

    private void toggleGripper()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (isGripperClosed)
            {
                gripperController.Open();
                isGripperClosed = false;
            }
            else
            {
                gripperController.Close();
                isGripperClosed = true;
            }
        }
    }

}
