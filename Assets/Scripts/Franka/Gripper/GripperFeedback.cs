using UnityEngine;

public class GripperFeedback : MonoBehaviour
{
    public ArticulationBody articulationBody;  // Link this to the articulation body of the finger
    public float gripStrength = 20f;          // Maximum force the gripper should apply
    public float holdPositionMargin = 0.01f;   // Margin within which to hold the position when gripping
    private float initialDriveTarget;          // To store the initial target of the drive for reference
    private bool isGripping = false;           // State to manage gripping

    void Start()
    {
        if (articulationBody == null)
        {
            Debug.LogError("ArticulationBody is not assigned", this);
            return;
        }

        // Store initial xDrive target
        initialDriveTarget = articulationBody.xDrive.target;

        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            AdjustGripStrength(collision);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            AdjustGripStrength(collision);
        }
    }

    private void AdjustGripStrength(Collision collision)
    {
        if (!isGripping)
        {
            // Calculate the required adjustment based on collision intensity
            float adjustment = Mathf.Clamp(gripStrength * Time.fixedDeltaTime, 0, holdPositionMargin);
            
            // Decrease the target position slightly to increase grip without squeezing
            ArticulationDrive drive = articulationBody.xDrive;
            drive.target = Mathf.Max(drive.target - adjustment, initialDriveTarget - holdPositionMargin);
            articulationBody.xDrive = drive;

            // Check if we are close enough to consider it "gripping"
            if (Mathf.Abs(drive.target - (initialDriveTarget - holdPositionMargin)) < 0.01f)
            {
                isGripping = true;  // Lock the position to maintain grip
            }
        }
    }
}
