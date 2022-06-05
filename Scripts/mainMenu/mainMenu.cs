using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Text _progressText;
    [SerializeField] private GameObject _difficultyToggles, _scorePanel;
    [SerializeField] private Button[] _menuButtons;
    [SerializeField] private Text[] _scoreText;


    // Start is called before the first frame update
    void Start()
    {
        _difficultyToggles.transform.GetChild((int)difficultyValues.difficulty).GetComponent<Toggle>().isOn = true;
    }

    void restoreColor()
    {
        for ( int i = 0; i < _menuButtons.Length; ++i )
            _menuButtons[i].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(0, 140, 255, 255);
    }

    public void LoadGame(int sceneIndex)
    {
        switch ( sceneIndex )
        {
            case 1:
                difficultyValues.isCoopMode = false;
                break;
            case 2:
                difficultyValues.isCoopMode = true;
                break;
        }

        StartCoroutine(LoadAsynchronously(sceneIndex));
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

    public void quitGame()
    {
        bestScore.saveBestScore();
        difficultyValues.saveDifficultyValue();

        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void chooseDifficultyGame()
    {
        for ( int i = 0; i < _menuButtons.Length; ++i )
            _menuButtons[i].gameObject.SetActive(false);

        _difficultyToggles.SetActive(true);
        _menuButtons[3].gameObject.SetActive(true);

        _difficultyToggles.transform.GetChild((int)difficultyValues.difficulty).GetComponent<Toggle>().isOn = true;
    }

    public void showBestScore()
    {
        for ( int i = 0; i < _menuButtons.Length; ++i )
            _menuButtons[i].gameObject.SetActive(false);

        for ( int i = 0; i < _scoreText.Length; ++i )
            _scoreText[i].text = (i+1)+"°: "+bestScore.popBestScore(i);

        _scorePanel.SetActive(true);
        _menuButtons[3].gameObject.SetActive(true);
    }

    public void backToMainMenu()
    {
        restoreColor();

        for ( int i = 0; i < _menuButtons.Length; ++i )
            _menuButtons[i].gameObject.SetActive(true);

        _difficultyToggles.SetActive(false);
        _scorePanel.SetActive(false);
        _menuButtons[3].gameObject.SetActive(false);
    }

    #region difficulty
    public void setEasyDificulty(bool isOn)
    {
        if ( isOn )
            difficultyValues.difficulty = difficultyValues.Difficulties.Easy;
    }

    public void setMediumDificulty(bool isOn)
    {
        if ( isOn )
            difficultyValues.difficulty = difficultyValues.Difficulties.Medium;
    }

    public void setHardDificulty(bool isOn)
    {
        if ( isOn )
            difficultyValues.difficulty = difficultyValues.Difficulties.Hard;
    }
    #endregion
}
