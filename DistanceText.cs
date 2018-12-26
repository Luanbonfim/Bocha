//Script responsible for managing the text showing the distance between the current player and the white ball (text on the UI).
using UnityEngine;
using UnityEngine.UI;

public class DistanceText : MonoBehaviour
{

    public static int distance = 0;
    Text distanceFromWhiteBall;
    public Transform blueTeam;
    public Text distanceText;

    private void Start()
    {
        distanceFromWhiteBall = GetComponent<Text>();
    }

    void Update()
    {
        if (R_Balls.r_ballsValue != 0)
        {
            distanceFromWhiteBall.text = distance + "m";
        }

    }
}
