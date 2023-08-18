using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackValueManager : MonoBehaviour
{
    private List<List<float>> _attackValues = new List<List<float>>();
    private List<List<float>> _attackIncreases = new List<List<float>>();
    private List<List<float>> _minimumAttackValues = new List<List<float>>();
    private AttackSystem _attackSystem;

    [TextArea]
    public string _aboutAttacks;

    [Header("Light Attack")]
    public int _lightAttackDamage;
    public float _lightAttackRange;
    public float _lightAttackSpeed;
    [Space(10)]
    public int _lightAttackDamageIncrease;
    public float _lightAttackRangeIncrease;
    public float _lightAttackSpeedIncrease;

    private List<float> _lightAttackValues = new List<float>();

    [Header("Heavy Attack")]
    public int _heavyAttackDamage;
    public float _heavyAttackRange;
    public float _heavyAttackSpeed;
    [Space(10)]
    public int _heavyAttackDamageIncrease;
    public float _heavyAttackRangeIncrease;
    public float _heavyAttackSpeedIncrease;

    [Header("Dash Attack")]
    public int _dashAttackDamage;
    public float _dashAttackRange;
    public float _dashAttackSpeed;
    [Space(10)]
    public int _dashAttackDamageIncrease;
    public float _dashAttackRangeIncrease;
    public float _dashAttackSpeedIncrease;
    // Start is called before the first frame update
    void Start()
    {
        //Fill the attack Values List
        InitialiseAttackIncreasesList();
        InitialiseAttackValues(_attackValues);
        InitialiseAttackValues(_minimumAttackValues);

        _attackSystem = GetComponent<AttackSystem>();
    }

    public void EvolveAttack(int attackIndex, int parameterIndex, bool evolves)
    {
        //We get the current attack parameter and the value it should change by
        List<float> currentAttackValues = _attackValues[attackIndex];
        float currentAttackParameter = currentAttackValues[parameterIndex];

        List<float> currentAttackIncreases = _attackIncreases[attackIndex];
        float currentAttackIncreaseValue = currentAttackIncreases[parameterIndex];

        float updatedAttackParameter = currentAttackParameter;

        if (evolves)
        {
            updatedAttackParameter += currentAttackIncreaseValue;
        }
        //Check if the updated value is valid 
        else if ((currentAttackParameter - currentAttackIncreaseValue) > _minimumAttackValues[attackIndex][parameterIndex])
        {
            updatedAttackParameter -= currentAttackIncreaseValue;
        }

        //Put the updated attack value into the list
        _attackValues[attackIndex][parameterIndex] = updatedAttackParameter;

        //Specifically for the Speed Parameter
        if (parameterIndex == 2)
        {
            if (evolves)
            {
                switch (attackIndex)
                {
                    case 0:
                        GetComponent<LightAttack>().ChangeAttackSpeed(_attackIncreases[0][2]);
                        break;
                    case 1:
                        GetComponent<HeavyAttack>().ChangeAttackSpeed(_attackIncreases[1][2]);
                        break;
                    case 2:
                        GetComponent<DashAttack>().ChangeAttackSpeed(_attackIncreases[2][2]);
                        break;
                }
            }
        }

        UpdateAttackValues();
        _attackSystem.UpdateAttackValues();
    }

    private void UpdateAttackValues()
    {
        //Update the public values of the attacks
        //Light Attack
        _lightAttackDamage = (int)_attackValues[0][0];
        _lightAttackRange = _attackValues[0][1];
        _lightAttackSpeed = _attackValues[0][2];

        //Heavy Attack
        _heavyAttackDamage = (int)_attackValues[1][0];
        _heavyAttackRange = _attackValues[1][1];
        _heavyAttackSpeed = _attackValues[1][2];

        //DashAttack
        _dashAttackDamage = (int)_attackValues[2][0];
        _dashAttackRange = _attackValues[2][1];
        _dashAttackSpeed = _attackValues[2][2];
    }

    private void InitialiseAttackValues(List<List<float>> currentList)
    {
        //This is going to be long and hardcoded
        //We update the list that holds the Values for each parameter of each attack
        List<float> lightAttackValues = new List<float>();
        lightAttackValues.Add(_lightAttackDamage);
        lightAttackValues.Add(_lightAttackRange);
        lightAttackValues.Add(_lightAttackSpeed);

        List<float> heavyAttackValues = new List<float>();
        heavyAttackValues.Add(_heavyAttackDamage);
        heavyAttackValues.Add(_heavyAttackRange);
        heavyAttackValues.Add(_heavyAttackSpeed);

        List<float> dashAttackValues = new List<float>();
        dashAttackValues.Add( _dashAttackDamage);
        dashAttackValues.Add(_dashAttackRange);
        dashAttackValues.Add(_dashAttackSpeed);

        currentList.Add(lightAttackValues);
        currentList.Add(heavyAttackValues);
        currentList.Add(dashAttackValues);
    }

    private void InitialiseAttackIncreasesList()
    {
        //We fill the list that holds the Increase Values for each parameter of each attack
        List<float> lightAttackIncreases = new List<float>();
        lightAttackIncreases.Add(_lightAttackDamageIncrease);
        lightAttackIncreases.Add(_lightAttackRangeIncrease);
        lightAttackIncreases.Add(_lightAttackSpeedIncrease);

        List<float> heavyAttackIncreases = new List<float>();
        heavyAttackIncreases.Add(_heavyAttackDamageIncrease);
        heavyAttackIncreases.Add(_heavyAttackRangeIncrease);
        heavyAttackIncreases.Add(_heavyAttackSpeedIncrease);

        List<float> dashAttackIncreases = new List<float>();
        dashAttackIncreases.Add(_dashAttackDamageIncrease);
        dashAttackIncreases.Add(_dashAttackRangeIncrease);
        dashAttackIncreases.Add(_dashAttackSpeedIncrease);

        _attackIncreases.Add(lightAttackIncreases);
        _attackIncreases.Add(heavyAttackIncreases);
        _attackIncreases.Add(dashAttackIncreases);
    }
}
