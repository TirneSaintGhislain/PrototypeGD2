using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private int _healthMax = 10;
    [SerializeField]
    private float _invincibilityTime = 0.3f;

    private bool _canGetHit = true;
    private int _currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit(int damage)
    {
        if (_canGetHit)
        {
            _currentHealth -= damage;

            if (_currentHealth < 1)
            {
                Die();
            }

            //Starts the Invincibility Timer
            StartCoroutine(CountDownInvincibilityTimer());

            Debug.Log(name + " got hit, current health: " + _currentHealth);
        }
    }

    private void Die()
    {
        Debug.Log(name + " loses");
    }

    private IEnumerator CountDownInvincibilityTimer()
    {
        _canGetHit = false;
        yield return new WaitForSeconds(_invincibilityTime);
        _canGetHit = true;
    }
}
