using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
