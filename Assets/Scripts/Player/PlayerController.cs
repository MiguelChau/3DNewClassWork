using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;

public class PlayerController : MonoBehaviour, IDamageable
{
    public Animator myAnimator;
    public CharacterController characterController;
    public HealthBase healthBase;

    public float speed = 1f;
    public float turnSpeed = 1f; 
    public float gravity = -9.8f;

    [Header("Keys Moviment")]
    public KeyCode diagonalLeft = KeyCode.Q;
    public KeyCode diagonalRight = KeyCode.E;

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

    public void Damage(float damage)
    {
        healthBase.Damage(damage);
    }


    private void Update()
    {
        _playerStateMachine.Update();


        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0); 

        var inputAxisVertical = Input.GetAxis("Vertical");
        var inputAxisHorizontal = Input.GetAxis("Horizontal");
        int diagonalInput = Input.GetKey(diagonalLeft) ? -1 : Input.GetKey(diagonalRight) ? 1 : 0;

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

            
            speedVector = (transform.forward * inputAxisVertical + transform.right * diagonalInput) * speed * diagonalFactor;

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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with player detected.");
        BossBase boss = collision.gameObject.GetComponent<BossBase>();
        if (boss != null)
        {
            
            boss.PrepareAttack();
        }

    }
}
