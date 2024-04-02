using System.Collections;
using UnityEngine;
using RosMessageTypes.CtrlInterfaces;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;


public class FollowTarget : MonoBehaviour
{
    public GameObject frankaLeftFinger;
    public GameObject frankaRightFinger;
    public GameObject endEffectorTargetPrefab;
    public GameObject world;
    private GameObject endEffectorTarget;
    private Vector3 lastTargetPosition;
    private bool isSpawned = false;

    private RosConnector rosConnector;
    // private float publishHz = 60.0f;
    // private float publishFrequency => 1.0f / publishHz;
    // private float timeElapsed;

    private Vector3 lastFramePosition;
    private bool isTargetStill = true;
    

    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();
        Time.fixedDeltaTime = 0.01667f * 2;
    }

    private void FixedUpdate()
    {
        if (!isSpawned && endEffectorTarget == null)
        {
            StartCoroutine(DelaySpawnCenterTarget());
            isSpawned = true;
            return;
        }

        // timeElapsed += Time.deltaTime;
        // if (timeElapsed > publishFrequency)
        // {

        // Check if the target has moved since the last frame
        if (endEffectorTarget != null)
        {
            float distanceMoved = Vector3.Distance(endEffectorTarget.transform.position, lastFramePosition);
            if (distanceMoved <= FrankaConstants.targetMoveThreshold)
            {
                // The target has moved less than the threshold, consider it still
                if (!isTargetStill)
                {
                    // Target just became still in this frame
                    isTargetStill = true;
                }
            }
            else
            {
                // The target is moving
                isTargetStill = false;
            }
            // Update lastFramePosition for the next frame's comparison
            lastFramePosition = endEffectorTarget.transform.position;
        }        
        
        if (endEffectorTarget != null && isTargetStill && !FrankaConstants.similarPosition(endEffectorTarget.transform.position, lastTargetPosition))
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
        // timeElapsed = 0;

        // }
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
            lastFramePosition = endEffectorTargetPosition;

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

}
