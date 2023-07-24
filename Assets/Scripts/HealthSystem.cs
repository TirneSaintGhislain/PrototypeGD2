using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private int _healthMax = 10;
    [SerializeField]
    private float _invincibilityTime = 0.3f;

    private Transform _opponent;
    private bool _foundOpponent;

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
        if (!_foundOpponent)
        {
            FindOpponent();
        }
    }

    private void FindOpponent()
    {
        List<GameObject> allPlayers = new List<GameObject>();
        allPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        //Debug.Log(allPlayers.Count);
        if (allPlayers.Count > 1)
        {
            foreach (var player in allPlayers)
            {
                if (player != gameObject)
                {
                    _opponent = player.transform;
                    _foundOpponent = true;
                    Debug.Log(_opponent.position);
                }
            }
        }
    }

    public void GetHit(int damage, float stunTime, float knockBackStrength)
    {
        bool isStunned = GetComponent<HitstunSystem>()._isStunned;
        if (_canGetHit && !isStunned)
        {
            _currentHealth -= damage;

            if (_currentHealth < 1)
            {
                Die();
            }

            KnockBack(knockBackStrength);

            //Starts the Invincibility Timer
            //StartCoroutine(CountDownInvincibilityTimer());

            Debug.Log(name + " got hit, current health: " + _currentHealth);

            //Make the character get Stunned
            StartCoroutine(GetComponent<HitstunSystem>().StartHitStunTime(stunTime));

            GetComponent<AudioSystem>().PlayHurtSound();
        }
    }

    private void KnockBack(float knockBackStrength)
    {
        Vector3 direction = transform.position - _opponent.position;
        Vector3 knockBackForce = direction * knockBackStrength;
        GetComponent<Rigidbody>().AddForce(knockBackForce);
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
