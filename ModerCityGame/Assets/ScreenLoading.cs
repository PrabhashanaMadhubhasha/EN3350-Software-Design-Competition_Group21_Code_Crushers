using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Add the scene names in the order you want to load them
    private string[] sceneNames = { "LoadingScreen", "MainMenu" };
    private int currentSceneIndex = 0;

    public MonoBehaviour scriptToRunAfterLoadingScene3; // Reference to the script to run after loading Scene 3

    void Start()
    {
        // Start loading the next scene in the array
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        if (currentSceneIndex < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[currentSceneIndex]);
            currentSceneIndex++;
        }
        else
        {
            Debug.Log("All scenes loaded.");
            if (scriptToRunAfterLoadingScene3 != null)
            {
                // Run the script attached to the GameObject in Scene 3
                scriptToRunAfterLoadingScene3.enabled = true;
            }
        }
    }
}
