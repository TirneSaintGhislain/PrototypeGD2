using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitstunSystem : MonoBehaviour
{
    [SerializeField]
    private float _attackCooldown;

    [SerializeField]
    private Color _stunnedColor;

    [HideInInspector]
    public bool _canAttack = true;
    [HideInInspector]
    public bool _isStunned = false;

    [HideInInspector]
    public Color _defaultColor;

    private void Start()
    {
        _defaultColor = GetComponent<Renderer>().material.color;
    }

    public IEnumerator StartAttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    public IEnumerator StartHitStunTime(float stunTime)
    {
        //Changes the player's color to show they're stunned
        GetComponent<Renderer>().material.color = _stunnedColor;
        GetComponent<DashAttack>().StopAttack();

        _canAttack = false;
        _isStunned = true;

        yield return new WaitForSeconds(stunTime);

        GetComponent<Renderer>().material.color = _defaultColor;

        _canAttack = true;
        _isStunned = false;
    }
}
