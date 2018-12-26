//Script responsible for managing the blue team balls.
using UnityEngine;
using UnityEngine.UI;
public class B_Balls : MonoBehaviour
{

    public static int b_ballsValue = 6;
    Text b_Balls;
    public Transform blueTeam;
    public Text scoreText;

    private void Start()
    {
        b_Balls = GetComponent<Text>();
    }

    void Update()
    {

        b_Balls.text = "" + b_ballsValue;

    }
}

