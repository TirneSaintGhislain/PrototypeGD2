using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class EvolutionPicker : MonoBehaviour
{
    private TextMeshProUGUI _thisPlayerText;

    [HideInInspector]
    public int _chosenEvolutionIndex = 0;

    [HideInInspector]
    public string _chosenEvolution = "Damage";
    public void DPadLeft(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _chosenEvolutionIndex = 0;
            _chosenEvolution = "Damage";
            UpdateText();
        }
    }
    public void DPadUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _chosenEvolutionIndex = 1;
            _chosenEvolution = "Range";
            UpdateText();
        }
    }
    public void DPadRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _chosenEvolutionIndex = 2;
            _chosenEvolution = "Speed";
            UpdateText();
        }
    }

    private void UpdateText()
    {
        _thisPlayerText.text = _chosenEvolution;
    }

    private void Start()
    {
        if (GetComponent<MultiplayerManager>()._playerIndex == 0)
        {
            _thisPlayerText = GameObject.FindGameObjectWithTag("1").GetComponent<TextMeshProUGUI>();
        }
        else if (GetComponent<MultiplayerManager>()._playerIndex == 1)
        {
            _thisPlayerText = GameObject.FindGameObjectWithTag("2").GetComponent<TextMeshProUGUI>();
        }
        Debug.Log(GetComponent<MultiplayerManager>()._playerIndex);
        UpdateText();
    }
}
