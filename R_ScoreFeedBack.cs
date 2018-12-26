//Script responsible for managing the red tem score's feedback (Text on the UI and audio).
using UnityEngine;
using UnityEngine.UI;
public class R_ScoreFeedBack : MonoBehaviour
{

    public static int r_scoreFeedBack = 0;
    Text r_Score;
    public Text scoreText;
    private bool playIncrement;
    private bool playDecrement;

    private void Start()
    {
        r_Score = GetComponent<Text>();
    }
    private void Update()
    {
        PlayAudio();
    }

    public void ChangeScore()
    {
        r_Score.text = "+ " + r_scoreFeedBack;
        playIncrement = true;
    }

    public void DecrementScore()
    {
        r_Score.text = "- " + r_scoreFeedBack;
        playDecrement = true;
    }

    public void ChangeScoreBack()
    {
        r_Score.text = "";
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
