using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class ClothItemBase : MonoBehaviour
    {
        public ClothType clothType; 
        public float duration = 2f;
        public AudioSource audioSource;

        public string compareTag = "Player";

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.CompareTag(compareTag))
            {
                Debug.Log("Collect");

                var setup = ClothManager.Instance.GetSetuByType(clothType);

                PlayerController.Instance.ChangeTexture(setup, duration);

                Collect();
            }
        }

        public virtual void Collect() 
        {
            if (audioSource != null) audioSource.Play();

            HideObject();
        }

        private void HideObject()
        {
            gameObject.SetActive(false);
        }
    }
}

