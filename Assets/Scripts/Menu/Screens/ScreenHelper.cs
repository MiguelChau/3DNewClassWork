using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Screens
{
    public class ScreenHelper : MonoBehaviour
    {
        public ScreenType screenType;

        public float delayOnLoad = .5f;


        public void OnClick()
        {
            StartCoroutine(ShowScreenWithDelay());
            
        }

        private IEnumerator ShowScreenWithDelay()
        {
            yield return new WaitForSeconds(delayOnLoad);
            ScreenManager.Instance.ShowByType(screenType);
        }
    }

}
