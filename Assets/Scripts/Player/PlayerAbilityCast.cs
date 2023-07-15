using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityCast : PlayerAbilityBase
{

    public MagicBase[] magicBase;
    public Transform castPosition;

    private MagicBase _currentMagic;
    private int currentMagicIndex;
    protected override void Init()
    {
        base.Init();

        CreateMagic();

        inputs.GamePlay.Cast.performed += ctx => StartCast();
        inputs.GamePlay.Cast.canceled += ctx => StopCast();
        inputs.GamePlay.Cast.performed += ctx => SwitchSpell(0);
        inputs.GamePlay.Cast2.performed += ctx => SwitchSpell(1);
    }

    private void CreateMagic()
    {
        currentMagicIndex = 0;
        _currentMagic = Instantiate(magicBase[currentMagicIndex], castPosition);

       _currentMagic.transform.localPosition = _currentMagic.transform.localEulerAngles = Vector3.zero;
    }

    private void StartCast()
    {
        _currentMagic.StartCast();
        Debug.Log("Fire");
    }

    private void StopCast()
    {
        Debug.Log("Fire");
        _currentMagic.StopCast();
    }

    private void SwitchSpell(int newIndex)
    {
        if (newIndex < 0 || newIndex >= magicBase.Length)
            return;

        if (currentMagicIndex != newIndex)
        {
            Destroy(_currentMagic.gameObject);
            currentMagicIndex = newIndex;
            _currentMagic = Instantiate(magicBase[currentMagicIndex], castPosition);
            _currentMagic.transform.localPosition = _currentMagic.transform.localEulerAngles = Vector3.zero;
        }
    }
}
