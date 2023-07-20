using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Animation;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        public Collider enemyCollider;
        public ParticleSystem enemyParticleSystem;
        public ParticleSystem enemyDeathParticleSystem;
        public float startLife = 10f;
        public bool lookAtPlayer = false;

        [SerializeField] private float _currentLife;

        [Header("Animation")]
        [SerializeField] private AnimationBase _animationBase;

        [Header("Start Animation")]
        public float startAnimationDuration = .2f;
        public Ease startAnimationEase = Ease.OutBack;
        public bool startWithBornAnimation = true;

        private PlayerController _player;

        private void Awake()
        {
            Init();
        }
        private void Start()
        {
            _player = GameObject.FindObjectOfType<PlayerController>();
        }
        protected void ResetLife()
        {
            _currentLife = startLife;
        }

        protected virtual void Init()
        {
            ResetLife();
            if (startWithBornAnimation)
            {
                transform.localScale = Vector3.zero;
                BornAnimation();
            }
        }

        protected virtual void Kill()
        {
            if (enemyDeathParticleSystem != null) enemyDeathParticleSystem.Play();
            OnKill();
        }


        protected virtual void OnKill()
        {
            if (enemyCollider != null) enemyCollider.enabled = false;
            Destroy(gameObject, 3f);
            PlayAnimationByTrigger(AnimationType.DEATH);

        }

        public void OnDamage(float f)
        {
            if (enemyParticleSystem != null) enemyParticleSystem.Play();
            _currentLife -= f;

            if (_currentLife <= 0)
            {
                Kill();
            }
        }

        #region ANIMATION
        private void BornAnimation()
        {
            transform.localScale = Vector3.zero; 

            transform.DOScale(Vector3.one, startAnimationDuration)
                .SetEase(startAnimationEase);
        }

        public void PlayAnimationByTrigger(AnimationType animationType)
        {
            _animationBase.PlayAnimationByTrigger(animationType);
        }
        #endregion
        
        public void Damage(float damage)
        {
            Debug.Log("Damage");
            OnDamage(damage);
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
        }
    }

}
