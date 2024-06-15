using System;
using UnityEngine;
using DLS.MessageSystem;
using DLS.MessageSystem.Messaging;
using Input;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [field: SerializeField, Header("Player Attributes"), Tooltip("This is the speed of the player")] public float Speed { get; set; } = 3f;
    [field: SerializeField] public float JumpForce { get; set; } = 250f;
    
    [field: SerializeField] public int MaxJumps { get; set; } = 2;
    [field: SerializeField] public float GroundCheckRadius { get; set; } = 0.6f;
    [field: SerializeField] public LayerMask GroundLayer { get; set; }
    [field: SerializeField] public bool isGrounded { get; set; }
    
    protected PlayerInputActions playerInputActions;
    protected Vector2 movementInput;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    
    protected Vector2 lastPosition;
    protected int jumpCount;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += MoveOnperformed;
        playerInputActions.Player.Move.canceled += MoveOnperformed;
        playerInputActions.Player.Jump.performed += JumpOnperformed;
    }
    
    private void OnDisable()
    {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Move.performed -= MoveOnperformed;
        playerInputActions.Player.Move.canceled -= MoveOnperformed;
        playerInputActions.Player.Jump.performed -= JumpOnperformed;
    }
    
    private void MoveOnperformed(InputAction.CallbackContext input)
    {
        movementInput.x = input.ReadValue<Vector2>().x;
        if (movementInput.x != 0)
        {
            lastPosition.x = movementInput.x;
        }
        var isMoving = movementInput.x != 0;
        anim.SetBool("isMoving", isMoving);
        var isFacingLeft = lastPosition.x < 0;
        sr.flipX = isFacingLeft;
    }
    
    private void JumpOnperformed(InputAction.CallbackContext input)
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckRadius, GroundLayer);
        isGrounded = hit;
        if(isGrounded)
        {
            jumpCount = 0;
        }
        if (isGrounded || jumpCount < MaxJumps)
        {
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
            jumpCount++;
        }
    }
    
    private void Update()
    {
        transform.position += (Vector3)movementInput * (Speed * Time.deltaTime);
    }


}
