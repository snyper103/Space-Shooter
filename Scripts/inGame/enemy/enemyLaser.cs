using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLaser : MonoBehaviour
{
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        changeDifficulty();
    }
    void changeDifficulty()
    {
        switch ( difficultyValues.difficulty )
        {
            case difficultyValues.Difficulties.Easy:
                _speed = 7.0f;
                break;

            case difficultyValues.Difficulties.Medium:
                _speed = 11.0f;
                break;

            case difficultyValues.Difficulties.Hard:
                _speed = 13.0f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
    }

    void calculateMovement()
    {
        transform.Translate(0, -_speed*Time.deltaTime, 0);

        if ( transform.position.y <= -8.0f )
            destroyLaser();
    }

    void destroyLaser()
    {
        Destroy(gameObject);
    }
}
