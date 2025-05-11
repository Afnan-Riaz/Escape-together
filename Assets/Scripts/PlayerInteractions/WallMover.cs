using UnityEngine;

public class WallMover : MonoBehaviour
{
    public GameObject wall;
    public Vector3 finalPosition;
    public float moveSpeed = 2f;      
    private bool shouldMove = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldMove = true;
        }
    }

    private void Update()
    {
        if (shouldMove && wall != null)
        {
            wall.transform.position = Vector3.MoveTowards(wall.transform.position, finalPosition, moveSpeed * Time.deltaTime);
        }
    }
}
