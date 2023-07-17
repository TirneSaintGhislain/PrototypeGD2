using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionSystem : MonoBehaviour
{   
    [SerializeField]
    private int _comboGoal;

    [SerializeField]
    private float _damageIncrease;

    [SerializeField]
    private List<float> _attackDamageValues = new List<float>();

    private int _currentCombo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SuccesfulHit(int attackIndex)
    {
        
    }
}
