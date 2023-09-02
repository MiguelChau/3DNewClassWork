using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class ClothItemBase : MonoBehaviour
    {
        public SFXType sfxType;
        public ClothType clothType; 
        public float duration = 2f;

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
        [NaughtyAttributes.Button]
        private void PlaySFX()
        {
            SFXPool.Instance.Play(sfxType);
        }
        public virtual void Collect() 
        {
            PlaySFX();

            HideObject();
        }

        private void HideObject()
        {
            gameObject.SetActive(false);
        }
    }
}

