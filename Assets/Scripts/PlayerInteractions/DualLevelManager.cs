using UnityEngine;

public class DualLeverManager : MonoBehaviour
{
    public DualLever leverA;
    public DualLever leverB;

    private float leverATime = -2f;
    private float leverBTime = -2f;

    public float activationWindow = 1f;

    private bool gateOpened = false;

    [Header("Gate 1 Settings")]
    public Transform gate1;
    public Vector3 closedPosition1;
    public Vector3 openPosition1;

    [Header("Gate 2 Settings")]
    public Transform gate2;
    public Vector3 closedPosition2;
    public Vector3 openPosition2;

    public float gateMoveSpeed = 2f;

    void Start()
    {
        leverA.onToggle += OnLeverAToggled;
        leverB.onToggle += OnLeverBToggled;
    }

    void Update()
    {
        if (gate1 != null)
        {
            Vector3 target1 = gateOpened ? openPosition1 : closedPosition1;
            gate1.position = Vector3.Lerp(gate1.position, target1, Time.deltaTime * gateMoveSpeed);
        }

        if (gate2 != null)
        {
            Vector3 target2 = gateOpened ? openPosition2 : closedPosition2;
            gate2.position = Vector3.Lerp(gate2.position, target2, Time.deltaTime * gateMoveSpeed);
        }
    }

    private void OnLeverAToggled()
    {
        leverATime = Time.time;
        CheckDualActivation();
    }

    private void OnLeverBToggled()
    {
        leverBTime = Time.time;
        CheckDualActivation();
    }

    private void CheckDualActivation()
    {
        Debug.Log($"Lever A: {leverATime} | Lever B: {leverBTime}");

        if (gateOpened) return;

        if (Mathf.Abs(leverATime - leverBTime) <= activationWindow)
        {
            Debug.Log("Both levers activated within time window. Opening gate.");
            gateOpened = true;
        }
    }
}
