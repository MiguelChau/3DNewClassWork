using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chau.StateMachine;
using DG.Tweening;
using Animation;

namespace Boss
{
    public enum BossAction
    {
        NONE = 0,
        INIT,
        IDLE,
        WALK,
        ATTACK,
        DEATH,
        SHOOT
    }

    public class BossBase : MonoBehaviour
    {
        public Collider bossCollider;
        public bool lookAtPlayer = false;
        protected bool _playerDetected = false;

        public float radiusToDetectPlayer = 10f;

        [Header("Animation")]
        public float startAnimationDuration = .5f;
        public Ease startAnimationEase = Ease.OutBack;
        public bool startWithBornAnimation = true;

        [Header("Attack")]
        public int attackAmount = 20;
        public float timeBetweenAttacks = .5f;

        [Header("Shoot")]
        public MagicBase bossMagicBase;
        public float timeBetweenCast = .3f;
        public float speed = 50f;

        public float bossSpeed = 5f;
        public List<Transform> waypoints;

        public AnimationBase animationBase;
        public HealthBase healthBase;

        private PlayerController _player;

        protected internal StateMachine<BossAction> stateMachine;

        private bool attacking = false;
        private Coroutine _currentCoroutine;

        private void Awake()
        {
            Init();
            healthBase.OnKill += OnEnemyKill;
        }

        private void Start()
        {
            _player = GameObject.FindObjectOfType<PlayerController>();
        }

        protected virtual void Init()
        {
            stateMachine = new StateMachine<BossAction>();
            stateMachine.Init();

            stateMachine.RegisterStates(BossAction.INIT, new BossStateInit());
            stateMachine.RegisterStates(BossAction.WALK, new BossStateWalk());
            stateMachine.RegisterStates(BossAction.ATTACK, new BossStateAttack());
            stateMachine.RegisterStates(BossAction.DEATH, new BossStateDeath());
            stateMachine.RegisterStates(BossAction.SHOOT, new BossStateShoot());

            SwitchState(BossAction.INIT);
        }

        #region HEALTH
        private void OnEnemyKill(HealthBase h)
        {
            SwitchState(BossAction.DEATH);
        }
        #endregion

        #region ATTACK
        public void StartAttack(Action endCallback = null)
        {
            StartCoroutine(AttackCoroutine(endCallback));
        }

        IEnumerator AttackCoroutine(Action endCallback)
        {
            int attacks = 0;
            attacking = true;
            while (attacks < attackAmount)
            {
                attacks++;
                transform.DOScale(1.1f, .1f).SetLoops(2, LoopType.Yoyo);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

            endCallback?.Invoke();
            attacking = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            PlayerController p = collision.transform.GetComponent<PlayerController>();

            if (p != null)
            {
                p.Damage(20);
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
                    SwitchState(BossAction.WALK);
                    _playerDetected = false;
                }
                else
                {
                    if (stateMachine.CurrentStateType == BossAction.SHOOT)
                    {
                        Debug.Log("boss shoot");
                        SwitchState(BossAction.SHOOT);
                        if (!attacking)
                        {
                            StartShoot(() =>
                            {
                                SwitchState(BossAction.WALK);
                            });
                        }
                    }
                    else
                    {
                        SwitchState(BossAction.ATTACK);
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

        #region SHOOT
        [NaughtyAttributes.Button]
        public virtual void StartShoot(Action endCallback = null)
        {
            StartCoroutine(ShootCoroutine(endCallback));
        }

        private IEnumerator ShootCoroutine(Action endCallback)
        {
            bossMagicBase.StartCast();

            yield return new WaitForSeconds(timeBetweenCast);


            endCallback?.Invoke();
        }


        #endregion

        #region WALK

        public void GoToRandomPoint(Action onArrive = null)
        {
            if (waypoints.Count > 0)
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

        #region ANIMATION
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

            SwitchState(BossAction.WALK);
        }

        private void BornAnimation()
        {
            transform.localScale = Vector3.zero;

            transform.DOScale(Vector3.one, startAnimationDuration)
                .SetEase(startAnimationEase);
        }
        #endregion

        #region STATE MACHINE
        public void SwitchState(BossAction state)
        {
            if (stateMachine.CurrentStateType != state)
            {
                stateMachine.SwitchState(state, this);
            }
        }

        #endregion
    }
}

