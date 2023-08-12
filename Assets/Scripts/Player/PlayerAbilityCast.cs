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

    private MagicBase[] _magicInstances;

    protected override void Init()
    {
        base.Init();

        CreateMagic();

       
        inputs.GamePlay.Cast1.performed += ctx =>
        {
            SwitchSpell(0);
            StartCast();
        };
        inputs.GamePlay.Cast1.canceled += ctx => StopCast();

        inputs.GamePlay.Cast2.performed += ctx =>
        {
            SwitchSpell(1);
            StartCast();
        };
        inputs.GamePlay.Cast2.canceled += ctx => StopCast();
    }

    private void CreateMagic()
    {
        _magicInstances = new MagicBase[magicBase.Length];

        for(int i = 0; i < _magicInstances.Length; ++i)
        {
            _magicInstances[i] = Instantiate(magicBase[i], castPosition);
            _magicInstances[i].gameObject.SetActive(false);
            _magicInstances[i].transform.localPosition = _magicInstances[i].transform.localEulerAngles = Vector3.zero;
        }
        _currentMagic = _magicInstances[0];
        _currentMagic.gameObject.SetActive(true);
        currentMagicIndex = 0;
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
            currentMagicIndex = newIndex;
            _currentMagic.gameObject.SetActive(false);
            _currentMagic = _magicInstances[currentMagicIndex];
            _currentMagic.gameObject.SetActive(true);
        }
    }
}
