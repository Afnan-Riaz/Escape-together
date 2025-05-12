using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public TextMeshProUGUI messageText;
    public bool hasKey = false;
    public bool hasEscaped;

    private Key nearbyKey = null;
    private Door nearbyDoor = null;
    private Lever nearbyLever = null;
    private DualLever nearbyDualLever = null;
    private ColorTile nearbyTile = null;

    private PlayerInput playerInput;
    private InputAction interactAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        interactAction.Enable();
        interactAction.performed += OnInteracting;
    }

    void Update()
    {
        DetectNearbyObjects();

        if (nearbyKey != null && !hasKey)
        {
            messageText.text = "Interact to pick up key";
        }
        else if (nearbyDoor != null && !nearbyDoor.isUnlocked)
        {
            DepositPlatform platform = FindObjectOfType<DepositPlatform>();
            if (!hasKey)
            {
                messageText.text = "The door is locked";
            }
            else if (platform == null)
            {
                messageText.text = "Interact to unlock door";
            }
            else
            {
                if (platform.Completed)
                    messageText.text = "Interact to unlock door";
                else
                    messageText.text = $"Collect all cubes to unlock ({platform.DepositedCount}/{platform.totalCubes})";
            }
        }
        else if (nearbyLever != null)
        {
            messageText.text = "Interact to pull lever";
        }
        else if (nearbyDualLever != null)
        {
            messageText.text = "Interact to pull lever";
        }
        else if (nearbyTile != null)
        {
            messageText.text = "Interact to change color";
        }
        else
        {
            messageText.text = "";
        }
    }

    public void OnInteracting(InputAction.CallbackContext context)
    {
        if (nearbyKey != null && !hasKey)
        {
            hasKey = true;
            Destroy(nearbyKey.gameObject);
            messageText.text = "";
            nearbyKey = null;
            return;
        }

        if (nearbyDoor != null && !nearbyDoor.isUnlocked && hasKey)
        {
            DepositPlatform platform = FindObjectOfType<DepositPlatform>();
            bool canUnlock = (platform == null) || platform.Completed;
            if (canUnlock)
            {
                nearbyDoor.Unlock();
                messageText.text = "";
                nearbyDoor = null;
            }
            return;
        }

        if (nearbyLever != null)
        {
            nearbyLever.Toggle();
            return;
        }

        if (nearbyDualLever != null)
        {
            nearbyDualLever.Toggle();
            return;
        }

        if (nearbyTile != null)
        {
            nearbyTile.CycleColor();
        }
    }

    void DetectNearbyObjects()
    {
        nearbyKey = null;
        nearbyDoor = null;
        nearbyLever = null;
        nearbyDualLever = null;
        nearbyTile = null;

        float closestTileDist = interactRange;

        foreach (var key in FindObjectsByType<Key>(FindObjectsSortMode.None))
            if (Vector3.Distance(transform.position, key.transform.position) < interactRange)
                nearbyKey = key;

        foreach (var door in FindObjectsByType<Door>(FindObjectsSortMode.None))
            if (Vector3.Distance(transform.position, door.transform.position) < interactRange)
                nearbyDoor = door;

        foreach (var lever in FindObjectsByType<Lever>(FindObjectsSortMode.None))
            if (Vector3.Distance(transform.position, lever.transform.position) < interactRange)
                nearbyLever = lever;

        foreach (var dualLever in FindObjectsByType<DualLever>(FindObjectsSortMode.None))
            if (Vector3.Distance(transform.position, dualLever.transform.position) < interactRange)
                nearbyDualLever = dualLever;

        foreach (var tile in FindObjectsByType<ColorTile>(FindObjectsSortMode.None))
        {
            if (!tile.isInputTile) continue;

            float dist = Vector3.Distance(transform.position, tile.transform.position);
            if (dist < closestTileDist)
            {
                closestTileDist = dist;
                nearbyTile = tile;
            }
        }
    }
}
