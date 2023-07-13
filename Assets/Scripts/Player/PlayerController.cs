using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator myAnimator;
    public CharacterController characterController;

    public float speed = 1f;
    public float turnSpeed = 1f; 
    public float gravity = -9.8f; 

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.Z;
    public float speedRun = 40f;

    [Header("Jump Setup")]
    public float jumpSpeed = 15f;
    private bool _isJumping = false;
    

    private float vSpeed = 0f;
    public PlayerStateMachine _playerStateMachine;

    private void Start()
    {
        _playerStateMachine = new PlayerStateMachine();
        _playerStateMachine.Init();
    }

    private void Update()
    {
        _playerStateMachine.Update();


        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0); 

        var inputAxisVertical = Input.GetAxis("Vertical");
        var inputAxisHorizontal = Input.GetAxis("Horizontal");
        var speedVector = transform.forward * inputAxisVertical * speed;

        if (characterController.isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vSpeed = jumpSpeed;
                _isJumping = true;
                _playerStateMachine.Jump();
                
            }
            else
            {
                _isJumping = false;
            }
            
        }
        var isWalking = inputAxisVertical != 0 || inputAxisHorizontal != 0;
        if(isWalking)
        {
            _playerStateMachine.MoveForward();
        }
        else
        {
            _playerStateMachine.Stop();
        }

        if (isWalking && (Mathf.Abs(inputAxisVertical) > 0 || Mathf.Abs(inputAxisHorizontal) > 0))
        {
            
            float diagonalFactor = Mathf.Sqrt(0.5f);

            
            speedVector = (transform.forward * inputAxisVertical + transform.right * inputAxisHorizontal) * speed * diagonalFactor;

            if (Input.GetKey(keyRun))
            {
                speedVector *= speedRun;
                myAnimator.speed = speedRun;
            }
            else
            {
                myAnimator.speed = 1;
            }
        }


        vSpeed -= gravity * Time.deltaTime; 
        speedVector.y = vSpeed; 

        characterController.Move(speedVector * Time.deltaTime);  



        myAnimator.SetBool("Run", isWalking);  
        myAnimator.SetBool("Jump", _isJumping);  

    }
}
