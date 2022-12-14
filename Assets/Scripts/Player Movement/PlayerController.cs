using UnityEngine;
using AustenKinney.AudioSystem;
using AustenKinney.AI.DetectionSystem;
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

    private Vector2 moveInput;
    private Vector3 currentVelocity;

    [Header("Player Whistle")]
    [Tooltip("The distance at which the player's whistling will be heard by NPCs")]
    [SerializeField] private float whistleRange;

    private bool whistling;

    [Header("Debug Mode")]
    [SerializeField] private bool debugMode;


    private Animator animator;
    private CharacterController characterController;
    private AudioManager audioManager;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    void Update()
    {
        Move();
        Whistle();
    }

    private void Move()
    {
        moveInput = InputProvider.playerActions.GameActions.Move.ReadValue<Vector2>();

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
        animator.SetFloat("ForwardVelocity", (Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) * currentVelocity).z / sprintSpeed);
        animator.SetFloat("RightVelocity", (Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) * currentVelocity).x / sprintSpeed);
        animator.SetFloat("CurrentSpeed", (currentVelocity).magnitude / sprintSpeed);
    }

    private float CalculateSpeed()
    {
        if(InputProvider.playerActions.GameActions.Sprint.IsPressed())
        {
            return sprintSpeed;
        }
        else
        {
            return baseSpeed;
        }
    }

    private void Whistle()
    {
        bool whistleInput = InputProvider.playerActions.GameActions.Whistle.WasPressedThisFrame();

        if (whistleInput == true && whistling == false)
        {
            whistling = true;
            SoundData sfx = audioManager.Database.LookUpRandomSound("Whistle");
            audioManager.PlaySound(sfx, transform);
            NoiseMaker.CreateNoise(transform.position, whistleRange, this.gameObject);
            whistling = false;
        }
    }
}
