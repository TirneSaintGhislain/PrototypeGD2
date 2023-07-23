using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseAttack : MonoBehaviour
{
    // This is a base class meant to be inherited from when implementing future attack types

    protected AttackSystem _attackSystem;
    [Header("Attack Frame Data")]

    [SerializeField]
    private int _startupFrames;
    [SerializeField]
    private int _activeFrames;
    [SerializeField]
    private int _cooldownFrames;
    [SerializeField]
    protected AttackTypes _thisAttackType;

    //protected bool _startupFinished = false;
    //protected bool _activeFinished = false;
    //protected bool _cooldownFinished = false;

    private void Awake()
    {
        _attackSystem = GetComponent<AttackSystem>();
    }

    public abstract void StartAttack(InputAction.CallbackContext context);


    protected virtual void Cleanup()
    {
        _attackSystem.ActiveAttack = false;
        //_startupFinished = false;
        //_cooldownFinished = false;
        //_activeFinished = false;
    }

    public void StartStartup()
    {
        StartCoroutine(StartupFrames());
    }
    public void StartActive()
    {
        StartCoroutine(ActiveFrames());
    }
    public void StartCooldown()
    {
        StartCoroutine(CooldownFrames());
    }
    protected abstract void CooldownEvent();
    protected abstract void ActiveEvent();
    protected abstract void StartupEvent();
    protected abstract void ActiveFinishedEvent();
    protected abstract void StartupFinishedEvent();
    protected abstract void CooldownFinishedEvent();

    private IEnumerator CooldownFrames()
    {
        for (int i = 0; i < _cooldownFrames; i++)
        {
            CooldownEvent();
            yield return null;
        }
        Debug.Log("Cooldown Finished");
        CooldownFinishedEvent();
    }
    private IEnumerator ActiveFrames()
    {
        for (int i = 0; i < _activeFrames; i++)
        {
            ActiveEvent();
            yield return null;
        }
        Debug.Log("Active Finished");
        ActiveFinishedEvent();
    }
    private IEnumerator StartupFrames()
    {
        for (int i = 0; i < _startupFrames; i++)
        {
            StartupEvent();
            yield return null;
        }
        Debug.Log("Startup Finished");
        StartupFinishedEvent();
    }
}