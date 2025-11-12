using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;

    [SerializeField] private AudioClip buttonClickSFX;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void OpenTutorialPanel()
    {
        tutorialPanel.SetActive(true);
        tutorialPanel.transform.localScale = Vector3.one * 0.8f;
        tutorialPanel.transform.DOScale(1, 0.4f).SetEase(Ease.OutExpo);
    }
    public void CloseTutorialPanel()
    {
        tutorialPanel.transform.DOScale(0.8f, 0.4f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            tutorialPanel.SetActive(false);
        });
    }
    public void OpenSocialSite(string url)
    {
        Application.OpenURL(url);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void PlayButtonSFX()
    {
        _audioSource.PlayOneShot(buttonClickSFX, 0.5f);
    }

}
