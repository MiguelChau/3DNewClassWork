using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemCollactableBase : MonoBehaviour
    {
        public ItemType itemType;

        public string compareTag = "Player";
        public GameObject graphicItem;
        public float timeToHide = 2;

        [Header("Collider")]
        public Collider itemCollider;

        [Header("Sounds")]
        public AudioSource audioSource;

        [Header("VFX")]
        public ParticleSystem itemParticleSystem;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.CompareTag(compareTag))
            {
                Collect();
            }
        }
        protected virtual void Collect()
        {
            if (itemCollider != null) itemCollider.enabled = false;
            if (graphicItem != null) graphicItem.SetActive(false);
            Invoke("HideObject", timeToHide);
            OnCollect();
        }

        private void HideObject()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnCollect()
        {
            if (itemParticleSystem != null) itemParticleSystem.Play();
            if (audioSource != null) audioSource.Play();
            ItemManager.Instance.AddByType(itemType);
        }
    }
}

