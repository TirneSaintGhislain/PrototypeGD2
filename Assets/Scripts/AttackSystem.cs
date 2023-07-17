using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSystem : MonoBehaviour
{
    [SerializeField]
    private InputAction _lightAttack;
    [SerializeField]
    private InputAction _heavyAttack;
    [SerializeField]
    private InputAction _dashAttack;

    public float HeavyArea { set => _heavyArea = value; }
    public float DashDamage { set => _dashDamage = value; }
    public int LightHits { set => _lightHits = value; }

    private float _heavyArea;
    private int _lightHits;
    private float _dashDamage;

    void Awake()
    {
        _lightAttack.Enable();
        _heavyAttack.Enable();
        _dashAttack.Enable();
    }
    void Update()
    {
        CheckAttackInput();
    }

    void CheckAttackInput()
    {
        if (_lightAttack.IsPressed())
        {
            FireLightAttack();
        }
        if (_heavyAttack.IsPressed())
        {
            FireHeavyAttack();
        }
        if (_dashAttack.IsPressed())
        {
            FireDashAttack();
        }

    }

    void FireLightAttack()
    {

    }

    void FireHeavyAttack()
    {

    }

    void FireDashAttack()
    {

    }
}
