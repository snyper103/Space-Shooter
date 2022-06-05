using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab, _tripleFirePrefab,_shieldPrefab, _explosionPrefab, _thrusterPrefab;
    [SerializeField] private GameObject[] _damage = new GameObject[2];
    private GameObject _shield;
    private spawnManager _spawnManager;
    private UImanager _UImg;
    private GameObject _playerThruster;
    private Animator _movementAnim;
    private float _speed = 4.0f, _fireRate = 0.15f, _powerUpTime = 5.0f, _nextFire = 0.0f;
    private int _score = 0, _hp = 3;
    private bool _tripleShotEnable = false, _shieldEnable = false;
    private Vector3 _movement = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.FindWithTag("Respawn").GetComponent<spawnManager>();
        _UImg = GameObject.FindWithTag("UI_Manager").GetComponent<UImanager>();
        _movementAnim = GetComponent<Animator>();

        LogError();

        _playerThruster = Instantiate(_thrusterPrefab, transform.position, Quaternion.identity);
        _playerThruster.transform.parent = transform;
        _playerThruster.transform.localScale = new Vector3(1, 1, 1);
    }
    void LogError()
    {
        if ( !_laserPrefab )
            Debug.LogError("player::_laserPrefab is NULL");

        if ( !_tripleFirePrefab )
            Debug.LogError("player::_tripleFirePrefab is NULL");

        if ( !_shieldPrefab )
            Debug.LogError("player::_shieldPrefab is NULL");
        
        if ( !_spawnManager )
            Debug.LogError("player::_spawnManager is NULL");

        if ( !_explosionPrefab )
            Debug.LogError("player::_explosionPrefab is NULL");

        if ( !_UImg )
            Debug.LogError("player::_UImg is NULL");

        if ( !_damage[0] )
            Debug.LogError("player::_damage[0] is NULL");

        if ( !_damage[1] )
            Debug.LogError("player::_damage[1] is NULL");

        if ( !_thrusterPrefab )
            Debug.LogError("player::_thrusterPrefab is NULL");

        if ( !_movementAnim )
            Debug.LogError("player::_movementAnim is NULL");
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();

        if ( Input.GetKeyDown(KeyCode.Space) && Time.time >= _nextFire )
            shootingLaser();
    }

    void calculateMovement()
    {
        _movement.x = Input.GetAxis("Horizontal");
        Vector2 speedX = new Vector2(_movement.x, 0);
        _movement.y = Input.GetAxis("Vertical");

        _movementAnim.SetFloat("Horizontal", _movement.x);
        _movementAnim.SetFloat("Speed", speedX.sqrMagnitude);
        transform.Translate(_movement*_speed*Time.deltaTime);

        if ( transform.position.x < -10.20f )
            transform.position = new Vector3(-10.20f, transform.position.y, 0);

        else if ( transform.position.x > 10.20f )
            transform.position = new Vector3(10.20f, transform.position.y, 0);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.9f, 0), 0);
    }

    void shootingLaser()
    {
        _nextFire = Time.time + _fireRate;

        if ( _tripleShotEnable )
        {
            GameObject newLaser = Instantiate(_tripleFirePrefab, new Vector3(transform.position.x-0.9265615f, transform.position.y+0.06076493f, transform.position.z), Quaternion.identity);

            for ( int i = 0; i < 3; ++i )
                newLaser.transform.GetChild(i).gameObject.GetComponent<laser>().playerIndex(0);
        }

        else
        {
            GameObject newLaser = Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y+1.0f, transform.position.z), Quaternion.identity);

            newLaser.GetComponent<laser>().playerIndex(0);
        }
    }

    void powerUpEnable(int powerUpID)
    {
        switch ( powerUpID )
        {
            case 0:
                _tripleShotEnable = true;
                break;
            case 1:
                _powerUpTime = 7.5f;
                _speed *= 2.0f;
                break;
            case 2:
                _powerUpTime = 10.0f;
                _shieldEnable = true;
                _shield = Instantiate(_shieldPrefab, transform.position, Quaternion.identity);
                _shield.transform.parent = transform;

                if ( !_shield.transform.parent )
                    Debug.LogError("player::_shield.transform.parent is NULL");
                break;
            default:
                break;
        }

        StartCoroutine(powerUpDisable(powerUpID));
    }
    IEnumerator powerUpDisable(int powerUpID)
    {
        yield return new WaitForSeconds(_powerUpTime);

        switch ( powerUpID )
        {
            case 0:
                _tripleShotEnable = false;
                break;
            case 1:
                _powerUpTime = 5.0f;
                _speed /= 2.0f;
                break;
            case 2:
                _powerUpTime = 5.0f;
                _shieldEnable = false;
                Destroy(_shield);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Enemy" )
        {   
            enemy myEnemy = other.GetComponent<enemy>();

            if ( !myEnemy )
                Debug.LogError("player::myEnemy is NULL");

            if ( _hp > 0 )
            {
                if ( !_shieldEnable )
                {
                    --_hp;
                    _UImg.updateLives(_hp, 0);

                    switch ( _hp )
                    {
                        case 1:
                            _damage[0].SetActive(false);
                            _damage[1].SetActive(true);
                            break;
                        case 2:
                            _damage[0].SetActive(true);
                            break;
                        default:
                            Destroy(_playerThruster);
                           
                            _spawnManager.onPlayersDeath(0);
                            _speed = 0.0f;
                            bestScore.updateBestScore(_score);
                           
                            Destroy(gameObject, 0.2f);
                            GameObject newExplosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                            Destroy(newExplosion, 2.38f);
                            break;  
                    }
                }

                else
                {
                    _powerUpTime = 5.0f;
                    _shieldEnable = false;
                    Destroy(_shield);
                    StopCoroutine(powerUpDisable(2));
                }

                myEnemy.activeDestruction(false, 0);
                Destroy(other.gameObject, 2.38f);
                _spawnManager.onPlayersKill();
            }
        }

        else
        {
            if ( other.tag == "PowerUp" )
            {
                powerUp myPower = other.GetComponent<powerUp>();

                if ( !myPower )
                    Debug.LogError("player::myPower is NULL");

                FindObjectOfType<AudioManager>().Play("powerUp");
                powerUpEnable(myPower.returnPowerUpID());
                Destroy(other.gameObject);
            }

            else
                if ( other.tag == "enemyLaser" )
                {
                    if ( _hp > 0 )
                    {
                        if ( !_shieldEnable )
                        {
                            --_hp;
                            _UImg.updateLives(_hp, 0);

                            switch ( _hp )
                            {
                                case 1:
                                    _damage[0].SetActive(false);
                                    _damage[1].SetActive(true);
                                    break;
                                case 2:
                                    _damage[0].SetActive(true);
                                    break;
                                default:
                                    Destroy(_playerThruster);

                                    _spawnManager.onPlayersDeath(0);
                                    _speed = 0.0f;
                                    bestScore.updateBestScore(_score);
                                   
                                    Destroy(gameObject, 0.2f);
                                    GameObject newExplosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                                    Destroy(newExplosion, 2.38f);
                                    break; 
                            }
                        }

                        else
                        {
                            _powerUpTime = 5.0f;
                            _shieldEnable = false;
                            Destroy(_shield);
                            StopCoroutine(powerUpDisable(2));
                        }
                    }

                    Destroy(other.gameObject);
                }
        }
    }

    public int returnUpdatedScore(int points)
    {
        _score += points;
        return _score;
    }

    public int returnScore()
    {
        return _score;
    }
}