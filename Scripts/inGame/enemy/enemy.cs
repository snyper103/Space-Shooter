using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField] private GameObject _thrusterPrefab, _laserPrefab;
    private GameObject _enemyThruster;
    private float _speed, _fireRate, _nextFire, _minFireRate, _maxFireRate, _minNextFire, _maxNextFire;
    private int _enemyPoints;
    private Animator _deathAnim;
    private bool _isDestroyed = false;
    private UImanager _UItext;
    private player _player;
    private player2 _player2;
    private spawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _deathAnim = gameObject.GetComponent<Animator>();
        _UItext = GameObject.FindWithTag("UI_Manager").GetComponent<UImanager>();
        _spawnManager = GameObject.FindWithTag("Respawn").GetComponent<spawnManager>();

        searchPlayer();

        LogError();

        _enemyThruster = Instantiate(_thrusterPrefab, transform.position, Quaternion.identity);
        _enemyThruster.transform.parent = transform;

        changeDifficulty();

        _fireRate = Random.Range(_minFireRate, _maxFireRate);
        _nextFire = Random.Range(_minNextFire, _maxNextFire);
    }
    void LogError()
    {
        if ( !_deathAnim )
            Debug.LogError("enemy::_deathAnim is NULL");

        if ( !_thrusterPrefab )
            Debug.LogError("enemy::_thrusterPrefab is NULL");

        if ( !_laserPrefab )
            Debug.LogError("enemy::_laserPrefab is NULL");

        if ( !_UItext )
            Debug.LogError("enemy::_UItext is NULL");

        if ( !_spawnManager )
            Debug.LogError("enemy::_spawnManager is NULL");
    }
    void changeDifficulty()
    {
        switch ( difficultyValues.difficulty )
        {
            case difficultyValues.Difficulties.Easy:
                _speed = 5.0f;

                _minFireRate = 1.0f;
                _maxFireRate = 3.0f;
                
                _minNextFire = 0.0f;
                _maxNextFire = 3.0f;
                
                _enemyPoints = 10;

                if ( difficultyValues.isCoopMode )
                    _enemyPoints /= 2;
                break;

            case difficultyValues.Difficulties.Medium:
                _speed = 7.5f;

                _minFireRate = 1.0f;
                _maxFireRate = 1.5f;
                
                _minNextFire = 0.0f;
                _maxNextFire = 2.0f;
                
                _enemyPoints = 15;

                if ( difficultyValues.isCoopMode )
                    _enemyPoints /= 2;
                break;

            case difficultyValues.Difficulties.Hard:
                _speed = 10.0f;

                _minFireRate = 0.5f;
                _maxFireRate = 1.5f;
                
                _minNextFire = 0.0f;
                _maxNextFire = 1.5f;
                
                _enemyPoints = 50;

                if ( difficultyValues.isCoopMode )
                    _enemyPoints /= 2;
                break;
        }
    }
    void searchPlayer()
    {
        if ( difficultyValues.isCoopMode )
        {
            if ( _spawnManager.isPlayerAlive() && _spawnManager.isPlayer2Alive() )
            {
                _player = GameObject.FindWithTag("Player").GetComponent<player>();
                _player2 = GameObject.FindWithTag("Player2").GetComponent<player2>();

                if ( !_player )
                    Debug.LogError("enemy::_player is NULL");
                if ( !_player2 )
                    Debug.LogError("enemy::_player2 is NULL");
            }

            else
            {
                if ( _spawnManager.isPlayerAlive() )
                {
                    _player = GameObject.FindWithTag("Player").GetComponent<player>();

                    if ( !_player )
                        Debug.LogError("enemy::_player is NULL");
                }

                else
                {
                    _player2 = GameObject.FindWithTag("Player2").GetComponent<player2>();

                    if ( !_player2 )
                        Debug.LogError("enemy::_player2 is NULL");
                }
            }
        }
        else
        {
            _player = GameObject.FindWithTag("Player").GetComponent<player>();

            if ( !_player )
                Debug.LogError("enemy::_player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();

        if ( Time.time >= _nextFire && !_isDestroyed )
            shootingLaser();
    }

    void calculateMovement()
    {
        transform.Translate(0, -_speed*Time.deltaTime, 0);

        if ( transform.position.y < -5.4f )
            transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0);
    }

    void shootingLaser()
    {
        _nextFire = Time.time + _fireRate;

        Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y-0.7f, transform.position.z), Quaternion.identity);

        _fireRate = Random.Range(_minFireRate, _maxFireRate);
    }

    public void activeDestruction(bool gotShooted, int index)
    {
        FindObjectOfType<AudioManager>().Play("explosion");

        if ( gotShooted )
            switch ( index )
            {
                case 0:
                    _UItext.updateScore(_player.returnUpdatedScore(_enemyPoints), index);
                    break;
                case 1:
                    _UItext.updateScore(_player2.returnUpdatedScore(_enemyPoints), index);
                    break;
            }
        
        Destroy(GetComponent<Collider2D>());
        Destroy(_enemyThruster);
        
        _speed = 0.0f;
        _isDestroyed = true;
        
        _deathAnim.SetTrigger("onEnemyDeath");
    }
}
