using UnityEngine;

public class MoveFranka : MonoBehaviour
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
        Vector3 newPos = articulationBody.transform.position;
        Quaternion newRot = articulationBody.transform.rotation;

        // Movement
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPos += Vector3.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPos += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPos += Vector3.up * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPos += Vector3.down * moveSpeed * Time.deltaTime;
        }

        // Rotation
        if (Input.GetKey(KeyCode.A))
        {
            newRot *= Quaternion.Euler(0, rotateSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            newRot *= Quaternion.Euler(0, -rotateSpeed * Time.deltaTime, 0);
        }

        // Apply teleportation
        articulationBody.TeleportRoot(newPos, newRot);
    }
}
