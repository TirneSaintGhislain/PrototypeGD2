using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionUI : MonoBehaviour
{
    private GameObject _evolutionPanel;
    [SerializeField]
    private Text _lightAttackText;
    [SerializeField]
    private Text _heavyAttackText;
    [SerializeField]
    private Text _dashAttackText;

    private int _lightAttackCounter;
    private int _heavyAttackCounter;
    private int _dashAttackCounter;

    [SerializeField]
    private Text _lightAttackInfo;
    [SerializeField]
    private Text _heavyAttackInfo;
    [SerializeField]
    private Text _dashAttackInfo;

    public void UpdateLightAttack(bool positive)
    {
        if(positive)
        {
            _lightAttackCounter += 2;
            if(_lightAttackCounter < 5)
            {
                _lightAttackText.text = "" + _lightAttackCounter;
            }
            else
            {
                StartCoroutine(LightAttackEffect(true));
                _lightAttackCounter = 0;
            }
        }
        else
        {
            _lightAttackCounter--;
            if(_lightAttackCounter > -5)
            {
                _lightAttackText.text = "" + _lightAttackCounter;
            }
            else
            {
                StartCoroutine(LightAttackEffect(false));
                _lightAttackCounter = 0;
            }
        }
    }

    private IEnumerator LightAttackEffect(bool positive)
    {
        _lightAttackInfo.enabled = true;
        if(positive)
        {
            _lightAttackInfo.color = Color.yellow;
            _lightAttackInfo.text = "Light Attack Evolved!";
        }
        else
        {
            _lightAttackInfo.color = Color.red;
            _lightAttackInfo.text = "Light Attack De-evolved!";
        }
        yield return new WaitForSeconds(0.4f);
        _lightAttackInfo.enabled = false;
    }

    public void UpdateHeavyAttack(bool positive)
    {
        if (positive)
        {
            _heavyAttackCounter += 2;
            if (_heavyAttackCounter < 5)
            {
                _heavyAttackText.text = "" + _heavyAttackCounter;
            }
            else
            {
                StartCoroutine(HeavyAttackEffect(true));
                _heavyAttackCounter = 0;
            }
        }
        else
        {
            _heavyAttackCounter--;
            if (_heavyAttackCounter > -5)
            {
                _heavyAttackText.text = "" + _heavyAttackCounter;
            }
            else
            {
                StartCoroutine(HeavyAttackEffect(false));
                _heavyAttackCounter = 0;
            }
        }
    }

    private IEnumerator HeavyAttackEffect(bool positive)
    {
        _heavyAttackInfo.enabled = true;
        if (positive)
        {
            _heavyAttackInfo.color = Color.yellow;
            _heavyAttackInfo.text = "Heavy Attack Evolved!";
        }
        else
        {
            _heavyAttackInfo.color = Color.red;
            _heavyAttackInfo.text = "Heavy Attack De-evolved!";
        }
        yield return new WaitForSeconds(0.4f);
        _heavyAttackInfo.enabled = false;
    }

    public void UpdateDashAttack(bool positive)
    {
        if (positive)
        {
            _dashAttackCounter += 2;
            if (_dashAttackCounter < 5)
            {
                _dashAttackText.text = "" + _dashAttackCounter;
            }
            else
            {
                StartCoroutine(HeavyAttackEffect(true));
                _dashAttackCounter = 0;
            }
        }
        else
        {
            _dashAttackCounter--;
            if (_dashAttackCounter > -5)
            {
                _dashAttackText.text = "" + _dashAttackCounter;
            }
            else
            {
                StartCoroutine(HeavyAttackEffect(false));
                _dashAttackCounter = 0;
            }
        }
    }

    private IEnumerator DashAttackEffect(bool positive)
    {
        _dashAttackInfo.enabled = true;
        if (positive)
        {
            _dashAttackInfo.color = Color.yellow;
            _dashAttackInfo.text = "Dash Attack Evolved!";
        }
        else
        {
            _dashAttackInfo.color = Color.red;
            _dashAttackInfo.text = "Dash Attack De-evolved!";
        }
        yield return new WaitForSeconds(0.4f);
        _dashAttackInfo.enabled = false;
    }

}
