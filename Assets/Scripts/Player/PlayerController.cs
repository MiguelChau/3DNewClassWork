using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;
using Enemy;
using Core.Singleton;
using Cloth;

public class PlayerController : Singleton<PlayerController>
{
    public List<Collider> playerColliders;
    public Animator myAnimator;
    public CharacterController characterController;

    [Header("Life")]
    public HealthBase healthBase;
    private bool _alive = true;
    
    [Header("Keys Moviment")]
    public KeyCode diagonalLeft = KeyCode.Q;
    public KeyCode diagonalRight = KeyCode.E;
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = -9.8f;

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.Z;
    public float speedRun = 40f;

    [Header("Jump Setup")]
    public float jumpSpeed = 15f;
    private bool _isJumping = false;

    [Header("VFX")]
    public ParticleSystem immolationAuraVFX;

    [Space]
    [SerializeField] private ClothChange _clothChange;

    private bool _isCastingMagic = false;
    private float vSpeed = 0f;
    public PlayerStateMachine _playerStateMachine;
    private float targetScale;

    private void OnValidate()
    {
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
    }

    protected override void Awake()
    {
        base.Awake();
        OnValidate();

        CheckLoadedData();

        healthBase.OnDamage += Damage;
        healthBase.OnKill += OnKill;
    }

    private void CheckLoadedData()
    {
        var saveSetup = SaveManager.Instance.Setup;

        if(saveSetup != null)
        {
            if(saveSetup.playerPositionHasValue)
            {
                transform.position = saveSetup.playerPosition;
            }

            if(saveSetup.playerHealth != -1)
            {
                healthBase.SetLife(saveSetup.playerHealth);
            }
        }
    }

    public void SetTargetScale(float targetScale)
    {
        this.targetScale = targetScale;
    }

    public void SetInvulnerable()
    {
        healthBase.SetInvulnerable();
    }

    private void Start()
    {
        _playerStateMachine = new PlayerStateMachine(myAnimator);
        _playerStateMachine.Init();

        immolationAuraVFX = GetComponentInChildren<ParticleSystem>();
    }

   
    #region LIFE
    private void OnKill(HealthBase h)
    {
        if (_alive)
        {
            _alive = false;
            _playerStateMachine.Death(h);
            playerColliders.ForEach(i => i.enabled = false);

            Invoke(nameof(RevivePlayer), 3f);
        }
    }

    public void Damage(HealthBase h)
    {
        Debug.Log("Player took Damage");
        if ((float)h._currentLife / h.startLife < 0.5f)
        {
            EffectsManager.Instance.ChangeVignette(true);
        }
        else
        {
            EffectsManager.Instance.ChangeVignette(false);
        }

        ShakeCameraOnDamage.Instance.ShakeCam();
    }

    private void RevivePlayer()
    {
        _playerStateMachine.Revive();
        _alive = true;
        healthBase.ResetLife();
        RespawnPlayer();
        Invoke(nameof(TurnOnColliders), .1f);
    }

    private void TurnOnColliders()
    {
        playerColliders.ForEach(i => i.enabled = true);
    }

    #endregion
    public void AttackMagic()
    {
        
        myAnimator.SetBool("CastingMagic", true);

        
        _isCastingMagic = true;

        
        _playerStateMachine.SwitchState(PlayerStates.ATTACK);
    }

    public void StopCastingMagic()
    {
       
        myAnimator.SetBool("CastingMagic", false);


        _isCastingMagic = false;

       
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
        myAnimator.SetBool("CastingMagic", false);


    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with player detected.");
        BossBase boss = collision.gameObject.GetComponent<BossBase>();
        if (boss != null)
        {
            
            boss.PrepareAttack();
        }

        EnemyBaseSM enemy = collision.gameObject.GetComponent<EnemyBaseSM>();
        if( enemy != null)
        {
            enemy.StartAttack();
        }

    }


    public void RespawnPlayer()
    {
        if (CheckPointManager.Instance.HasCheckPoint())
        {
            characterController.enabled = false;
            transform.position = CheckPointManager.Instance.GetPositionFromLastCheckPoint();
            characterController.enabled = true;
        }
    }

    public void ChangeSpeed(float speed, float duration)
    {
        StartCoroutine(ChangeSpeedCoroutine(speed, duration));
    }

    IEnumerator ChangeSpeedCoroutine(float localSpeed, float duration)
    {
        var defaultSpeed = speed;
        speed = localSpeed;
        yield return new WaitForSeconds(duration);
        speed = defaultSpeed;
    }

    public void ChangeTexture(ClothSetup setup, float duration)
    {
        StartCoroutine(ChangeTextureCoroutine(setup, duration));
    }

    IEnumerator ChangeTextureCoroutine(ClothSetup setup, float duration)
    {
        _clothChange.ChangeTexture(setup);
        yield return new WaitForSeconds(duration);
        _clothChange.ResetTexture();
    }

    public void ActivateImmolationAura(float duration, float damagePerSecond)
    {
        StartCoroutine(ActivateImmolationAuraCoroutine(duration, damagePerSecond));
    }

    private IEnumerator ActivateImmolationAuraCoroutine(float duration, float damagePerSecond)
    {
        var immolationSetup = ClothManager.Instance.GetSetuByType(ClothType.IMMOLATION_AURA);
        _clothChange.ChangeTexture(immolationSetup);

        
        if (immolationAuraVFX != null)
        {
            immolationAuraVFX.Play();
        }


        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            
            ApplyDamageToNearbyEnemies(damagePerSecond * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }



        if (immolationAuraVFX != null)
        {
            immolationAuraVFX.Stop();
        }
        _clothChange.ResetTexture();
    }

    private void ApplyDamageToNearbyEnemies(float damagePerSecond)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

        foreach (Collider collider in colliders)
        {
            Debug.Log("Damage immoaura ");
            EnemyBaseSM enemy = collider.GetComponent<EnemyBaseSM>();
            if (enemy != null)
            {
                enemy.healthBase.Damage(damagePerSecond);
            }
            
            BossBase boss = collider.GetComponent<BossBase>();
            if (boss != null)
            {
                boss.healthBase.Damage(damagePerSecond);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f); 
    }

}
