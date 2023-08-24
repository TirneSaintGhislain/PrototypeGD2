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

    // Reference to the MultiplayerManager
    private MultiplayerManager _multiplayerManager;

    private void Awake()
    {
        // Get the MultiplayerManager component
        _multiplayerManager = GetComponent<MultiplayerManager>();
    }

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
        if (_thisPlayerText != null)
        {
            _thisPlayerText.text = _chosenEvolution;
        }
        else
        {
            Debug.LogWarning("Text component is not assigned for EvolutionPicker.");
        }
    }

    private void Start()
    {
        if (_multiplayerManager._playerIndex == 0)
        {
            _thisPlayerText = GameObject.FindGameObjectWithTag("1").GetComponent<TextMeshProUGUI>();
        }
        else if (_multiplayerManager._playerIndex == 1)
        {
            _thisPlayerText = GameObject.FindGameObjectWithTag("2").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("Player index out of bounds for EvolutionPicker.");
        }

        Debug.Log(_multiplayerManager._playerIndex);
        UpdateText();
    }
}
