using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    private float _speed = 10.0f;
    private spawnManager _spawnManager;
    private asteroid _asteroid;
    private int _index;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.FindWithTag("Respawn").GetComponent<spawnManager>();

        logError();
    }
    void logError()
    {
        if ( !_spawnManager )
            Debug.LogError("laser::_spawnManager is NULL");

        if ( !_explosionPrefab )
            Debug.LogError("laser::_explosionPrefab is NULL");
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
    }

    void calculateMovement()
    {
        transform.Translate(0, _speed*Time.deltaTime, 0);

        if ( transform.position.y >= 8.0f )
            destroyLaser();
    }

    void destroyLaser()
    {
        if ( transform.parent != null )
            Destroy(transform.parent.gameObject);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Enemy" )
        {
            enemy myEnemy = other.GetComponent<enemy>();

            if ( !myEnemy )
                Debug.LogError("laser::myEnemy is NULL");
                
            _spawnManager.onPlayersKill();

            myEnemy.activeDestruction(true, _index);
            Destroy(other.gameObject, 2.38f);
            destroyLaser();
        }

        else
        {
            if ( other.tag == "Asteroid" )
            {
                _asteroid = other.gameObject.GetComponent<asteroid>();

                if ( !_asteroid )
                    Debug.LogError("laser::_asteroid is NULL");

                if ( !_asteroid.isAsteroidDestroyed() )
                {
                    StartCoroutine(_asteroid.asteroidDestruction());
                    
                    Destroy(other.gameObject, 0.2f);
                    GameObject newExplosion = Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);
                    Destroy(newExplosion, 2.38f);

                    _spawnManager.startSpawning();
                }

                if ( !_asteroid.canPassThrough() )
                    Destroy(gameObject);
            }

            else
            {
                if ( other.tag == "enemyLaser" )
                {
                    Destroy(gameObject);
                    Destroy(other.gameObject);
                }

                else
                {
                    if ( other.tag == "Player" && _index == 1 )
                        Destroy(gameObject);

                    else
                        if ( other.tag == "Player2" && _index == 0 )
                            Destroy(gameObject);
                }
            }
        }
    }

    public void playerIndex(int i)
    {
        _index = i;
    }
}
