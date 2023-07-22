using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chau.StateMachine;
using DG.Tweening;

namespace Enemy
{
    public enum EnemyAction
    {
        INIT,
        IDLE,
        WALK,
        ATTACK,
        DEATH
    }
    public class EnemyBaseSM : MonoBehaviour
    {
        public Collider enemyCollider;
        public bool lookAtPlayer = false;
        public Animator enemyAnimator;
        public float radiusToDetectPlayer = 10f;

        [Header("Animation")]
        public float startAnimationDuration = .5f;
        public Ease startAnimationEase = Ease.OutBack;
        public bool startWithBornAnimation = true;

        [Header("Attack")]
        public int attackAmount = 5;
        public float timeBetweenAttacks = .5f;

        public float speed = 5f;
        public List<Transform> waypoints;

        public HealthBase healthBase;

        private PlayerController _player;

        private StateMachine<EnemyAction> stateMachine;

        private void Awake()
        {
            Init();
            healthBase.OnKill += OnEnemyKill;
        }

        private void Start()
        {
            enemyAnimator = GetComponent<Animator>();
            _player = GameObject.FindObjectOfType<PlayerController>();

        }

        private void Init()
        {
            stateMachine = new StateMachine<EnemyAction>();
            stateMachine.Init();

            stateMachine.RegisterStates(EnemyAction.INIT, new EnemyStateInit());
            stateMachine.RegisterStates(EnemyAction.WALK, new EnemyStateWalk());
            stateMachine.RegisterStates(EnemyAction.ATTACK, new EnemyStateAttack());
            stateMachine.RegisterStates(EnemyAction.DEATH, new EnemyStateDeath());
        }

        #region HEALTH
        private void OnEnemyKill(HealthBase h)
        {
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
            int attacks = 0;
            while (attacks < attackAmount)
            {
                attacks++;
                transform.DOScale(1.1f, .1f).SetLoops(2, LoopType.Yoyo);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

            endCallback?.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            PlayerController p = collision.transform.GetComponent<PlayerController>();

            if (p != null)
            {
                p.Damage(1);
            }
        }

        public virtual void Update()
        {
            if (lookAtPlayer)
            {
                transform.LookAt(_player.transform.position);
            }

            
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            if (distanceToPlayer <= radiusToDetectPlayer)
            {
                SwitchState(EnemyAction.WALK);
                SwitchState(EnemyAction.ATTACK);
                StartAttack();
            }
        }
        #endregion

        #region WALK

        public void GoToRandomPoint(Action onArrive = null)
        {
            StartCoroutine(GoToPointCoroutine(waypoints[UnityEngine.Random.Range(0, waypoints.Count)], onArrive));
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
        }

        private void BornAnimation()
        {
            transform.localScale = Vector3.zero;

            transform.DOScale(Vector3.one, startAnimationDuration)
                .SetEase(startAnimationEase);
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
            stateMachine.SwitchState(state, this);
        }
        #endregion
    }

}