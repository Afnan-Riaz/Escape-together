using UnityEngine;
using System.Collections;

public class DualLever : MonoBehaviour
{
    public System.Action onToggle;

    public Animator animator;
    public bool isDown = false;

    public void Toggle()
    {
        isDown = !isDown;

        if (isDown)
        {
            animator.SetTrigger("PullDown");
        }
        else
        {
            animator.SetTrigger("PullUp");
        }
        Debug.Log("Invoking");
        onToggle?.Invoke();

    }

}
