using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroid : MonoBehaviour
{
    private float _speed = 10.0f;
    private bool _isAsteroidDestroyed = false, _canPassThrough = false;

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
    }

    void calculateMovement()
    {
        transform.Rotate(0, 0, _speed*Time.deltaTime);
    }

    public IEnumerator asteroidDestruction()
    {
        _speed = 0.0f;
        _isAsteroidDestroyed = true;

        yield return new WaitForSeconds(0.2f);

        _canPassThrough = true;
    }

    public bool isAsteroidDestroyed()
    {
        return _isAsteroidDestroyed;
    }
    public bool canPassThrough()
    {
        return _canPassThrough;
    }
}
