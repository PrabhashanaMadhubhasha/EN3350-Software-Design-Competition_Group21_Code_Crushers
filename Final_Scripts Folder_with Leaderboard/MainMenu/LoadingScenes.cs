using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoadingScenes : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string loadingSceneName = "LoadingScreen";


    private void Start()
    {
        // Subscribe to the videoPlayer's loopPointReached event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Load the loading page scene
        SceneManager.LoadScene(loadingSceneName);

    }

    
}
