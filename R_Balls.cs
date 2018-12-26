//Script responsible for managing the red team balls
using UnityEngine;
using UnityEngine.UI;
public class R_Balls : MonoBehaviour
{

    public static int r_ballsValue = 6;
    Text r_Balls;
    public Transform redTeam;
    public Text scoreText;

    private void Start()
    {
        r_Balls = GetComponent<Text>();
    }

    void Update()
    {

        r_Balls.text = "" + r_ballsValue;

    }
}

