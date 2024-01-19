using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame(){
        #if UNITY_EDITOR
            // Quitting in Unity Editor:
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
            // Quitting in the WebGL build:
            Application.OpenURL(Application.absoluteURL);
        #else // !UNITY_WEBPLAYER
            // Quitting in all other builds: 
            Application.Quit(); 
        #endif
    }
}
