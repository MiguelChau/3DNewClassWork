using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chau.StateMachine;
using DG.Tweening;
using Animation;
using UnityEditor;

namespace Boss
{
    public enum BossAction
    {
        NONE = 0,
        INIT,
        IDLE,
        WALK,
        DEATH,
        PREPARE_ATTACK,
        SHOOT,
        CHARGE_MELEE
    }

    public class BossBase : MonoBehaviour
    {
        private enum BossAttackStage
        {
            none,
            Shoot,
            Melee
        }

        public Collider bossCollider;
        public bool lookAtPlayer = false;
        protected bool _playerDetected = false;

        public float radiusToDetectPlayer = 10f;
        public Color gizmoColor = Color.red;
        public float attackRadius = 5f;

        [Header("Animation")]
        public float startAnimationDuration = .5f;
        public Ease startAnimationEase = Ease.OutBack;
        public bool startWithBornAnimation = true;

        [Header("Attack")]
        public Transform jawTipTransform;
        public float biteRadius = 2f;
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

        private BossAttackStage attackStage = BossAttackStage.none;

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
            stateMachine = new StateMachine<BossAction>();
            stateMachine.Init();

            stateMachine.RegisterStates(BossAction.INIT, new BossStateInit());
            stateMachine.RegisterStates(BossAction.WALK, new BossStateWalk());
            stateMachine.RegisterStates(BossAction.PREPARE_ATTACK, new BossStatePrepareAttack());
            stateMachine.RegisterStates(BossAction.DEATH, new BossStateDeath());
            stateMachine.RegisterStates(BossAction.SHOOT, new BossStateShoot());
            stateMachine.RegisterStates(BossAction.CHARGE_MELEE, new BossStateChargeAttack());

            SwitchState(BossAction.INIT);
        }


        #region ANIMATION

        public void AttackAnimationEvent()
        {
            if(attackStage == BossAttackStage.Melee)
            {
                var colliders = Physics.OverlapSphere(jawTipTransform.position, biteRadius);
                for(int i = 0; i < colliders.Length; ++i)
                {
                    if (colliders[i].gameObject.tag == "Boss")
                        continue;

                    if(colliders[i].gameObject.TryGetComponent<IDamageable>(out var dmg))
                    {
                        dmg.Damage(20f);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
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

            SwitchState(BossAction.WALK);
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
            SwitchState(BossAction.DEATH);
        }
        #endregion

        #region PREPARE_ATTACK


        public void PrepareAttack()
        {
            StartCoroutine(PrepareAttackCoroutine());
        }

        private IEnumerator PrepareAttackCoroutine()
        {
            yield return new WaitForSeconds(1);

            float random = UnityEngine.Random.Range(0f, 1f);

            if (random < 0.5f)
            {
                SwitchState(BossAction.SHOOT);
            }
            else
            {
                SwitchState(BossAction.CHARGE_MELEE);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            PlayerController p = collision.transform.GetComponent<PlayerController>();

            if (p != null)
            {
                Debug.Log("Boss detectou o jogador e está causando dano.");
                p.healthBase.Damage(20);
            }
        }

        #endregion

        #region CHARGE ATTACK
        public void ChargeMelee()
        {
            StartCoroutine(ChargeMeleeCoroutine());
        }

        IEnumerator ChargeMeleeCoroutine()
        {
            attackStage = BossAttackStage.Melee;

            while (Vector3.Distance(transform.position, _player.transform.position) > 3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, Time.deltaTime * speed);
                transform.LookAt(_player.transform.position);
                yield return null;
            }

            int attacks = 0;
            attacking = true;
            animationBase.PlayAnimationByTrigger(AnimationType.ATTACK);
            while (attacks < attackAmount)
            {
                attacks++;
                transform.DOScale(1.1f, .1f).SetLoops(2, LoopType.Yoyo);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            attacking = false;

            attackStage = BossAttackStage.none;

            SwitchState(BossAction.PREPARE_ATTACK);
        }
        #endregion

        #region SHOOT
        public virtual void StartShoot(Action endCallback = null)
        {
            StartCoroutine(ShootCoroutine(endCallback));
        }

        private IEnumerator ShootCoroutine(Action endCallback)
        {
            print($"Starting shoot Coroutine");

            attackStage = BossAttackStage.Shoot;

            int numberOfCast = 20;
            float timeBetweenCast = .5f;

            animationBase.PlayAnimationByTrigger(AnimationType.ATTACK);

            int shooterCounter = 0;
            while (shooterCounter < numberOfCast)
            {
                transform.LookAt(_player.transform.position);

                bossMagicBase.Cast();

                float timeCounter = 0;

                while (timeCounter < timeBetweenCast)
                {
                    timeCounter += Time.deltaTime;
                    yield return null;
                }
                shooterCounter++;
            }

            attackStage = BossAttackStage.none;

            SwitchState(BossAction.PREPARE_ATTACK);
        }


        #endregion

        #region WALK

        public void GoToRandomPoint()
        {
            if (_player == null)
            {
                Debug.LogWarning("Referência para o jogador não encontrada.");
                return;
            }

            if (waypoints.Count > 0)
            {
                _currentCoroutine = StartCoroutine(GoToPointCoroutine());
            }
        }

        IEnumerator GoToPointCoroutine()
        {
            while (true)
            {
                int randomIndex = UnityEngine.Random.Range(0, waypoints.Count);
                while (Vector3.Distance(transform.position, waypoints[randomIndex].position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[randomIndex].position, Time.deltaTime * speed);

                    if (Vector3.Distance(transform.position, _player.transform.position) < radiusToDetectPlayer)
                    {

                        SwitchState(BossAction.PREPARE_ATTACK);
                    }

                    yield return new WaitForEndOfFrame();
                }

                float counter = 0;
                while (counter < 2f)
                {
                    counter += Time.deltaTime;
                    if (Vector3.Distance(transform.position, _player.transform.position) < radiusToDetectPlayer)
                    {
                        SwitchState(BossAction.PREPARE_ATTACK);
                    }
                    yield return null;
                }
            }
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

        #region DEBUGS
        [NaughtyAttributes.Button]
        private void SwitchInit()
        {
            SwitchState(BossAction.INIT);

        }

        [NaughtyAttributes.Button]

        private void SwitchWalk()
        {
            SwitchState(BossAction.WALK);

        }

        [NaughtyAttributes.Button]

        private void SwitchPrepareAttack()
        {
            SwitchState(BossAction.PREPARE_ATTACK);

        }

        [NaughtyAttributes.Button]

        private void SwitchChargeAttack()
        {
            SwitchState(BossAction.CHARGE_MELEE);

        }

        [NaughtyAttributes.Button]

        private void SwitchStartShoot()
        {
            SwitchState(BossAction.SHOOT);

        }


        #endregion
    }

       /* #region GIZMOS
        private void OnDrawGizmos()
        {
            if (_player != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, (_player.transform.position - transform.position).normalized * radiusToDetectPlayer);
            }
        }
    
    
    }
        #endregion*/
}