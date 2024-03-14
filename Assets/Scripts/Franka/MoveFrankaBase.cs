using UnityEngine;

public class MoveFrankaBase : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 90.0f; // Degrees per second

    private ArticulationBody articulationBody;

    void Start()
    {
        // Assuming the ArticulationBody component is on the same GameObject as this script
        articulationBody = transform.Find("world/panda_link0").GetComponent<ArticulationBody>();
    }

    void Update()
    {
        // Get the left thumbstick's current state
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Calculate the new position based on the input
        Vector3 newPos = articulationBody.transform.position + new Vector3(input.x, 0, input.y) * moveSpeed * Time.deltaTime;

        // Keep the current rotation unchanged
        Quaternion newRot = articulationBody.transform.rotation;

        // Apply teleportation to the new position with the current rotation
        articulationBody.TeleportRoot(newPos, newRot);
    }
}
