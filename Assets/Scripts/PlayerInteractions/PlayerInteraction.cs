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

    public bool hasEscaped;

    void Update()
    {
        DetectNearbyObjects();

        if (nearbyKey != null && !hasKey)
        {
            messageText.text = "Press E to pick up key";

            if (Input.GetKeyDown(KeyCode.E))
            {
                hasKey = true;
                Destroy(nearbyKey.gameObject);
                messageText.text = "";
                nearbyKey = null;
            }
        }
        else if (nearbyDoor != null && !nearbyDoor.isUnlocked)
        {
            if (hasKey)
            {
                messageText.text = "Press E to unlock door";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    nearbyDoor.Unlock();
                    messageText.text = "";
                    nearbyDoor = null;
                }
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

    void DetectNearbyObjects()
    {
        nearbyKey = null;
        nearbyDoor = null;

        Key[] allKeys = GameObject.FindObjectsOfType<Key>();
        foreach (var key in allKeys)
        {
            if (Vector3.Distance(transform.position, key.transform.position) < interactRange)
            {
                nearbyKey = key;
                break;
            }
        }

        Door[] allDoors = GameObject.FindObjectsOfType<Door>();
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
