using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class StartCinematicController : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    [SerializeField] private string nextSceneName;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();

        if (!PlayerPrefs.HasKey("StartFlag"))
        {
            PlayerPrefs.SetInt("StartFlag", 1);
            _videoPlayer.Play();
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _videoPlayer.Stop();
            SceneManager.LoadScene(nextSceneName);
        }

        if(_videoPlayer.time > 1f && !_videoPlayer.isPlaying)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
