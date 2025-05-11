using UnityEngine;
using System.Collections;
using TMPro;

public class Lever : MonoBehaviour
{
    public Animator animator;
    public bool isDown = false;
    public Transform door;
    public Vector3 upPosition;
    public Vector3 downPosition;
    public float moveSpeed = 2f;
    private bool isOpen = false;
    public Rigidbody ball;
    public string message;
    public float messageDuration = 2f;
    public TextMeshProUGUI messageLabel;
    private Coroutine messageRoutine;


    private bool hasAppliedForce = false;
    public Vector3 forceDirection = new Vector3(1, 0, 0);
    public float forceAmount = 10f;
    void Update()
    {
        Vector3 target = isOpen ? upPosition : downPosition;
        door.position = Vector3.Lerp(door.position, target, Time.deltaTime * moveSpeed);
        if (isOpen && !hasAppliedForce)
        {
            ApplyForceToBall();
        }
    }
    void ShowMessage(string message, float duration = 2f)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(ShowMessageRoutine(message, duration));
    }

    IEnumerator ShowMessageRoutine(string message, float duration)
    {
        if (messageLabel != null)
            messageLabel.text = message;

        yield return new WaitForSeconds(duration);

        if (messageLabel != null)
            messageLabel.text = "";
    }

    public void Open(){
        isOpen = true;
    }
    public void Close(){
        isOpen = false;
    }
    public void Toggle()
    {
        if (message != null)
        {
            ShowMessage(message, messageDuration);
        }

        isDown = !isDown;

        if (isDown)
        {
            animator.SetTrigger("PullDown");
            Close();
        }
        else
        {
            animator.SetTrigger("PullUp");
            Open();
        }
    }
    private void ApplyForceToBall()
    {
        ball.isKinematic = false;
        ball.useGravity = true;

        StartCoroutine(ApplyForceRepeatedly());
    }
    private IEnumerator ApplyForceRepeatedly()
    {
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            ball.AddForce(forceDirection.normalized * forceAmount, ForceMode.Force);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hasAppliedForce = true;
    }
}
