using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    float rotationFactorPerFrame = 1.0f;

    [SerializeField] AudioSource walkSound;

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        walkSound.Play();
    }

    void handleAnimation()
    {
        bool isRunning = animator.GetBool("isRunning");

        if (isMovementPressed && !isRunning)
        {
            animator.SetBool("isRunning", true);
        }

        else if (!isMovementPressed && isRunning)
        {
            animator.SetBool("isRunning", false);
        }
    }
    
    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.y;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

        }
        
    }

    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = -0.05f;
            currentMovement.y += groundedGravity * Time.deltaTime;
        }
        else
        {
            float gravity = -2f;
            currentMovement.y += gravity * Time.deltaTime;
        }
    }

    void onControllerColliderHit (ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy Body"))
        {
            Debug.Log("Death");
        }        
    }

    void Update()
    {
        handleRotation();
        handleAnimation();
        // handleGravity();
        characterController.Move(currentMovement * Time.deltaTime);

    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

    // private void onCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Enemy Body"))
    //     {
    //         die();
    //     }
    // }

    // void die()
    // {
    //     GetComponent<SkinnedMeshRenderer>().enabled = false;
    //     OnDisable();
    // }
}
