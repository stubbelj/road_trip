using UnityEngine;

public static class PhysicsUtils
{
    public static float MaxObjectSpeed = 100;

    public static void ClampToMaxObjectSpeed(Rigidbody rb) {
        rb.linearVelocity =
        new Vector3(Mathf.Clamp(rb.linearVelocity.x, -MaxObjectSpeed, MaxObjectSpeed),
                    Mathf.Clamp(rb.linearVelocity.y, -MaxObjectSpeed, MaxObjectSpeed),
                    Mathf.Clamp(rb.linearVelocity.z, -MaxObjectSpeed, MaxObjectSpeed));
    }
}
