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
    [SerializeField]
    private Transform _rightHandPosition;
    [SerializeField]
    private Transform _leftHandPosition;
    [SerializeField]
    private Vector3 _offset;

    private Vector2 _movementInput = Vector2.zero;
    private VegetableInstance[] carriedVegetables = new VegetableInstance[2];
    private bool _canMove = true;

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
        if (callbackContext.performed)
        {
            // Check for a chopping board in proximity if carrying vegetables
            if (IsCarryingVegetable())
            {
                TryPlaceVegetableOnChoppingBoard();
            }
            else // Otherwise, try to pick up a vegetable
            {
                TryPickUpVegetable();
            }
        }
    }

    void Update()
    {
        if(_canMove)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector3 move = new(_movementInput.x, 0, _movementInput.y);
            controller.Move(_playerSpeed * Time.deltaTime * move);
            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }
        
        //controller.Move(playerVelocity * Time.deltaTime);
    }

    private bool IsCarryingVegetable()
    {
        //The player is carrying max vegetables if niether hand is empty
        return carriedVegetables[0] != null && carriedVegetables[1] != null;
    }

    private void TryPlaceVegetableOnChoppingBoard()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // Interaction radius
        foreach (var hitCollider in hitColliders)
        {
            ChoppingBoard choppingBoard = hitCollider.GetComponent<ChoppingBoard>();
            Debug.Log("Chopping board found");
            if (choppingBoard != null && choppingBoard.IsAvailable())
            {
                // Place the first picked vegetable on the chopping board
                if (carriedVegetables[0] != null)
                {
                    if (choppingBoard.ChopVegetable(carriedVegetables[0]))
                    {
                        //Subscribe to chopping board events
                        choppingBoard.OnChoppingStarted += StopPlayerMovement;
                        choppingBoard.OnChoppingCompleted += AllowPlayerMovement;
                        RemoveVegetableFromHand(0);
                    }
                    return; // Exit after attempting to place one vegetable
                }
            }
        }
    }

    private void StopPlayerMovement()
    {
        _canMove = false;
    }

    private void AllowPlayerMovement()
    {
        _canMove = true;

        //Unsubscribe from chopping board events to avoid multiple calls
        ChoppingBoard choppingBoard = carriedVegetables[0].GetComponentInParent<ChoppingBoard>();
        if(choppingBoard != null)
        {
            choppingBoard.OnChoppingStarted -= StopPlayerMovement;
            choppingBoard.OnChoppingCompleted -= AllowPlayerMovement;
        }
    }

    private void TryPickUpVegetable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // Interaction radius
        Debug.Log("Checking for vegetables. Found: " + hitColliders.Length);
        foreach (var hitCollider in hitColliders)
        {
            var vegetableInstance = hitCollider.GetComponent<VegetableInstance>();
            Debug.Log("Found Object: " + hitCollider.name);
            if (vegetableInstance != null && vegetableInstance._currentState == VegetableState.Idle && CanPickUpVegetable())
            {
                PickUpVegetable(vegetableInstance);
                break; // Only pick up one vegetable at a time
            }
        }
    }

    private bool CanPickUpVegetable()
    {
        // Check if there is space to pick up another vegetable
        return carriedVegetables[0] == null || carriedVegetables[1] == null;
    }

    private void PickUpVegetable(VegetableInstance vegetable)
    {
        //Find the first available hand
        for (int i = 0; i < carriedVegetables.Length; i++)
        {
            if (carriedVegetables[i] == null)
            {
                carriedVegetables[i] = vegetable;
                vegetable.PickUp();
                AttachVegetableToHand(vegetable, i);
                return; //Exit the loop and method after successfully picking up a vegetable
            }
        }
    }

    private void AttachVegetableToHand(VegetableInstance vegetable, int handIndex)
    {
        Vector3 originalScale = vegetable.transform.localScale;
        Debug.Log("Local Scale: " + originalScale);

        Transform handTransform = handIndex == 0 ? _rightHandPosition : _leftHandPosition;
        vegetable.transform.SetParent(handTransform, false);
        vegetable.transform.localPosition = _offset;
        vegetable.transform.localScale = originalScale;
    }

    private void RemoveVegetableFromHand(int index)
    {
        carriedVegetables[index].transform.SetParent(null); // Detach the vegetable from the player
        carriedVegetables[index] = null;

        // Shift the second vegetable to the first slot if the first one was placed
        if (index == 0 && carriedVegetables[1] != null)
        {
            carriedVegetables[0] = carriedVegetables[1];
            AttachVegetableToHand(carriedVegetables[0], 0);
            carriedVegetables[1] = null;
        }
    }
}
