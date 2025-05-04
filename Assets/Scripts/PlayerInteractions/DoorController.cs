using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public Vector3 leftDoorOpenOffset;
    public Vector3 rightDoorOpenOffset;
    public float doorSpeed = 2f;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private int activeButtons = 0;

    private void Start()
    {
        leftClosedPos = leftDoor.position;
        rightClosedPos = rightDoor.position;
    }

    private void Update()
    {
        bool shouldOpen = activeButtons > 0;

        leftDoor.position = Vector3.MoveTowards(
            leftDoor.position,
            shouldOpen ? leftClosedPos + leftDoorOpenOffset : leftClosedPos,
            Time.deltaTime * doorSpeed
        );

        rightDoor.position = Vector3.MoveTowards(
            rightDoor.position,
            shouldOpen ? rightClosedPos + rightDoorOpenOffset : rightClosedPos,
            Time.deltaTime * doorSpeed
        );
    }

    public void ButtonPressed()
    {
        activeButtons++;
    }

    public void ButtonReleased()
    {
        activeButtons = Mathf.Max(0, activeButtons - 1);
    }
}
