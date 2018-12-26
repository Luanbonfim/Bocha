//Script responsible for managing the game over menu (when the red team wins).
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RedGameOverMenu : MonoBehaviour
{
    Text r_Score;
    private void Start()
    {
        r_Score = GetComponent<Text>();
    }
    public void Update()
    {
        r_Score.text = "" + R_Score.r_scoreValue;
    }
    public void PlayAgain()
    {
        R_Balls.r_ballsValue = 6;
        B_Balls.b_ballsValue = 6;
        B_Score.b_scoreValue = 0;
        R_Score.r_scoreValue = 0;
        SceneManager.LoadScene("Menu_Fields");
      
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
