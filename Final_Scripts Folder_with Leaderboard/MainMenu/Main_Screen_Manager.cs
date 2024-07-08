using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_Screen_Manager : MonoBehaviour
{
    [Header("Levels to Load")]
    public static string _newGameLevel = "Level1"; // Updated to static
    // Start is called before the first frame update
    public void NewGameDIalogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void Exitbutton()
    {
        Application.Quit();
    }
}
