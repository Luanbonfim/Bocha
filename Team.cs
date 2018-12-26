//Script responsible for managing the Team's turns and also the winner, in the end of the match.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Team : MonoBehaviour
{
 
    public enum turn
    {
        blue, red, none
    }
    public ShooterBlue ShooterBlue;
    public ShooterRed ShooterRed;
    public Transform prefabRed;
    public Transform prefabBlue;
    public turn turnoAtual;
    public R_Balls redBalls;
    B_Score blueScore;
    R_Score redScore;
    public Transform m_cube;

    public void Start()
    {
        turnoAtual = turn.blue;
        Instanciar();
    }

    public void Update() {

        calcWinner();

            if (turnoAtual== turn.none)
        {
            ShooterRed.enabled = false;
            ShooterBlue.enabled = false;
            
        }
    }

    public void Instanciar()
         {
        if (turnoAtual == turn.blue)
        {
            Instantiate(prefabBlue, m_cube.position, m_cube.rotation);

        }
             
        if (turnoAtual == turn.red)
        {
            Instantiate(prefabRed, m_cube.position, m_cube.rotation);

        }

    }
    private void calcWinner()
    {
        if (R_Balls.r_ballsValue == 0 && B_Score.b_scoreValue > R_Score.r_scoreValue)
        {
            SceneManager.LoadScene("BlueTeamWins");
            turnoAtual = turn.none;

        }
        if (R_Balls.r_ballsValue == 0 && B_Score.b_scoreValue < R_Score.r_scoreValue)
        {
            SceneManager.LoadScene("RedTeamWins");
            turnoAtual = turn.none;
        }
        if (R_Balls.r_ballsValue == 0 && B_Score.b_scoreValue == R_Score.r_scoreValue)
        {
            SceneManager.LoadScene("Empate");
            turnoAtual = turn.none;
        }

    }
 

}

