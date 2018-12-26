//Script responsible to manage the blue team score.
using UnityEngine;
using UnityEngine.UI;
public class B_Score : MonoBehaviour {

    public static int b_scoreValue = 0;
    Text b_Score;
    public Transform blueTeam;
    public Text scoreText;

    private void Start()
    {
        b_Score = GetComponent<Text>();
    }

    void Update () {

        b_Score.text = "" + b_scoreValue;
		
	}
}
