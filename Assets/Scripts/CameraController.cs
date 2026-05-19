using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform FollowTarget;

    public Vector3 Offset = new Vector3(0, 2f, -4f);

    public float PositionSmooth = 10f;
    public float RotationSmooth = 10f;

    private void LateUpdate()
    {
        if (FollowTarget == null)
            return;

        // Follow behind player (relative to player rotation)
        Vector3 desiredPosition =
            FollowTarget.position +
            FollowTarget.TransformDirection(Offset);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            PositionSmooth * Time.deltaTime
        );

        // Camera rotates with player (NOT enemy)
        Quaternion targetRotation =
            Quaternion.LookRotation(FollowTarget.forward, Vector3.up);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            RotationSmooth * Time.deltaTime
        );
    }
}