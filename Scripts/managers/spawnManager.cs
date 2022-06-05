using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab, _enemyContainer, _asteroidPrefab;
    [SerializeField] private GameObject[] _powerUpPrefab;
    private UImanager _UItext;
    private float _spawnTime, _PowerUpSpawnTime, _powerUpMinTime, _powerUpMaxTime;
    private int _maxNumEnemy, _cont = 0;
    private bool _stopSpawning = false, _isGameOver = false, _isPlayerDead = false, _isPlayer2Dead = false;

    // Start is called before the first frame update
    void Start()
    {
        _UItext = GameObject.FindWithTag("UI_Manager").GetComponent<UImanager>();
        _enemyContainer =  GameObject.FindWithTag("EnemyContainer");

        logError();

        Instantiate(_asteroidPrefab, new Vector3(0, 4, 0), Quaternion.identity);

        changeDifficulty();

        _PowerUpSpawnTime = Random.Range(_powerUpMinTime/2.0f, _powerUpMaxTime/2.0f);
    }
    void logError()
    {
        if ( !_enemyPrefab )
            Debug.LogError("spawnManager::_enemyPrefab is NULL");

        if ( !_enemyContainer )
            Debug.LogError("spawnManager::_enemyContainer is NULL");

        if ( !_asteroidPrefab )
            Debug.LogError("spawnManager::_asteroidPrefab is NULL");

        for ( int i = 0; i < 3; ++i )
            if ( !_powerUpPrefab[i] )
                Debug.LogError("spawnManager::_powerUpPrefab is NULL on instance " + i);

        if ( !_UItext )
            Debug.LogError("spawnManager::_UItext is NULL");
    }
    void changeDifficulty()
    {
        switch ( difficultyValues.difficulty )
        {
            case difficultyValues.Difficulties.Easy:
                _spawnTime = 5.0f;
                _powerUpMinTime = 10.0f;
                _powerUpMaxTime = 20.0f;
                _maxNumEnemy = 5;

                if ( difficultyValues.isCoopMode )
                {
                    _spawnTime /= 2.0f;
                    _maxNumEnemy *= 2;
                }
                break;

            case difficultyValues.Difficulties.Medium:
                _spawnTime = 4.0f;
                _powerUpMinTime = 10.0f;
                _powerUpMaxTime = 25.0f;
                _maxNumEnemy = 10;

                if ( difficultyValues.isCoopMode )
                {
                    _spawnTime /= 2.0f;
                    _maxNumEnemy *= 2;
                }
                break;

            case difficultyValues.Difficulties.Hard:
                _spawnTime = 3.0f;
                _powerUpMinTime = 20.0f;
                _powerUpMaxTime = 40.0f;
                _maxNumEnemy = 25;

                if ( difficultyValues.isCoopMode )
                {
                    _spawnTime /= 2.0f;
                    _maxNumEnemy *= 2;
                }
                break;
        }
    }

    public void startSpawning()
    {
        StartCoroutine(spawnEnemy());
        StartCoroutine(spawnPowerUp());
    }

    IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(2.38f/2.0f);

        while ( !_stopSpawning )
        {
            if ( _cont < _maxNumEnemy )
            {
                ++_cont;
                GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }

            yield return new WaitForSeconds(_spawnTime);
        }
    }
    IEnumerator spawnPowerUp()
    {
        yield return new WaitForSeconds(2.38f/2.0f);

        while ( !_stopSpawning )
        {
            yield return new WaitForSeconds(_PowerUpSpawnTime);

            _PowerUpSpawnTime = Random.Range(_powerUpMinTime, _powerUpMaxTime);
            Instantiate(_powerUpPrefab[Random.Range(0, 3)], new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0), Quaternion.identity);

            if ( difficultyValues.isCoopMode )
            {
                yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
                Instantiate(_powerUpPrefab[Random.Range(0, 3)], new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0), Quaternion.identity);
            }
        }
    }

    IEnumerator flickeringGameOver()
    {
        while ( _stopSpawning )
        {
            _UItext.setOnGameOver(true);
            yield return new WaitForSeconds(1.0f);

            _UItext.setOnGameOver(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void onPlayersDeath(int index)
    {
        switch ( index )
        {
            case 0:
                _isPlayerDead = true;
                break;
            case 1:
                _isPlayer2Dead = true;
                break;
        }

        if ( difficultyValues.isCoopMode )
        {
            if ( _isPlayerDead && _isPlayer2Dead )
            {
                _stopSpawning = true;
                _isGameOver = true;
                _UItext.setOnRestart(true);
                _UItext.setOnGetBack(true);
                StartCoroutine(flickeringGameOver());
            }
        }

        else
        {
            _stopSpawning = true;
            _isGameOver = true;
            _UItext.setOnRestart(true);
            _UItext.setOnGetBack(true);
            StartCoroutine(flickeringGameOver());
        }
    }
    public void onPlayersKill()
    {
        --_cont;
    }

    public bool isGameOver()
    {
        return _isGameOver;
    }
    public bool isPlayerAlive()
    {
        return !_isPlayerDead;
    }
    public bool isPlayer2Alive()
    {
        return !_isPlayer2Dead;
    }
}
