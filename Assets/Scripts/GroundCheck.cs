using UnityEngine;

public class GroundCheck : MonoBehaviour
{
// Distance from the object to cast the ray (you can adjust this as needed)
    public float raycastDistance = 1f;

    // LayerMask for the ground, so the raycast only hits objects on the ground layer
    public LayerMask groundLayer;

    [SerializeField]
    Rigidbody rb;

    void Update()
    {
        if (IsGrounded())
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    bool IsGrounded()
    {
        // Cast a ray down from the object's position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            // If the ray hits something, it's the ground
            return true;
        }

        // If no ground was hit, return false
        return false;
    }

    // This method draws a Gizmo in the editor to visualize the raycast
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;  // Color of the Gizmo (green for ground)

        // Draw a ray from the object's position downwards to visualize the raycast
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);

        // Optionally, you can draw a sphere at the hit point if the raycast hits something (for debugging purposes)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            Gizmos.color = Color.red; // Color for the hit point (red)
            Gizmos.DrawSphere(hit.point, 0.1f);  // Draw a sphere at the point of contact
        }
    }
}
