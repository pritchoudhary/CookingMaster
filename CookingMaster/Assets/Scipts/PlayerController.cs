using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float _playerSpeed = 2.0f;

    private Vector2 _movementInput = Vector2.zero;
    private bool _isInteracting = false;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        _movementInput = callbackContext.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        _isInteracting = callbackContext.action.triggered;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new(_movementInput.x, 0, _movementInput.y);
        controller.Move(_playerSpeed * Time.deltaTime * move);

        
        
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
