using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugPlay : MonoBehaviour
{
    private void Awake()
    {
        //starts from mainmenu when playing in editor
        if(GameManager.instance == null)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
