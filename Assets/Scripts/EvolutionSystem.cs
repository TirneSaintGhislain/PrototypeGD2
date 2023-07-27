using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionSystem : MonoBehaviour
{
    [Header("General Parameters")]

    [SerializeField] private bool _usingMethod1;
    [SerializeField] private int _amountOfAttacks = 3;

    private AttackSystem _attackSystem;

    //ideally this is temporary until we can revamp the Attack System
    //This float is used to ensure we don't register the same Attack multiple times
    [SerializeField]
    private float _attackCoolDown;
    private bool canAttack = true;

    [SerializeField]
    private int _lightHitsIncrease;
    [SerializeField]
    private float _heavyAreaIncrease;
    [SerializeField]
    private float _dashDamageIncrease;
    [SerializeField]
    private int _lightHitsMinimum;
    [SerializeField]
    private float _heavyAreaMininum;
    [SerializeField]
    private float _dashDamageMinimum;

    [Header("Method 1")]
    [SerializeField]
    private int _attackRatingIncrease = 2;
    [SerializeField]
    private int _attackRatingDecrease = 1;
    [SerializeField]
    private int _attackEvolveThreshold = 5;
    [SerializeField]
    private int _attackDevolveThreshold = -5;
    private List<int> _attackRatings = new List<int>();

    [Header("Method 2")]
    [SerializeField]
    private int _comboGoal;
    [SerializeField]
    private int _unusedLimit;
    private int _currentCombo;
    private List<int> _timesUnused = new List<int>();

    //The combocooldown resets a combo when it doesn't get continued
    [SerializeField]
    private float _comboCoolDown;
    private float _comboTimer;


    // Start is called before the first frame update
    void Start()
    {
        _attackSystem = GetComponent<AttackSystem>();
        _attackSystem.LightHits = _lightHitsMinimum;

        //Do this in attacksystem
        GetComponent<HeavyAttack>().Radius = _heavyAreaMininum;
        GetComponent<AttackSystem>().DashDamage = _dashDamageMinimum;
        //Initialise the Lists
        for (int i = 0; i < _amountOfAttacks; i++)
        {
            _timesUnused.Add(0);
            _attackRatings.Add(0);
        }
    }

    private void Update()
    {
        CountDownComboCoolDown();
        //Debug.Log(_timesUnused[1]);
    }

    public void SuccesfulHit(int attackIndex, bool canEvolve)
    {
        if (canAttack && canEvolve)
        {
            if (_usingMethod1)
            {
                UpdateAttackMethod1(attackIndex);
            }
            else
            {
                UpdateAttackMethod2(attackIndex);
            }
            StartCoroutine(CountDownAttackCoolDown());
        }
    }

    private void UpdateAttackMethod2(int attackIndex)
    {
        //Restart Combo Timer
        _comboTimer = 0;

        //Increase combo length
        //Evolve attack when combo goal is reached, then reset combo counter
        _currentCombo++;
        if (_currentCombo >= _comboGoal)
        {
            EvolveAttack(attackIndex);
            _currentCombo = 0;
        }

        //Goes over every attack and updates the Unused Amount per attack
        for (int i = 0; i < _timesUnused.Count; i++)
        {
            //If the attack was used it gets reset to 0, otherwise 1 gets added
            if (i == attackIndex)
            {
                _timesUnused[attackIndex] = 0;
            }
            else if (i != attackIndex)
            {
                _timesUnused[i] += 1;
            }
        }

        CheckIfAttacksNeedToDeEvolve();
    }

    private void UpdateAttackMethod1(int attackIndex)
    {
        for (int i = 0; i < _amountOfAttacks; i++)
        {
            if (i == attackIndex)
            {
                _attackRatings[i] += _attackRatingIncrease;
            }
            else
            {
                _attackRatings[i] -= _attackRatingDecrease;
            }
        }

        //If the ratings reach the thresholds they evolve or devolve
        ApplyRatings();
    }

    private void ApplyRatings()
    {
        for (int i = 0; i < _attackRatings.Count; i++)
        {
            //Evolve the attack if it reaches or exceeds the Evolving Threshold
            if (_attackRatings[i] >= _attackEvolveThreshold)
            {
                EvolveAttack(i);
                _attackRatings[i] = 0;
            }

            //Devolve the attack if it reaches or exceeds the Devolving Threshold
            if (_attackRatings[i] <= _attackDevolveThreshold)
            {
                DeEvolveAttack(i);
                _attackRatings[i] = 0;
            }
        }
    }

    private void EvolveAttack(int attackIndex)
    {
        //Evolve the correct attack
        switch (attackIndex)
        {
            case 0:
                _attackSystem.LightHits += _lightHitsIncrease;
                Debug.Log("Quick Attack Evolved: " + _attackSystem.LightHits);
                break;
            case 1:
                _attackSystem.HeavyArea += _heavyAreaIncrease;
                Debug.Log("Strong Attack Evolved: " + _attackSystem.HeavyArea);
                break;
            case 2:
                _attackSystem.DashDamage += _dashDamageIncrease;
                Debug.Log("Dash Attack Evolved: " + _attackSystem.DashDamage);
                break;
        }

        GetComponent<AudioSystem>().PlayEvolvedSound();
    }

    private void CheckIfAttacksNeedToDeEvolve()
    {
        //If an attack gets unused too much it de-evolves
        for (int i = 0; i < _timesUnused.Count; i++)
        {
            if (_timesUnused[i] >= _unusedLimit)
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
            //Only decrease the values if it stays above or at the minimum
            case 0:
                if (_attackSystem.LightHits - _lightHitsIncrease > _lightHitsMinimum)
                    _attackSystem.LightHits -= _lightHitsIncrease;
                Debug.Log("Quick Attack De-Evolved: " + _attackSystem.LightHits);
                break;
            case 1:
                if (_attackSystem.HeavyArea - _heavyAreaIncrease > _heavyAreaMininum)
                    _attackSystem.HeavyArea -= _heavyAreaIncrease;
                Debug.Log("Strong Attack De-Evolved: " + _attackSystem.HeavyArea);
                break;
            case 2:
                if (_attackSystem.DashDamage - _dashDamageIncrease > _dashDamageMinimum)
                    _attackSystem.DashDamage -= _dashDamageIncrease;
                Debug.Log("Dash Attack De-Evolved: " + _attackSystem.DashDamage);
                break;
        }
    }

    private IEnumerator CountDownAttackCoolDown()
    {
        //This coroutine makes it so we don't log multiple attacks in one
        canAttack = false;
        yield return new WaitForSeconds(_attackCoolDown);
        canAttack = true;
    }

    private void CountDownComboCoolDown()
    {
        //This timer resets the combo when it doesn't get continued
        if (_comboTimer < _comboCoolDown)
            _comboTimer += Time.deltaTime;

        if (_comboTimer >= _comboCoolDown)
        {
            _currentCombo = 0;
        }
        //Debug.Log(_comboTimer + " " + _currentCombo);
    }
}
