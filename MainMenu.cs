//Script responsible for managing the Main Menu.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
    {
        SceneManager.LoadScene("Menu_Fields");
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
