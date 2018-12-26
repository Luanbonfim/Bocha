//Script responsible for managing the game over menu (when the blue team wins).
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlueGameOverMenu : MonoBehaviour
{
    Text b_Score;
    private void Start()
    {
        b_Score = GetComponent<Text>();
    }
    public void Update()
    {
        b_Score.text = "" + B_Score.b_scoreValue;
        Debug.Log("Score: " + B_Score.b_scoreValue);
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
