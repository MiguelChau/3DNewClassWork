using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chau.StateMachine;

public enum PlayerStates
{
    IDLE,
    RUN,
    JUMPING
}

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator currentPlayer;
    public StateMachine<PlayerStates> stateMachine;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private void Awake()
    {
        currentPlayer = GetComponent<Animator>();
        stateMachine = new StateMachine<PlayerStates>();
    }

    private void Start()
    {
        stateMachine.Init();
        stateMachine.RegisterStates(PlayerStates.IDLE, new IdleState());
        stateMachine.RegisterStates(PlayerStates.RUN, new WalkingState());
        stateMachine.RegisterStates(PlayerStates.JUMPING, new JumpingState());

        stateMachine.SwitchState(PlayerStates.IDLE);
    }

    private void Update()
    {
        stateMachine.Update();
        Move();
        PlayJump();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    private void PlayJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
        }
    }

    public void MoveForward()
    {
        stateMachine.SwitchState(PlayerStates.RUN);
    }

    public void Stop()
    {
        stateMachine.SwitchState(PlayerStates.IDLE);
    }

    public void Jump()
    {
        stateMachine.SwitchState(PlayerStates.JUMPING);
    }
}
