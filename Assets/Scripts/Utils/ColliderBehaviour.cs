using UnityEngine;

public class CollisionIgnorer : MonoBehaviour
{
    public string ignoredTag = "Franka";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ignoredTag))
        {
            // Retrieve all colliders on the current GameObject
            Collider[] myColliders = GetComponentsInChildren<Collider>();
            // Retrieve all colliders on the colliding GameObject
            Collider[] otherColliders = collision.gameObject.GetComponentsInChildren<Collider>();

            // Iterate over all combinations of my colliders and other colliders
            foreach (Collider myCollider in myColliders)
            {
                foreach (Collider otherCollider in otherColliders)
                {
                    Physics.IgnoreCollision(otherCollider, myCollider, true);
                }
            }
        }
        else
        {
            Debug.Log($"Collided with {collision.gameObject.name}");
        }
    }
}
