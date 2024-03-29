using UnityEngine;

public class MoveBase : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float rotateSpeed = 45.0f; // Degrees per second

    private ArticulationBody articulationBody;

    void Start()
    {
        // Assuming the ArticulationBody component is on the same GameObject as this script
        articulationBody = transform.Find("world/panda_link0").GetComponent<ArticulationBody>();
    }

    void Update()
    {
        // Get the right thumbstick's current state
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Calculate the new position based on the input
        Vector3 newPos = articulationBody.transform.position + new Vector3(input.x, 0, input.y) * moveSpeed * Time.deltaTime;

        // Get the primary thumbstick's current state for rotation
        Vector2 rotationInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        
        bool moveUp = OVRInput.Get(OVRInput.RawButton.Y);
        if (moveUp)
        {
            newPos += Vector3.up * moveSpeed * Time.deltaTime;
        }

        bool moveDown = OVRInput.Get(OVRInput.RawButton.X);
        if (moveDown)
        {
            newPos -= Vector3.up * moveSpeed * Time.deltaTime;
        }

        // Calculate the new rotation around the Y-axis based on the thumbstick's X-axis
        float newYRotation = rotationInput.x * rotateSpeed * Time.deltaTime;
        Quaternion newRot = articulationBody.transform.rotation * Quaternion.Euler(0, -newYRotation, 0);

        // Apply teleportation to the new position with the current rotation
        articulationBody.TeleportRoot(newPos, newRot);
        gameObject.transform.position = newPos;
        gameObject.transform.rotation = newRot;
    }
}
