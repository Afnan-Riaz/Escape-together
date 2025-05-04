using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;
    public bool isUnlocked = false;
    public Vector3 closedRotation;
    public Vector3 openedRotation;
    public float speed = 2f;

    private bool isOpening = false;
    private bool isClosing = false;

    void Start()
    {
        if (door == null)
        {
            Debug.LogError("Door reference not set.");
            return;
        }

        // Initialize the door to its closed state
        door.localRotation = Quaternion.Euler(closedRotation);
    }

    public void Unlock()
    {
        if (isUnlocked) return;
        isUnlocked = true;
        isOpening = true;
        isClosing = false;
    }

    public void Close()
    {
        if (!isUnlocked) return;
        isOpening = false;
        isClosing = true;
    }

    void Update()
    {
        if (isOpening)
        {
            door.localRotation = Quaternion.Lerp(door.localRotation, Quaternion.Euler(openedRotation), Time.deltaTime * speed);

            if (Quaternion.Angle(door.localRotation, Quaternion.Euler(openedRotation)) < 0.1f)
            {
                isOpening = false;
            }
        }
        else if (isClosing)
        {
            door.localRotation = Quaternion.Lerp(door.localRotation, Quaternion.Euler(closedRotation), Time.deltaTime * speed);

            if (Quaternion.Angle(door.localRotation, Quaternion.Euler(closedRotation)) < 0.1f)
            {
                isClosing = false;
            }
        }
    }
}