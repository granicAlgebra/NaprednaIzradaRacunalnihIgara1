using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    public static int MaxHealth { get; set; } = 100;
    public static int CurrentHealth { get; set; } = 50;
    public static int RupleCurrency { get; set; } = 0;
    public static int InventoryCapacity { get; set; } = 32;
    public static int Damage { get; set; } = 1;
    public static int Armour { get; set; } = 1;
    //Brzina kretanja playera u svim smjerovima. 1 = 16 px u sekundi.
    public static int Speed { get; set; } = 3; 
    //Korigira brzinu kod dijagonalnog kretanja => cos 45' = 0.7071 ili korijen iz 2 / 2 =  0.7071.
    public const float DIAGONAL_MOVEMENT_CORRECTION = 0.7071f;

    private Rigidbody2D _playerRigidsBody;
    private Animator _playerAnimator;

    void Start()
    {
        _playerRigidsBody = this.GetComponent<Rigidbody2D>();
        _playerAnimator = this.GetComponent<Animator>();
        _playerAnimator.speed = Speed;
    }


    void Update()
    {
        _playerMovementControl();
        _playerAnimationHandler();
    }

    private void _playerMovementControl()
    {
        float movementSpeed = Speed;
        //Speed up;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = Speed * 2;
            //Brzina animacije prati brzinu kretanja
            _playerAnimator.speed = movementSpeed;
        }
        //Down
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _playerRigidsBody.linearVelocity = new Vector2(0, -1 * movementSpeed);
        }
        //Up
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _playerRigidsBody.linearVelocity = new Vector2(0, 1 * movementSpeed);
        }
        //Left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _playerRigidsBody.linearVelocity = new Vector2(-1 * movementSpeed, 0);
        }
        //Right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _playerRigidsBody.linearVelocity = new Vector2(1 * movementSpeed, 0);
        }
        //Diagonalno kretanje
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            _playerRigidsBody.linearVelocity = new Vector2(-1 * movementSpeed * DIAGONAL_MOVEMENT_CORRECTION, -1 * movementSpeed * DIAGONAL_MOVEMENT_CORRECTION);
        }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            _playerRigidsBody.linearVelocity = new Vector2(1 * movementSpeed * DIAGONAL_MOVEMENT_CORRECTION, -1 * movementSpeed * DIAGONAL_MOVEMENT_CORRECTION);
        }
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            _playerRigidsBody.linearVelocity = new Vector2(-1 * movementSpeed * DIAGONAL_MOVEMENT_CORRECTION, 1 * movementSpeed * DIAGONAL_MOVEMENT_CORRECTION);
        }
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            _playerRigidsBody.linearVelocity = new Vector2(1 * movementSpeed * DIAGONAL_MOVEMENT_CORRECTION, 1 * movementSpeed * DIAGONAL_MOVEMENT_CORRECTION);
        }
        //Kraj kretanja
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            _playerRigidsBody.linearVelocity = new Vector2(0, 0);
        }
        //Kraj trcanja
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = Speed;
            _playerAnimator.speed = movementSpeed;
        }
    }
    //Promjena animacije prilikom kretanja
    private void _playerAnimationHandler()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _playerAnimator.SetBool("IsWalkingDown", true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _playerAnimator.SetBool("IsWalkingUp", true);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _playerAnimator.SetBool("IsWalkingLeft", true);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _playerAnimator.SetBool("IsWalkingRight", true);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            _playerAnimator.SetBool("IsWalkingDown", false);
            _playerAnimator.Play("Player_idle", 0, 0);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            _playerAnimator.SetBool("IsWalkingUp", false);
            _playerAnimator.Play("Player_idle", 0, 0.3f);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _playerAnimator.SetBool("IsWalkingLeft", false);
            _playerAnimator.Play("Player_idle", 0, 0.6f);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            _playerAnimator.SetBool("IsWalkingRight", false);
            _playerAnimator.Play("Player_idle", 0, 0.9f);
        }
    }
}