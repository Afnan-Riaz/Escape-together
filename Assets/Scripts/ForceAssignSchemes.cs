using UnityEngine;
using UnityEngine.InputSystem;

public class ForceAssignSchemes : MonoBehaviour
{
    public PlayerInput player1;
    public PlayerInput player2;

    private void Start()
    {
        // Assign keyboard to player 1
        if (player1 != null)
        {
            player1.SwitchCurrentControlScheme("Keyboard", Keyboard.current, Mouse.current);
        }

        // Assign controller to player 2
        if (player2 != null && Gamepad.all.Count > 0)
        {
            player2.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
        }
    }
}
