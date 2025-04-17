using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private bool isRunning = false;
    private bool jumpPressed = false;
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        // binding movement controls from player's action map
        playerInput.actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Move"].canceled += ctx => moveInput = Vector2.zero;

        // same for running
        playerInput.actions["Run"].performed += ctx => isRunning = true;
        playerInput.actions["Run"].canceled += ctx => isRunning = false;

        // same for jumping
        playerInput.actions["Jump"].started += ctx => jumpPressed = true;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        isGrounded = IsPlayerGrounded();

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Convert moveInput (x = horizontal, y = vertical) into camera-relative direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Remove vertical component so player doesn't tilt
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camRight * moveInput.x + camForward * moveInput.y;

        float speed = isRunning ? runSpeed : walkSpeed;
        controller.Move(moveDirection * speed * Time.deltaTime);

        // Rotate player to face the direction of movement
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Animation
        animator.SetBool("isRunning", moveDirection.magnitude > 0 && isRunning);
        animator.SetBool("isWalking", moveDirection.magnitude > 0 && !isRunning);
        animator.SetBool("isGrounded", isGrounded);

        // Jump
        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        jumpPressed = false;
    }
    bool IsPlayerGrounded()
    {
        // Cast a ray downward from slightly above the player's position (to check if the player is on the ground)
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.1f + 0.01f);
    }

}
