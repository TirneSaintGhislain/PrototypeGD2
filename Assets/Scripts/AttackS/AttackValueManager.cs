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

    // Update is called once per frame
    void Update()
    {

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
        lightAttackValues[0] = _lightAttackDamage;
        lightAttackValues[1] = _lightAttackRange;
        lightAttackValues[2] = _lightAttackSpeed;

        List<float> heavyAttackValues = new List<float>();
        heavyAttackValues[0] = _heavyAttackDamage;
        heavyAttackValues[1] = _heavyAttackRange;
        heavyAttackValues[2] = _heavyAttackSpeed;

        List<float> dashAttackValues = new List<float>();
        dashAttackValues[0] = _dashAttackDamage;
        dashAttackValues[1] = _dashAttackRange;
        dashAttackValues[2] = _dashAttackSpeed;

        currentList.Add(lightAttackValues);
        currentList.Add(heavyAttackValues);
        currentList.Add(dashAttackValues);
    }

    private void InitialiseAttackIncreasesList()
    {
        //We fill the list that holds the Increase Values for each parameter of each attack
        List<float> lightAttackIncreases = new List<float>();
        lightAttackIncreases[0] = _lightAttackDamageIncrease;
        lightAttackIncreases[1] = _lightAttackRangeIncrease;
        lightAttackIncreases[2] = _lightAttackSpeedIncrease;

        List<float> heavyAttackIncreases = new List<float>();
        heavyAttackIncreases[0] = _heavyAttackDamageIncrease;
        heavyAttackIncreases[1] = _heavyAttackRangeIncrease;
        heavyAttackIncreases[2] = _heavyAttackSpeedIncrease;

        List<float> dashAttackIncreases = new List<float>();
        dashAttackIncreases[0] = _dashAttackDamageIncrease;
        dashAttackIncreases[1] = _dashAttackRangeIncrease;
        dashAttackIncreases[2] = _dashAttackSpeedIncrease;

        _attackIncreases.Add(lightAttackIncreases);
        _attackIncreases.Add(heavyAttackIncreases);
        _attackIncreases.Add(dashAttackIncreases);
    }
}
