using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int healthMax = 10;
    public int currentHealth;
    [SerializeField]
    private float _invincibilityTime = 0.3f;

    private Transform _opponent;
    private bool _foundOpponent;

    private bool _canGetHit = true;
    private int _currentHealth;
    private int _playerIndex;

    private HealthSystemUI _healthSystemUI;
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = healthMax;
        _healthSystemUI = GameObject.FindGameObjectWithTag("HealthManager").GetComponent<HealthSystemUI>();
        _healthSystemUI.FindPlayerHealthSystem();
        _playerIndex = GetComponent<MultiplayerManager>()._currentColorIndex;
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

            //if (_currentHealth < 1)
            //{
            //    StartCoroutine(Die());
            //}

            KnockBack(knockBackStrength);

            //Starts the Invincibility Timer
            //StartCoroutine(CountDownInvincibilityTimer());

            Debug.Log(name + " got hit, current health: " + _currentHealth);

            //Make the character get Stunned
            StartCoroutine(GetComponent<HitstunSystem>().StartHitStunTime(stunTime));

            GetComponent<AudioSystem>().PlayHurtSound();
            UpdateScore(damage);
        }
    }
   
    private void KnockBack(float knockBackStrength)
    {
        Vector3 direction = transform.position - _opponent.position;
        Vector3 knockBackForce = direction * knockBackStrength;
        GetComponent<Rigidbody>().AddForce(knockBackForce);
    }

    private IEnumerator Die()
    {
        Debug.Log(name + " loses");
        Destroy(gameObject);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator CountDownInvincibilityTimer()
    {
        _canGetHit = false;
        yield return new WaitForSeconds(_invincibilityTime);
        _canGetHit = true;
    }

    private void UpdateScore(int damage)
    {
        //ExampleFunction(damage, _playerIndex)
    }
}
