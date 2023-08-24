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

    [SerializeField]
    private GameObject playerModel;

    [HideInInspector]
    public Material _defaultMaterial; // Default material for the character

    public IEnumerator StartAttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    public IEnumerator StartHitStunTime(float stunTime)
    {
        // Change the color of all child renderers to show they're stunned
        foreach (Renderer renderer in playerModel.GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = _stunnedColor;
        }

        GetComponent<DashAttack>().StopAttack();

        _canAttack = false;
        _isStunned = true;

        yield return new WaitForSeconds(stunTime);

        // Revert the material of all child renderers to the default material
        foreach (Renderer renderer in playerModel.GetComponentsInChildren<Renderer>())
        {
            renderer.material = _defaultMaterial;
        }

        _canAttack = true;
        _isStunned = false;
        GetComponent<MovementSystem>().CanMove = true;
    }
}
