using System.Collections;
using System.Collections.Generic;
using Unity.Netcode; // <<---- IMPORTANT
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovemnt : NetworkBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Animator animator; // Reference to your Animator

    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Ground Check Settings")]
    public Transform groundCheckPoint;
    public Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
    public float castDistance = 0.1f;
    public LayerMask groundLayer;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();

        // Disable control for non-owners
        if (!IsOwner && playerCamera != null)
        {
            playerCamera.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        GroundCheck();
        
        // Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Gravity
        if (isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -0.1f;
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Apply movement
        characterController.Move(moveDirection * Time.deltaTime);

        // Camera rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        // Update animator
        UpdateAnimator(curSpeedX, curSpeedY, isRunning);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.BoxCast(
            groundCheckPoint.position,
            boxSize * 0.5f,
            Vector3.down,
            Quaternion.identity,
            castDistance,
            groundLayer
        );
    }

    private void UpdateAnimator(float moveX, float moveY, bool running)
    {
        float speed = new Vector2(moveX, moveY).magnitude;

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", running && speed > 0.1f);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", moveDirection.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(
            groundCheckPoint.position + Vector3.down * castDistance,
            boxSize
        );
    }
}
