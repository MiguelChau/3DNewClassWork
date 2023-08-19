using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chau.StateMachine;
using DG.Tweening;
using Animation;

namespace Enemy
{
    public enum EnemyAction
    {
        NONE = 0,
        INIT,
        IDLE,
        WALK,
        ATTACK,
        DEATH,
        SHOOT
    }

    public class EnemyBaseSM : MonoBehaviour
    {
        private enum EnemyAttackStage
        {
            None,
            Melee
        }

        public ParticleSystem enemyDeathParticleSystem;
        public ParticleSystem enemyDamageParticleSystem;
        public Collider enemyCollider;
        public bool lookAtPlayer = false;
        protected bool _playerDetected = false;

        public float radiusToDetectPlayer = 10f;

        [Header("Animation")]
        public float startAnimationDuration = .5f;
        public Ease startAnimationEase = Ease.OutBack;
        public bool startWithBornAnimation = true;

        [Header("Attack")]
        public Transform hornMidTipTransform;
        public float hornRadius = 2f;
        public int attackAmount = 5;
        public float timeBetweenAttacks = .5f;

        public float speed = 5f;
        public List<Transform> waypoints;

        public AnimationBase animationBase;
        public HealthBase healthBase;

        private PlayerController _player;

        protected internal StateMachine<EnemyAction> stateMachine;

        private bool attacking = false;

        private EnemyAttackStage attackStage = EnemyAttackStage.None;

        private void Awake()
        {
            _player = GameObject.FindObjectOfType<PlayerController>();
            if (_player == null)
            {
                Debug.LogError("PlayerController não encontrado!");
            }
            Init();
            healthBase.OnKill += OnEnemyKill;
        }

        private void Start()
        {
            StartInitAnimation();
        }

        protected virtual void Init()
        {
            stateMachine = new StateMachine<EnemyAction>();
            stateMachine.Init();

            stateMachine.RegisterStates(EnemyAction.INIT, new EnemyStateInit());
            stateMachine.RegisterStates(EnemyAction.WALK, new EnemyStateWalk());
            stateMachine.RegisterStates(EnemyAction.ATTACK, new EnemyStateAttack());
            stateMachine.RegisterStates(EnemyAction.DEATH, new EnemyStateDeath());

            if (this is EnemyShooter)
            {
                stateMachine.RegisterStates(EnemyAction.SHOOT, new EnemyStateShoot());
            }

            SwitchState(EnemyAction.INIT);
        }

        #region ANIMATION

        public void AttackAnimationEvent()
        {
            if (attackStage == EnemyAttackStage.Melee)
            {
                var colliders = Physics.OverlapSphere(hornMidTipTransform.position, hornRadius);
                for (int i = 0; i < colliders.Length; ++i)
                {
                    if (colliders[i].gameObject.tag == "Enemy")
                        continue;

                    if (colliders[i].gameObject.TryGetComponent<IDamageable>(out var dmg))
                    {
                        dmg.Damage(5f);
                    }
                }
            }
        }
        public void StartInitAnimation()
        {
            HealthBase healthBase = GetComponent<HealthBase>();
            if (healthBase != null)
            {
                healthBase.ResetLife();
            }

            if (startWithBornAnimation)
            {
                BornAnimation();
            }

            SwitchState(EnemyAction.WALK);
        }

        private void BornAnimation()
        {
            transform.localScale = Vector3.zero;

            transform.DOScale(Vector3.one, startAnimationDuration)
                .SetEase(startAnimationEase);
        }
        #endregion

        #region HEALTH
        private void OnEnemyKill(HealthBase h)
        {
            if (enemyDeathParticleSystem != null)
            {
                enemyDeathParticleSystem.Play();
            }
            SwitchState(EnemyAction.DEATH);
        }
        #endregion

        #region ATTACK
        public void StartAttack(Action endCallback = null)
        {
            StartCoroutine(AttackCoroutine(endCallback));
        }

        IEnumerator AttackCoroutine(Action endCallback)
        {
            attackStage = EnemyAttackStage.Melee;

            int attacks = 0;
            attacking = true;
            //animationBase.PlayAnimationByTrigger(AnimationType.ATTACK);
            while (attacks < attackAmount)
            {
                attacks++;
                transform.DOScale(1.1f, .1f).SetLoops(2, LoopType.Yoyo);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

            endCallback?.Invoke();
            attacking = false;

            attackStage = EnemyAttackStage.None;
        }

        private void OnCollisionEnter(Collision collision)
        {
            PlayerController p = collision.transform.GetComponent<PlayerController>();

            if (p != null)
            {
                                
              p.healthBase.Damage(1);
                
            }
           
        }

        public virtual void Update()
        {
            if (lookAtPlayer)
            {
                transform.LookAt(_player.transform.position);
            }

            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

            if (_playerDetected)
            {
                if (distanceToPlayer > radiusToDetectPlayer)
                {
                    SwitchState(EnemyAction.WALK);
                    _playerDetected = false;
                }
                else
                {
                    if (this is EnemyShooter)
                    {
                        SwitchState(EnemyAction.SHOOT);
                        if (!attacking)
                        {
                            StartShoot(() =>
                            {
                                SwitchState(EnemyAction.WALK);
                            });
                        }
                    }
                    else
                    {
                        SwitchState(EnemyAction.ATTACK);
                        if (!attacking)
                        {
                            StartAttack();
                        }

                        if (animationBase != null)
                        {
                            animationBase.PlayAnimationByTrigger(AnimationType.ATTACK);
                        }
                    }
                }
            }
            else
            {
                if (distanceToPlayer <= radiusToDetectPlayer)
                {
                    _playerDetected = true;
                }
            }
        }
        #endregion

        private void OnDisable()
        {
            transform.DOKill();
        }

        #region WALK

        public void GoToRandomPoint(Action onArrive = null)
        {
            if(waypoints.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, waypoints.Count);
                StartCoroutine(GoToPointCoroutine(waypoints[randomIndex], onArrive));
            }

        }

        IEnumerator GoToPointCoroutine(Transform t, Action onArrive = null)
        {
            while (Vector3.Distance(transform.position, t.position) > 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.position, Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }

            onArrive?.Invoke();
        }
        #endregion

        

        #region SHOOT

        public virtual void StartShoot(Action endCallback = null)
        {
            throw new InvalidOperationException("Just Shooter shoots.");
        }

        #endregion

        #region DEBUG 
        [NaughtyAttributes.Button]
        private void SwitchInit()
        {
            SwitchState(EnemyAction.INIT);

        }
        [NaughtyAttributes.Button]

        private void SwitchWalk()
        {
            SwitchState(EnemyAction.WALK);

        }
        [NaughtyAttributes.Button]
        private void SwitchAttack()
        {
            SwitchState(EnemyAction.ATTACK);

        }

        #endregion

        #region STATE MACHINE
        public void SwitchState(EnemyAction state)
        {
            if(stateMachine.CurrentStateType != state)
            {
                stateMachine.SwitchState(state, this);
            }
        }
        
        #endregion
    }

}