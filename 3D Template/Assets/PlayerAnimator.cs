using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private Animator animator;
    private CharacterController controller;

    private int speedHash = Animator.StringToHash("Speed");
    private int jumpHash = Animator.StringToHash("IsJumping");
    private int crouchHash = Animator.StringToHash("IsCrouching");

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!IsOwner) return; 

        // fnd movement speed
        Vector3 horizontalVel = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float speed = horizontalVel.magnitude;

        // set parameters locally(my player not yours)
        animator.SetFloat(speedHash, speed);

        animator.SetBool(jumpHash, !controller.isGrounded);

        bool isCrouching = Input.GetKey(KeyCode.C); 
        animator.SetBool(crouchHash, isCrouching);
    }
}
