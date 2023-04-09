using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);        
    }

    // End game if ESC is pressed
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }
    }
}
