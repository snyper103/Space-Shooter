using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class gameManager : MonoBehaviour
{
    private spawnManager _spawnManager;
    private UImanager _UIpause;

    void Start()
    {
        _spawnManager = GameObject.FindWithTag("Respawn").GetComponent<spawnManager>();
        _UIpause = GameObject.FindWithTag("UI_Manager").GetComponent<UImanager>();

        if ( !_spawnManager )
            Debug.LogError("gameManager::_spawnManager is NULL");

        if ( !_UIpause )
            Debug.LogError("gameManager::_UIpause is NULL");
    }

    // Update is called once per frame
    void Update()
    {
        if ( _spawnManager.isGameOver() && Input.GetKeyDown(KeyCode.R) )
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // Game scene 1
        else
        {
            if( difficultyValues.isCoopMode )
            {
                if ( (_spawnManager.isPlayerAlive() || _spawnManager.isPlayer2Alive()) && Input.GetKeyDown(KeyCode.Escape) )
                {
                    Time.timeScale = 0f;

                    _UIpause.setOnPauseMenu(true);
                }

                else
                    if ( _spawnManager.isGameOver() && Input.GetKeyDown(KeyCode.Escape) )
                        SceneManager.LoadScene(0);   // Game scene 0
            }

            else
            {
                if ( _spawnManager.isPlayerAlive() && Input.GetKeyDown(KeyCode.Escape) )
                {
                    Time.timeScale = 0f;

                    _UIpause.setOnPauseMenu(true);
                }

                else
                    if ( _spawnManager.isGameOver() && Input.GetKeyDown(KeyCode.Escape) )
                        SceneManager.LoadScene(0);   // Game scene 0
            }
        }
    }
}
