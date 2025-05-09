using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    public DoorController doorController;
    public GameObject buttonObject;
    public float buttonSpeed = 5f;
    public Material pressedMaterial;
    public Material idleMaterial;

    private Vector3 originalScale;
    private Vector3 pressedScale;
    private Renderer rend;
    private bool isPressed = false;

    void Start()
    {
        originalScale = buttonObject.transform.localScale;
        pressedScale = new Vector3(originalScale.x, 0.01f, originalScale.z);
        rend = buttonObject.GetComponent<Renderer>();
        rend.material = idleMaterial;
    }

    void Update()
    {
        buttonObject.transform.localScale = Vector3.Lerp(
            buttonObject.transform.localScale,
            isPressed ? pressedScale : originalScale,
            Time.deltaTime * buttonSpeed
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            isPressed = true;
            rend.material = pressedMaterial;
            doorController.ButtonPressed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPressed)
        {
            isPressed = false;
            rend.material = idleMaterial;
            doorController.ButtonReleased();
        }
    }
}
