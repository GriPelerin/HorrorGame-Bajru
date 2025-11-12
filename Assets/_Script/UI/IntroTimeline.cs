using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroTimeline : MonoBehaviour
{
    private PlayableDirector _playableDirector;

    [SerializeField] private AudioSource BGSound;

    private void Start()
    {
        _playableDirector.stopped += OnTimelineFinished;
    }

    private void OnDestroy()
    {
        _playableDirector.stopped -= OnTimelineFinished;
    }
    private void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }
    public void PlayIntroTimeline()
    {
        _playableDirector.Play();
        BGSound.Stop();
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        SceneManager.LoadScene("Game");
    }
}
