using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Text _progressText;
    [SerializeField] private Button[] _menuButtons;
    [SerializeField] private GameObject _Player;
    [SerializeField] private GameObject _Player2;
    private Animator _pauseAnim;

    // Start is called before the first frame update
    void Start()
    {
        _pauseAnim = gameObject.GetComponent<Animator>();
        
        if ( !_pauseAnim )
            Debug.LogError("pauseMenu::_pauseAnim is NULL");

        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {        
        if ( Input.GetKeyDown(KeyCode.Escape) )
            continueGame();
    }

    void restoreColor()
    {
        for ( int i = 0; i < _menuButtons.Length; ++i )
            _menuButtons[i].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(0, 140, 255, 255);
    }

    public void continueGame()
    {
        restoreColor();
        _pauseAnim.SetBool("isPaused", false);
        
        Time.timeScale = 1f;

        StartCoroutine(setOffPauseMenu());
    }
    IEnumerator setOffPauseMenu()
    {
        yield return new WaitForSeconds(0.5f);

        this.gameObject.SetActive(false);
    }

    public void restartGame()
    {
        changeBestScore();
        
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));

        Time.timeScale = 1f;
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);

        _loadingBar.gameObject.SetActive(true);

        while ( !op.isDone )
        {
            float progress = Mathf.Clamp01(op.progress/0.9f);
            _loadingBar.value = progress;
            _progressText.text = progress*100.0f+"%";

            yield return null;
        }
    }

    public void backToMainMenu()
    {
        changeBestScore();

        StartCoroutine(LoadAsynchronously(0));

        Time.timeScale = 1f;
    }
    
    public void quitGame()
    {
        changeBestScore();
        bestScore.saveBestScore();
        difficultyValues.saveDifficultyValue();
        PlayerPrefs.Save();

        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void pausingGame(bool enable)
    {
        _pauseAnim.SetBool("isPaused", enable);
    }

    void changeBestScore()
    {
        if ( difficultyValues.isCoopMode )
        {
            bestScore.updateBestScore(_Player.GetComponent<player>().returnScore());
            bestScore.updateBestScore(_Player2.GetComponent<player2>().returnScore());
        }
        else
            bestScore.updateBestScore(_Player.GetComponent<player>().returnScore());
    }
}
