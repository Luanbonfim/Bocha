//Script responsible to manage the bullets spawning, when the black buff is triggered by any ball.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBuffSpawner : MonoBehaviour {

    public GameObject BlueBulletsClone;
    public GameObject RedBulletsClone;
    public Transform BulletsReference;
    private GameObject BlueBullets;
    private GameObject RedBullets;
    private GameObject[] gameObjects;
    private static bool BlueOn;
    private static bool RedOn;
    public static int Balls;
    public B_ScoreFeedBack BScore;
    public R_ScoreFeedBack RScore;
    private bool playAudio;

    private void Update()
    {
        playLaserShots();

        if (BlackBuff_Trigger.BlackBuffBlueOn == true)
        {
            BlueBullets = Instantiate(BlueBulletsClone, BulletsReference.position, BulletsReference.rotation) as GameObject;
            BlueOn = true;
            playAudio = true;
            StartCoroutine(DestroyBalls());
            BlackBuff_Trigger.BlackBuffBlueOn = false;
        }

        if (BlackBuff_Trigger.BlackBuffRedOn == true)
        {
            RedBullets = Instantiate(RedBulletsClone, BulletsReference.position, BulletsReference.rotation) as GameObject;
            RedOn = true;
            playAudio = true;
            StartCoroutine(DestroyBalls());
            BlackBuff_Trigger.BlackBuffRedOn = false;
        }       
    }

   public IEnumerator DestroyBalls()
    {
        yield return new WaitForSeconds(5);
        if (BlueOn == true)
        {
            gameObjects = GameObject.FindGameObjectsWithTag("OffRedBall");
            Balls = gameObjects.Length;
            Balls = (Balls * 2);
            if (gameObjects.Length == 0)
            {
                Destroy(BlueBullets);
                BlueOn = false;
                
            }
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Destroy(gameObjects[i]);
                R_Score.r_scoreValue -= Balls;
                Destroy(BlueBullets);
                BlueOn = false;
           
            }
            R_ScoreFeedBack.r_scoreFeedBack = Balls;
            RScore.DecrementScore();
            StartCoroutine(DestroyTextR());
        }

        if (RedOn == true)
        {
            gameObjects = GameObject.FindGameObjectsWithTag("OffBlueBall");
            Balls = gameObjects.Length;
            Balls = (Balls * 2);
            if (gameObjects.Length == 0)
            {
                Destroy(RedBullets);
                RedOn = false;
               
            }
            for (int i = 0; i < gameObjects.Length; i++)
            {
                B_Score.b_scoreValue -= Balls;
                Destroy(gameObjects[i]);
                Destroy(RedBullets);
                RedOn = false;
               
            }
            B_ScoreFeedBack.b_scoreFeedBack = Balls;
            BScore.DecrementScore();
            StartCoroutine(DestroyTextB());
        }

    }

    public IEnumerator DestroyTextB()
    {
        yield return new WaitForSeconds(1);
        BScore.ChangeScoreBack();
    }

    public IEnumerator DestroyTextR()
    {
        yield return new WaitForSeconds(1);
        RScore.ChangeScoreBack();
    }

    private void playLaserShots()
    {
        if (playAudio == true)
        {
            FindObjectOfType<AudioManager>().play("LaserShots");
            playAudio = false;
        }
    }
}



