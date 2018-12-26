//Script Responsible for managing the red team score (text on the UI).
using UnityEngine;
using UnityEngine.UI;
public class R_Score : MonoBehaviour
{

    public static int r_scoreValue = 0;
    Text r_Score;
    public Transform blueTeam;
    public Text scoreText;

    private void Start()
    {
        r_Score = GetComponent<Text>();
    }

    void Update()
    {

        r_Score.text = "" + r_scoreValue;

    }
}
