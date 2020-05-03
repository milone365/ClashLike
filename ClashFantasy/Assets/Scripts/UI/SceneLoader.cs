using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    

    public void loadLevel(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
