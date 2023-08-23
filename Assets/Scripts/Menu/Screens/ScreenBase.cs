using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using NaughtyAttributes; 
using DG.Tweening;

namespace Screens
{
    public enum ScreenType
    {
        PANEL,
        ABOUT,
        PLAY,
        OPTIONS,
        LOAD
    }

    public class ScreenBase : MonoBehaviour
    {
        public ScreenType screenType;

        public List<Transform> listOfObjects; 
        public List<Typper> listOfPhrases; 

        public Image uiBackground;
        public bool startHided = false;

        [Header("Animation")]
        public float animationDuration = .3f;
        public float delayBetweenObjects = .05f;

        private void Start()
        {
            if (startHided)
            {
                HideObjects();
            }
        }

        [Button]
        public void Show()
        {
            ShowObjects();
            Debug.Log("Show");
        }
        [Button]
        public void Hide()
        {
            HideObjects();
            Debug.Log("Hide");
        }

        private void HideObjects()
        {
            listOfObjects.ForEach(i => i.gameObject.SetActive(false));
            uiBackground.enabled = false;
        }

        private void ShowObjects()
        {
            for (int i = 0; i < listOfObjects.Count; i++)
            {
                var obj = listOfObjects[i];

                obj.gameObject.SetActive(true); 
                obj.DOScale(0, animationDuration).From().SetDelay(i * delayBetweenObjects); 
            }

            Invoke(nameof(StartType), delayBetweenObjects * listOfObjects.Count);
            uiBackground.enabled = true;
        }

        private void StartType()
        {
            for (int i = 0; i < listOfPhrases.Count; i++)
            {
                listOfPhrases[i].StartType();
            }
        }

        private void ForceShowObjects()
        {
            listOfObjects.ForEach(i => i.gameObject.SetActive(true));
            uiBackground.enabled = true;
        }

    }
}
