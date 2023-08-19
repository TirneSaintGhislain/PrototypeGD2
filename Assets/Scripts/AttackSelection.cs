using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject _attackChoiceUI;
    [SerializeField]
    private Button _lightAttackButton;
    [SerializeField]
    private Button _altLightAttackButton;
    [SerializeField]
    private Button _heavyAttackButton;
    [SerializeField]
    private Button _altHeavyAttackButton;
    [SerializeField]
    private Button _dashAttackButton;
    [SerializeField]
    private Button _altDashAttackButton;

    private AttackSystem _attackSystem;
    private MovementSystem _movementSystem;

    public void SelectAttacks(AttackSystem attackSystem, MovementSystem movementSystem)
    {
        _attackChoiceUI.SetActive(true);
        _lightAttackButton.enabled = true;
        _altLightAttackButton.enabled = true;
        _lightAttackButton.Select();
        attackSystem.ActiveAttack = true;
        movementSystem.CanMove = false;
        _attackSystem = attackSystem;
        _movementSystem = movementSystem;
    }

    public void SetLightAttack(bool alternate)
    {
        _attackSystem.AlternateLightAttack = alternate;
        _heavyAttackButton.enabled = true;
        _altHeavyAttackButton.enabled = true;
        _heavyAttackButton.Select();
        _lightAttackButton.enabled = false;
        _altLightAttackButton.enabled = false;
    }

    public void SetHeavyAttack(bool alternate)
    {
        _attackSystem.AlternateHeavyAttack = alternate;
        _dashAttackButton.enabled = true;
        _altDashAttackButton.enabled = true;
        _dashAttackButton.Select();
        _heavyAttackButton.enabled = false;
        _altHeavyAttackButton.enabled = false;
    }

    public void SetDashAttack(bool alternate)
    {
        _attackSystem.AlternateDashAttack = alternate;
        _dashAttackButton.enabled = false;
        _altDashAttackButton.enabled = false;
        DisableAttackSelection();
    }

    public void DisableAttackSelection()
    {
        _attackSystem.ActiveAttack = false;
        _movementSystem.CanMove = true;
        _attackChoiceUI.SetActive(false);
    }


}
