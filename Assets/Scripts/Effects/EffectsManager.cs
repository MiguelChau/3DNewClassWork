using System.Collections;
using UnityEngine;
//using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using Core.Singleton;

public class EffectsManager : Singleton<EffectsManager>
{
    public PostProcessVolume processVolume;
    [SerializeField] private Vignette _vignette;
    public float duration = .1f;
    private bool _vignetteActive = false;


    public void ChangeVignette(bool activate)
    {
        if(activate && !_vignetteActive)
        {
            StartCoroutine(FlashColorVignette());
            _vignetteActive = true;
        }
        else if (!activate && _vignetteActive)
        {
            StopCoroutine(FlashColorVignette());
            ResetVignette();
            _vignetteActive = false;
            
        }
    }

    private void ResetVignette()
    {
        ColorParameter c = new ColorParameter();
        c.value = Color.black;
        _vignette.color.Override(c);
    }

    IEnumerator FlashColorVignette()
    {
        Vignette tmp;

        if (processVolume.profile.TryGetSettings<Vignette>(out tmp))
        {
            _vignette = tmp;
        }

        ColorParameter c = new ColorParameter();

        float time = 0;
        while (time < duration)
        {
            c.value = Color.Lerp(Color.black, Color.red, time / duration);
            time += Time.deltaTime;
            _vignette.color.Override(c);

            yield return new WaitForEndOfFrame();
        }

        time = 0;
        while (time < duration)
        {
            c.value = Color.Lerp(Color.red, Color.black, time / duration);
            time += Time.deltaTime;
            _vignette.color.Override(c);

            yield return new WaitForEndOfFrame();
        }


    } 
}
