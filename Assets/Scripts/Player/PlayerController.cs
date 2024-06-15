using System;
using UnityEngine;
using DLS.MessageSystem;
using DLS.MessageSystem.Messaging;
using Input;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [field: SerializeField, Header("Player Attributes"), Tooltip("This is the speed of the player")] public float Speed { get; set; } = 3f;
    [field: SerializeField] public float JumpForce { get; set; } = 250f;
    
    protected PlayerInputActions playerInputActions;
    protected Vector2 movementInput;
    protected Rigidbody2D rb;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
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
    }
    
    private void MoveOnperformed(InputAction.CallbackContext input)
    {
        movementInput.x = input.ReadValue<Vector2>().x;
    }
    
    private void JumpOnperformed(InputAction.CallbackContext input)
    {
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }
    
    private void Update()
    {
        transform.position += (Vector3)movementInput * (Speed * Time.deltaTime);
    }


}
