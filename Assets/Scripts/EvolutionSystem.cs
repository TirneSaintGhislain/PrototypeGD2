using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionSystem : MonoBehaviour
{
    [SerializeField]
    private int _comboGoal;

    [SerializeField]
    private int _lightHitsIncrease;

    [SerializeField]
    private float _heavyAreaIncrease;

    [SerializeField]
    private float _dashDamageIncrease;

    [SerializeField]
    private int _lightHitsMinimum;

    [SerializeField]
    private float _heavyAreaMiminum;

    [SerializeField]
    private float _dashDamageMinimume;

    [SerializeField]
    private int _unusedLimit;

    private List<int> _timesUnused;

    private AttackSystem _attackSystem;
    private int _currentCombo;

    // Start is called before the first frame update
    void Start()
    {
        _attackSystem = GetComponent<AttackSystem>();
    }

    public void SuccesfulHit(int attackIndex)
    {
        //Increase combo length and evolve attack when combo goal is reached
        _currentCombo++;
        if (_currentCombo >= _comboGoal)
        {
            EvolveAttack(attackIndex);
        }

        //Goes over every attack and updates the Unused Amount per attack
        foreach (int attack in _timesUnused)
        {
            //If the attack was used it gets reset to 0, otherwise 1 gets added
            if (_timesUnused.IndexOf(attack) == attackIndex)
            {
                _timesUnused[attackIndex] = 0;
            }
            else if (_timesUnused.IndexOf(attack) != attackIndex)
            {
                _timesUnused[_timesUnused.IndexOf(attack)] += 1;
            }
        }

        CheckIfAttacksNeedToDeEvolve();
    }

    private void EvolveAttack(int attackIndex)
    {
        //Evolve the correct attack
        switch (attackIndex)
        {
            case 0:
                _attackSystem._lightHits += _lightHitsIncrease;
                Debug.Log("Quick Attack Evolved: " + _attackSystem._lightHits);
                break;
            case 1:
                _attackSystem._heavyArea += _heavyAreaIncrease;
                Debug.Log("Strong Attack Evolved: " + _attackSystem._heavyArea);
                break;
            case 2:
                _attackSystem._dashDamage += _dashDamageIncrease;
                Debug.Log("Dash Attack Evolved: " + _attackSystem._dashDamage);
                break;
        }
    }

    private void CheckIfAttacksNeedToDeEvolve()
    {
        //If an attack gets unused too much it de-evolves
        for (int i = 0; i < _timesUnused.Count; i++)
        {
            int timesUnused = _timesUnused[i];

            if (timesUnused >= _unusedLimit)
            {
                //Reset Times-Unused and De-Evolve Attack
                _timesUnused[i] = 0;
                DeEvolveAttack(i);
            }
        }
    }

    private void DeEvolveAttack(int attackIndex)
    {
        //Not the best way of doing this but it should work
        switch (attackIndex)
        {
            case 0:
                _attackSystem._lightHits -= _lightHitsIncrease;
                Debug.Log("Quick Attack Evolved: " + _attackSystem._lightHits);
                break;
            case 1:
                _attackSystem._heavyArea -= _heavyAreaIncrease;
                Debug.Log("Strong Attack Evolved: " + _attackSystem._heavyArea);
                break;
            case 2:
                _attackSystem._dashDamage -= _dashDamageIncrease;
                Debug.Log("Dash Attack Evolved: " + _attackSystem._dashDamage);
                break;
        }
    }
}
