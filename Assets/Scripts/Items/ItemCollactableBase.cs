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
        public Collider[] itemColliders;

        [Header("Sounds")]
        public AudioSource audioSource;

        [Header("VFX")]
        public ParticleSystem itemParticleSystem;

        private void OnValidate()
        {
            itemColliders = GetComponents<Collider>();
        }
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.CompareTag(compareTag))
            {
                Collect();
            }
        }
        protected virtual void Collect()
        {
            if (itemColliders != null)
            {
                for(int i = 0; i < itemColliders.Length; ++i)
                {
                    itemColliders[i].enabled = false;
                }
            }
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

