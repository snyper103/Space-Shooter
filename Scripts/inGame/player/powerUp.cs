using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int powerUpID;

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
    }

    void calculateMovement()
    {
        transform.Translate(0, -_speed*Time.deltaTime, 0);

        if ( transform.position.y < -5.4f )
            Destroy(gameObject);
    }
    public int returnPowerUpID()
    {
        return powerUpID;
    }
}
