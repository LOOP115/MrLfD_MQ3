using UnityEngine;

// [RequireComponent(typeof(SphereCollider))]
public class CollisionIgnorer : MonoBehaviour
{
    public string ignoredTag;
    
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the specific tag
        if (collision.gameObject.CompareTag(ignoredTag))
        {
            // Optionally, you can perform additional logic here if needed,
            // like logging or handling special cases when encountering the ignored object.
            
            // Physically ignore the collision
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else
        {
            // Handle other collisions
            Debug.Log($"Collided with {collision.gameObject.name}");
        }
    }
}
