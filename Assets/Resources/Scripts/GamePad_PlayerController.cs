using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

//Code for Unitys New InputSystem

public class GamePad_PlayerController : MonoBehaviour
{
   

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float controllerDeadzone = 0.1f;
    [SerializeField] private float gamepadRotateSmoothing = 1000f;
    [SerializeField] private float _bulletSpeed = 30f;
    [SerializeField] private bool isGamepad;

    private CharacterController controller;
    private Vector2 movement;
    private Vector2 aim;
    private Vector3 playerVelocity;
    private PlayerControls playerControls;

    private PlayerInput playerInput;
    private float _shootDelay = .12f;
    private float _shootTimer;
    
    public AudioClip hitSound;
    public AudioClip shootSound;
    private AudioSource _audioSource;
    public bool shoot;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    //Controls for Gamepad
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        playerControls.Controls.Shoot.performed += _ => Shoot();
        playerControls.Controls.Shoot.performed += _ => ShootRapid();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
   
    // Update is called once per frame
    //Movements For the Main Player For New Input System 
    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
        
    }

    void HandleInput()
    {
        movement = playerControls.Controls.Movement.ReadValue<Vector2>();
        aim = playerControls.Controls.Aim.ReadValue<Vector2>();
    }
    void HandleMovement()
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }
    void HandleRotation()
    {
        if (isGamepad)
        {

            if (Mathf.Abs(aim.x) > controllerDeadzone || Mathf.Abs(aim.y) > controllerDeadzone)
            {
                Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;
                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, gamepadRotateSmoothing * Time.deltaTime);
                }
            }
        }
    }

        public void OnDeviceChange(PlayerInput pi)
    {
    //"isGamepad" must be checked during gameplay in unity's Inspector Tab when "Player" is selected in Hierarchy for handleRotation to take affect
        isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;

    }

    //Shooting System For Main Player 
    void Shoot()
    {
        GameObject bullet = Resources.Load("Prefabs/PlayerBullet") as GameObject;

        _audioSource.PlayOneShot(shootSound, 0.75f);

        Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody>()
              .AddForce(transform.forward * _bulletSpeed, ForceMode.Impulse);

    }
    void ShootRapid()
    {
        _shootTimer -= Time.deltaTime;

        if (_shootTimer < 0)
        {
            Shoot();
            _shootTimer += _shootDelay;
        }
    }

}
