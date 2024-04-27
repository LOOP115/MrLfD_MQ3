using UnityEngine;

public class WallCollisionReporter : MonoBehaviour
{
    private AirWallsManager parentController;

    void Start()
    {
        parentController = GetComponentInParent<AirWallsManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        parentController.ChildCollisionEnter();
    }

    void OnCollisionExit(Collision collision)
    {
        parentController.ChildCollisionExit();
    }
}
