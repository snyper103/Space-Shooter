using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [SerializeField] private Text _gameOverText, _restartText, _mainMenuText;
    [SerializeField] private Text[] _scoreText, _info;
    [SerializeField] private Image[] _livesImg;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private GameObject _pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        logError();

        StartCoroutine(displayInfo());

        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        for ( int i = 0; i < _scoreText.Length; ++i )
            _scoreText[i].text = "Score: " + 0;
    }

    void logError()
    {
        for ( int i = 0; i < _scoreText.Length; ++i )
            if ( !_scoreText[i] )
                Debug.LogError("UImanager::_scoreText is NULL on instance " + i);

        if ( !_gameOverText )
            Debug.LogError("UImanager::_gameOverText is NULL");

        if ( !_restartText )
            Debug.LogError("UImanager::_restartText is NULL");

        if ( !_mainMenuText )
            Debug.LogError("UImanager::_mainMenuText is NULL");

        for ( int i = 0; i < _livesImg.Length; ++i )
            if ( !_livesImg[i] )
                Debug.LogError("UImanager::_livesImg is NULL on instance " + i);

        for ( int i = 0; i < _liveSprites.Length; ++i )
            if ( !_liveSprites[i] )
                Debug.LogError("UImanager::_liveSprites is NULL on instance " + i);

        for ( int i = 0; i < _info.Length; ++i )
            if ( !_info[i] )
                Debug.LogError("UImanager::_info is NULL on instance " + i);

        if ( !_pauseMenu )
            Debug.LogError("UImanager::_pauseMenu is NULL");
    }

    public void updateScore(int score, int index)
    {
        _scoreText[index].text = "Score: " + score;
    }
    public void updateLives(int currentLive, int index)
    {
        _livesImg[index].sprite = _liveSprites[currentLive];
    }
    public void setOnGameOver(bool enable)
    {
        _gameOverText.gameObject.SetActive(enable);
    }
    public void setOnRestart(bool enable)
    {
        _restartText.gameObject.SetActive(enable);
    }
    public void setOnGetBack(bool enable)
    {
        _mainMenuText.gameObject.SetActive(enable);
    }
    public void setOnPauseMenu(bool enable)
    {
        _pauseMenu.SetActive(enable);
        _pauseMenu.GetComponent<pauseMenu>().pausingGame(enable);
    }

    IEnumerator displayInfo()
    {
        yield return new WaitForSeconds(2.0f);

        for ( int i = 0; i < _info.Length; ++i )
            _info[i].gameObject.SetActive(false);
    }
}
