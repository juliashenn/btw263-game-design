using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    public float speed = 6.0f;
    public float turnSpeed = 10.0f;
    private Vector3 moveDirection = Vector3.zero;
    public float gravity = 9.0f;
    public float jumpHeight = 70.0f;
    private float ySpeed = 0f;

    public Transform cameraTransform;  // Reference to the main camera

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Animation control
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("s"))
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else if (Input.GetKey("space"))
        {
            anim.SetInteger("AnimationPar", 3);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
        }

        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get direction relative to the camera
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 desiredMoveDirection = camForward * vertical + camRight * horizontal;

        if (controller.isGrounded)
        {
            moveDirection = desiredMoveDirection * speed;

            if (desiredMoveDirection != Vector3.zero)
            {
                // Smoothly rotate player to face move direction
                Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }

            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpHeight;
            }
        }
        else
        {
            ySpeed -= gravity * Time.deltaTime;
        }

        // Apply vertical speed
        moveDirection.y = ySpeed;

        // Move the character
        controller.Move(moveDirection * Time.deltaTime);
    }
}
