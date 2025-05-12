using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CubePickup : MonoBehaviour
{
    public float pickupRange = 2f;
    public Transform carryPoint;

    private PlayerInput playerInput;
    private InputAction interactAction;
    private GameObject carriedCube = null;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        interactAction.performed += _ => TryInteract();
    }

    void TryInteract()
    {
        if (carriedCube == null) TryPickUp();
        else                    Drop();
    }

    void TryPickUp()
    {
        foreach (var hit in Physics.OverlapSphere(transform.position, pickupRange))
        {
            if (hit.CompareTag("Cube"))
            {
                PickUp(hit.gameObject);
                return;
            }
        }
    }

    void PickUp(GameObject cube)
    {
        carriedCube = cube;
        var col = cube.GetComponent<Collider>();
        if (col) col.enabled = false;

        var rb = cube.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic      = true;
            rb.useGravity       = false;
            rb.detectCollisions = false;
        }

        cube.transform.SetParent(carryPoint);
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localRotation = Quaternion.identity;
    }

    void Drop()
    {
        carriedCube.transform.SetParent(null);
        var rb  = carriedCube.GetComponent<Rigidbody>();
        var col = carriedCube.GetComponent<Collider>();
        if (col) col.enabled = true;
        if (rb)
        {
            rb.isKinematic           = false;
            rb.useGravity            = false; 
            rb.detectCollisions      = true;
            rb.linearVelocity              = Vector3.zero;
            rb.angularVelocity       = Vector3.zero;
            rb.collisionDetectionMode= CollisionDetectionMode.ContinuousDynamic;
        }

        RaycastHit[] hits = Physics.RaycastAll(
            carryPoint.position,
            Vector3.down,
            10f,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Collide
        );

        if (hits.Length == 0)
        {
            Debug.LogWarning("Drop(): No surface detected beneath carry point.");
            carriedCube = null;
            return;
        }

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("DepositPlatform"))
            {
                var platform = hit.collider.GetComponent<DepositPlatform>();
                if (platform != null && platform.TryRegisterDeposit(carriedCube))
                {
                    carriedCube = null;
                    return;
                }
            }
        }

        foreach (var hit in hits)
        {
            if (!hit.collider.isTrigger)
            {
                float halfH = col.bounds.extents.y;
                const float smallLift = 0.01f;
                carriedCube.transform.position = hit.point + Vector3.up * (halfH + smallLift);
                break;
            }
        }

        carriedCube = null;
    }
}
