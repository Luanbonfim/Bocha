//Script responsible for managing the "stage selection" menu.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fields_Menu : MonoBehaviour
{

    public void LoadField1()
    {
        SceneManager.LoadScene("Field1");
    }
    public void LoadField2()
    {
        SceneManager.LoadScene("Field2");
    }
    public void LoadField3()
    {
        SceneManager.LoadScene("Field3");
    }
    public void LoadMainManu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlayAudioSelect()
    {
        FindObjectOfType<AudioManager>().play("Select");
    }
    public void PlayAudioClick()
    {
        FindObjectOfType<AudioManager>().play("Click");
    }
}
