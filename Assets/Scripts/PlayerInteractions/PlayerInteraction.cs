using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public TextMeshProUGUI messageText;
    public bool hasKey = false;

    private Key nearbyKey = null;
    private Door nearbyDoor = null;
    private PlayerInput playerInput;
    private InputAction interactAction;
    public bool hasEscaped;
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
            if (hasKey)
            {
                messageText.text = "Interact to unlock door";
            }
            else
            {
                messageText.text = "The door is locked";
            }
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
        }
        else if (nearbyDoor != null && !nearbyDoor.isUnlocked)
        {
            if (hasKey)
            {
                nearbyDoor.Unlock();
                messageText.text = "";
                nearbyDoor = null;
            }
        }
    }
    void DetectNearbyObjects()
    {
        nearbyKey = null;
        nearbyDoor = null;

        Key[] allKeys = FindObjectsByType<Key>(FindObjectsSortMode.None);
        foreach (var key in allKeys)
        {
            if (Vector3.Distance(transform.position, key.transform.position) < interactRange)
            {
                nearbyKey = key;
                break;
            }
        }

        Door[] allDoors = FindObjectsByType<Door>(FindObjectsSortMode.None);
        foreach (var door in allDoors)
        {
            if (Vector3.Distance(transform.position, door.transform.position) < interactRange)
            {
                nearbyDoor = door;
                break;
            }
        }
    }
}
