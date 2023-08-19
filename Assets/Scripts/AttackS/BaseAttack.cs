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

    public int AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public bool AttackEnabled { get; set; }

    [HideInInspector]
    public int _attackDamage;

    public void ChangeAttackSpeed(float multiplier)
    {
        int newStartupFrames = (int)Math.Floor(_startupFrames * multiplier);
        int newActiveFrames = (int)Math.Floor(_activeFrames * multiplier);
        int newCooldownFrames = (int)Math.Floor(_cooldownFrames * multiplier);

        if(newStartupFrames <= 0)
        {
            newStartupFrames = 1;
        }
        if(newActiveFrames <= 0)
        {
            newActiveFrames = 1;
        }
        if(newCooldownFrames <= 0)
        {
            newCooldownFrames = 1;
        }

        _startupFrames = newStartupFrames;
        _activeFrames = newActiveFrames;
        _cooldownFrames = newCooldownFrames;
    }

    public void ChangeAttackSpeed(int startupFrames, int activeFrames, int cooldownFrames)
    {
        if (startupFrames <= 0)
        {
            startupFrames = 1;
        }
        if (activeFrames <= 0)
        {
            activeFrames = 1;
        }
        if (cooldownFrames <= 0)
        {
            cooldownFrames = 1;
        }

        _startupFrames = startupFrames;
        _activeFrames = activeFrames;
        _cooldownFrames = cooldownFrames;
    }

    //protected bool _startupFinished = false;
    //protected bool _activeFinished = false;
    //protected bool _cooldownFinished = false;

    protected virtual void Start()
    {
        _attackSystem = GetComponent<AttackSystem>();
    }

    public abstract void StartAttack(InputAction.CallbackContext context);

    public virtual void StopAttack()
    {
        StopAllCoroutines();
        Cleanup();
    }
    protected virtual void Cleanup()
    {
        _attackSystem.ActiveAttack = false;
        _attackSystem.InActiveFrames = false;
        _attackSystem.InStartupFrames = false;
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
        CooldownFinishedEvent();
    }
    private IEnumerator ActiveFrames()
    {
        for (int i = 0; i < _activeFrames; i++)
        {
            ActiveEvent();
            yield return null;
        }
        ActiveFinishedEvent();
    }
    private IEnumerator StartupFrames()
    {
        for (int i = 0; i < _startupFrames; i++)
        {
            StartupEvent();
            yield return null;
        }
        StartupFinishedEvent();
    }
}
