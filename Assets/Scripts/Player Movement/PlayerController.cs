using UnityEngine;
using AustenKinney.AudioSystem;
using AustenKinney.DetectionSystem;
using Lamplight.Input;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Handling")]
    [Tooltip("Base Speed controls the player's movement speed when walking")]
    [SerializeField] private float baseSpeed;
    [Tooltip("Sprint Speed controls the player's movement speed when sprinting")]
    [SerializeField] private float sprintSpeed;
    [Tooltip("Rotation Speed controls the player's turning speed")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("Acceleration Speed controls the rate at which the player gains momentum when moving")]
    [SerializeField] private float accelerationSpeed;
    [Tooltip("Deceleration Speed controls the rate at which the player loses momentum when stopping")]
    [SerializeField] private float decelerationSpeed;
    [Tooltip("Gravity Force controls how powerful the pull of gravity is when falling")]
    [SerializeField] private float gravityForce;

    private Vector2 moveInput;
    private Vector3 currentVelocity;

    [Header("Player Whistle")]
    [Tooltip("The distance at which the player's whistling will be heard by NPCs")]
    [SerializeField] private float whistleRange;

    private bool whistling;

    [Header("Debug Mode")]
    [SerializeField] private bool debugMode;


    //External Components

    private Animator animator;
    private CharacterController characterController;
    private AudioManager audioManager;
    private DetectableObject detectableObject;
    private InventoryManager inventoryManager;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        detectableObject = transform.root.GetComponent<DetectableObject>();
    }

    private void Start()
    {
        audioManager = AudioManager.instance;
        inventoryManager = InventoryManager.instance;
    }

    void Update()
    {
        Move();
        Whistle();
    }

    #region Movement

    private void CalculateGravity()
    {
        if(characterController.isGrounded == false)
        {
            currentVelocity.y -= gravityForce * Time.deltaTime;
        }
    }

    private void Move()
    {
        moveInput = InputProvider.playerActions.GameActions.Move.ReadValue<Vector2>();

        CalculateGravity();

        if (moveInput.magnitude > 0 || currentVelocity.magnitude > 0.1f)
        {
            //calculate move based on input
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * CalculateSpeed();
            //rotate to account for player viewing angle
            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0), rotationSpeed * Time.deltaTime);
            transform.rotation = rotation;
            move = rotation * move;

            Vector3 finalMove = move;

            if (currentVelocity.magnitude < finalMove.magnitude)
            {
                currentVelocity = Vector3.Lerp(currentVelocity, finalMove, accelerationSpeed * Time.deltaTime);
            }
            else
            {
                currentVelocity = Vector3.Lerp(currentVelocity, finalMove, decelerationSpeed * Time.deltaTime);
            }


            if (debugMode == true)
            {
                Debug.Log("Moving Player \n X: " + currentVelocity.x + " Y: " + currentVelocity.y + " Z: " + currentVelocity.z);
                Debug.DrawLine(transform.position + characterController.center, transform.position + characterController.center + (currentVelocity / Time.deltaTime), Color.yellow);
            }

            characterController.Move(currentVelocity * Time.deltaTime);
        }

        if (animator != null)
        {
            animator.SetFloat("ForwardVelocity", (Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) * currentVelocity).z / sprintSpeed);
            animator.SetFloat("RightVelocity", (Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) * currentVelocity).x / sprintSpeed);
            float horizontalSpeed = new Vector2(currentVelocity.x, currentVelocity.z).magnitude;
            animator.SetFloat("CurrentSpeed", horizontalSpeed / sprintSpeed);
        }
    }

    //Calculates the player's movement speed
    private float CalculateSpeed()
    {
        float finalSpeed;

        //weight modifier - The more weight the player carries the slower they will move.
        float weightMod = 1f;
        if (inventoryManager.CarriedWeight > 0)
        {
            float encumberance = (inventoryManager.CarriedWeight / inventoryManager.MaxCarryWeight);
            weightMod = Mathf.Lerp(1f, 0.5f, encumberance);
        }

        if (InputProvider.playerActions.GameActions.Sprint.IsPressed())
        {
            finalSpeed = sprintSpeed * weightMod;
        }
        else
        {
            finalSpeed = baseSpeed * weightMod;
        }

        return finalSpeed;
    }

    #endregion

    #region Actions

    private void Whistle()
    {
        bool whistleInput = InputProvider.playerActions.GameActions.Whistle.WasPressedThisFrame();

        if (whistleInput == true && whistling == false)
        {
            whistling = true;
            SoundData sfx = audioManager.Database.LookUpRandomSound("Whistle");
            audioManager.PlaySound(sfx, transform);
            NoiseMaker.CreateNoise(transform.position, whistleRange, detectableObject);
            whistling = false;
        }
    }

    #endregion
}
