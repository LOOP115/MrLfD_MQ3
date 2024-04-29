using UnityEngine;

public class WallCollisionReporter : MonoBehaviour
{
    private WallsManager parentController;

    void Start()
    {
        parentController = GetComponentInParent<WallsManager>();
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
