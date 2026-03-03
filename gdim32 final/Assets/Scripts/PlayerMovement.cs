using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;

    [SerializeField, Min(0f)] private float walkSpeed = 5f;
    [SerializeField, Min(0f)] private float sprintSpeed = 8.5f;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [SerializeField, Min(0f)] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [SerializeField, Min(0f)] private float mouseSensitivity = 2f;
    [SerializeField, Range(1f, 89f)] private float maxLookAngle = 85f;
    [SerializeField] private bool lockCursor = true;

    private CharacterController controller;
    [SerializeField] private DialogueManager dialogueManager;
    private float cameraPitch;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            var cam = GetComponentInChildren<Camera>();
            if (cam != null) playerCamera = cam.transform;
        }
    }

    private void OnEnable()
    {
        ApplyCursorState();
    }

    private void Update()
    {
        Look();
        Move();
    }

private void ApplyCursorState()
    {
        if (!lockCursor) return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Look()
    {
        if (playerCamera == null) return;

        if (!dialogueManager.inConversation)
        {
            float mx = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            float my = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mx);

            cameraPitch -= my;
            cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);
            playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    private void Move()
    {
        bool grounded = controller.isGrounded;

        if (grounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        float forward =
            (Input.GetKey(KeyCode.W) ? 1f : 0f) +
            (Input.GetKey(KeyCode.UpArrow) ? 1f : 0f) -
            (Input.GetKey(KeyCode.S) ? 1f : 0f) -
            (Input.GetKey(KeyCode.DownArrow) ? 1f : 0f);

        float right =
            (Input.GetKey(KeyCode.D) ? 1f : 0f) +
            (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f) -
            (Input.GetKey(KeyCode.A) ? 1f : 0f) -
            (Input.GetKey(KeyCode.LeftArrow) ? 1f : 0f);

        Vector2 input = new Vector2(right, forward);
        if (input.sqrMagnitude > 1f) input.Normalize();

        bool sprinting = Input.GetKey(sprintKey);
        float speed = sprinting ? sprintSpeed : walkSpeed;

        Vector3 moveWorld = (transform.right * input.x + transform.forward * input.y) * speed;

        if (grounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = new Vector3(moveWorld.x, verticalVelocity, moveWorld.z);
        controller.Move(velocity * Time.deltaTime);
    }
}