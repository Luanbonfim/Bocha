//Script responsible for managing the blue team's score feedback.
using UnityEngine;
using UnityEngine.UI;
public class B_ScoreFeedBack : MonoBehaviour {

    public static int b_scoreFeedBack= 0;
    Text b_Score;
    public Text scoreText;
    private bool playIncrement;
    private bool playDecrement;

    private void Start()
    {
        b_Score = GetComponent<Text>();
    }
    private void Update()
    {
        PlayAudio();
    }

    public void ChangeScore()
    {
        b_Score.text = "+ " + b_scoreFeedBack;
        playIncrement = true;
    }

    public void DecrementScore()
    {
        b_Score.text = "- " + b_scoreFeedBack;
        playDecrement = true;
    }

    public void ChangeScoreBack()
    {
        b_Score.text = "";
    }

    private void PlayAudio()
    {
        if (playIncrement == true)
        {
            FindObjectOfType<AudioManager>().play("Score");
            playIncrement = false;
        }
        if (playDecrement == true)
        {
            FindObjectOfType<AudioManager>().play("DecrementScore");
            playDecrement = false;
        }
    }
}
