using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
    [Header("PANELS")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject loseMenuPanel;
    [SerializeField] private GameObject winMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("FADE FIELDS")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private TextMeshProUGUI fadeText;

    [Header("SETTINGS MENU FIELDS")]
    [SerializeField] private AudioMixer audioMixer;

    private Sequence _fadeSequence;
    private Sequence _panelSequence;
    private Sequence _gameStateSequence;
    private void OnEnable()
    {
        GameEvents.Instance.OnGameStatesChanged += GameStatePanels;
    }


    private void OnDisable()
    {
        GameEvents.Instance.OnGameStatesChanged -= GameStatePanels;
        KillSequences();
    }
    private void Start()
    {
        HidePanel(pauseMenuPanel);
        HidePanel(loseMenuPanel);
        HidePanel(winMenuPanel);
        Time.timeScale = 1.0f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.CurrentGameState != GameStates.GamePause)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }

        }
    }
    public void PauseGame()
    {
        if (GameManager.Instance.CurrentGameState != GameStates.GameActive) return;

        ShowPanel(pauseMenuPanel);
        GameManager.Instance.CurrentGameState = GameStates.GamePause;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        HidePanel(settingsPanel);
        HidePanel(pauseMenuPanel);
        GameManager.Instance.CurrentGameState = GameStates.GameActive;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    #region Settings Methods
    public void OpenSettings()
    {
        ShowPanel(settingsPanel);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("BGVolume", volume);
    }
    #endregion
    public void ExitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        KillSequences();
        AudioManager.Instance.PlaySound(SoundList.UISound);
        HidePanel(loseMenuPanel);
        HidePanel(winMenuPanel);

        _fadeSequence = DOTween.Sequence()
            .SetUpdate(true) 
            .Append(FadeInTween(1, 0.5f)) 
            .OnComplete(() =>
            {
                GameManager.Instance.CurrentGameState = GameStates.GameActive;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                StartCoroutine(ReloadSceneAsync());
            });
    }
    private IEnumerator ReloadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        operation.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
        operation.allowSceneActivation = true;
    }
    private void GameWinSequence()
    {
        KillSequences();
        AudioManager.Instance.PlaySound (SoundList.WinSound);
        fadeText.color = Color.green;
        fadeText.text = "YOU WON";
        fadeText.alpha = 0f;
        fadeText.enabled = true;
        _gameStateSequence = DOTween.Sequence().SetUpdate(true);
        _gameStateSequence.Append(FadeInTween(1, 2));
        _gameStateSequence.Append(fadeText.DOFade(1, 2));
        _gameStateSequence.Append(fadeText.DOFade(0, 1));
        _gameStateSequence.AppendCallback(() => fadeText.enabled = false);
        _gameStateSequence.AppendCallback(() => ShowPanel(winMenuPanel));

    }
    private void GameLoseSequence()
    {
        KillSequences();
        AudioManager.Instance.PlaySound(SoundList.LoseSound);
        fadeText.color = Color.red;
        fadeText.text = "YOU LOSE";
        fadeText.alpha = 0f;
        fadeText.enabled = true;
        _gameStateSequence = DOTween.Sequence().SetUpdate(true);
        _gameStateSequence.Append(FadeInTween(1, 2));
        _gameStateSequence.Append(fadeText.DOFade(1, 2));
        _gameStateSequence.Append(fadeText.DOFade(0, 1));
        _gameStateSequence.AppendCallback(() => fadeText.enabled = false);
        _gameStateSequence.AppendCallback(() => ShowPanel(loseMenuPanel));

    }
    public void GameStatePanels(GameStates state)
    {
        switch(state)
        {
            case GameStates.GameWon:
                {
                    GameWinSequence();
                    break;
                }
            case GameStates.GameLose:
                {
                    GameLoseSequence();
                    break;
                }
            default:
                    break;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.one * 0.8f;
        _panelSequence = DOTween.Sequence().SetUpdate(true)
            .Append(FadeInTween(0.9f, 0.4f))
            .Join(panel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutExpo));
    }
    public void HidePanel(GameObject panel)
    {
        _panelSequence = DOTween.Sequence().SetUpdate(true)
            .Join(FadeOutTween(0, 0.4f))
            .Join(panel.transform.DOScale(0.8f, 0.4f).SetEase(Ease.InExpo))
            .OnComplete(() =>
            {
                panel.SetActive(false);
            });
    }
    private Tween FadeInTween(float fadeValue, float duration)
    {
        return fadePanel.DOFade(fadeValue, duration);
    }
    private Tween FadeOutTween(float fadeValue, float duration)
    {
        return fadePanel.DOFade(fadeValue, duration);
    }
    private void KillSequences()
    {
        _fadeSequence?.Kill();
        _panelSequence?.Kill();
    }
    public void PlayButtonSounds()
    {
        AudioManager.Instance.PlaySound(SoundList.UISound);
    }
}
