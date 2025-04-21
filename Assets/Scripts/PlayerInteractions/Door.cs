using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;                     // Assign the child door mesh here
    public bool isUnlocked = false;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public Vector3 hingeOffset = Vector3.zero; // Adjust this if needed
    public HingeJoint hingeJoint; // Assigned from child door

    private bool isOpening = false;
    private float rotatedAngle = 0f;
    private Rigidbody doorRb;

    void Start()
    {
        if (door == null)
        {
            Debug.LogError("Door reference not set.");
            return;
        }

        doorRb = door.GetComponent<Rigidbody>();

        if (doorRb == null)
        {
            Debug.LogError("Rigidbody missing on the child door object.");
            return;
        }

        // Lock it at start
        doorRb.isKinematic = true;
        doorRb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Unlock()
    {
        if (isUnlocked) return;
        isUnlocked = true;
        isOpening = true;
    }

    void Update()
    {
        if (isOpening && door != null)
        {
            float deltaAngle = openSpeed * Time.deltaTime;
            Vector3 pivotPoint = transform.position + hingeOffset;

            door.RotateAround(pivotPoint, Vector3.up, deltaAngle);
            rotatedAngle += deltaAngle;

            if (rotatedAngle >= openAngle)
            {
                float overshoot = rotatedAngle - openAngle;
                door.RotateAround(pivotPoint, Vector3.up, -overshoot);
                rotatedAngle = openAngle;
                isOpening = false; 

                // Activate physics, allow only Y-axis rotation
                doorRb.isKinematic = true; //Changed this to true
                doorRb.constraints = RigidbodyConstraints.FreezePositionX |
                                     RigidbodyConstraints.FreezePositionY |
                                     RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }
}
