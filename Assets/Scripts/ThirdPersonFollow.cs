using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonFollow : MonoBehaviour
{
    public Transform target; // The player to follow
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public float pitchMin = -30f;
    public float pitchMax = 60f;
    public float sensitivity = 0.3f;

    private float yaw = 0f;
    private float pitch = 10f;
    private PlayerInput playerInput;
    private Vector2 lookInput;
    public float collisionRadius = 0.2f;       // Radius of the sphere cast to detect walls
    public LayerMask collisionLayers;
    private void Awake()
    {
        playerInput = target.GetComponent<PlayerInput>();

        // Read input from Look action
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;
    }


    void LateUpdate()
    {
        if (target == null) return;
        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Create rotation from yaw and pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredCameraPos = target.position + rotation * offset;

        // Raycast from player towards desired camera position
        Vector3 targetCenter = target.position + Vector3.up * 1.5f;
        Vector3 direction = (desiredCameraPos - targetCenter).normalized;
        float desiredDistance = Vector3.Distance(targetCenter, desiredCameraPos);

        // Do a sphere cast to check for obstacles
        if (Physics.SphereCast(targetCenter, collisionRadius, direction, out RaycastHit hit, desiredDistance, collisionLayers))
        {
            // Move the camera closer to the hit point
            desiredCameraPos = targetCenter + direction * (hit.distance - 0.1f); // 0.1f to prevent clipping into the object
        }
        // Set camera position relative to player with rotation applied to offset
        transform.position = desiredCameraPos;

        // Look at a point slightly above the player
        transform.LookAt(targetCenter);
    }
}
