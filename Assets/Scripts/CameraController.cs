using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform FollowTarget;

    public Vector3 Offset = new Vector3(0, 2f, -4f);

    public float PositionSmooth = 10f;
    public float RotationSmooth = 10f;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;

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

        //Camera shake for the disco event
        if (shakeDuration > 0)
        {
            transform.position += Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}