using System.Collections;
using UnityEngine;
using RosMessageTypes.CtrlInterfaces;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;


public class TrackTarget : MonoBehaviour
{
    public GameObject frankaLeftFinger;
    public GameObject frankaRightFinger;
    public GameObject endEffectorTargetPrefab;
    public GameObject world;
    private GameObject endEffectorTarget;


    void Start()
    {
        StartCoroutine(DelaySpawnCenterTarget());
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
            
            endEffectorTarget = Instantiate(endEffectorTargetPrefab, endEffectorTargetPosition, Quaternion.Euler(0, 0, -180));
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
        }
    }

}
