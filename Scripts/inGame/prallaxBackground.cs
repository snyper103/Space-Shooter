using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    private Transform _cameraTransform;
    private Vector3 _backgroundPosition;
    private Sprite _sprite;
    private Texture2D _texture;
    private float _parallaxCoefficientX = 0.05f, _parallaxCoefficientY = 0.1f, _textureUnitSize, _speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite;
        _texture = _sprite.texture;

        LogError();

        _cameraTransform = _playerPrefab.transform;
        _backgroundPosition = _cameraTransform.position;
        _textureUnitSize = _texture.height/_sprite.pixelsPerUnit;
    }
    void LogError()
    {
        if ( !_playerPrefab )
            Debug.LogError("prallaxBackground::_playerPrefab is NULL");

        if ( !_sprite )
            Debug.LogError("prallaxBackground::_sprite is NULL");

        if ( !_texture )
            Debug.LogError("prallaxBackground::_texture is NULL");
    }
    // Update is called once per frame
    void Update()
    {
        calculateMovement();
    }

    // Update is called once per frame afeter Update
    void LateUpdate()
    {
        if ( _playerPrefab != null )
        {
            Vector3 deltaMovement = _cameraTransform.position-_backgroundPosition;
            transform.position += new Vector3(deltaMovement.x*_parallaxCoefficientX, deltaMovement.y*_parallaxCoefficientY, 0.0f);
            _backgroundPosition = _cameraTransform.position;

            if ( Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureUnitSize )
            {
                float offset = (_cameraTransform.position.y-transform.position.y) % _textureUnitSize;
                transform.position = new Vector3(transform.position.x, _cameraTransform.position.y+offset, 0.0f);
            }
        }
    }

    void calculateMovement()
    {
        transform.Translate(0, -_speed*Time.deltaTime, 0);
    }
}
