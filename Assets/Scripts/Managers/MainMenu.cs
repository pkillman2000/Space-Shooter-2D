using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // This is used by the "New Game" button in the Main Menu Scene
    public void LoadNewGame()
    {
        SceneManager.LoadScene("2DGame");
    }
}
