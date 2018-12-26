//Script responsible for managing the feedbacks (texts on the UI), when any buff is triggered.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffsFeedBack : MonoBehaviour {

    Text FeedBack;
    public Text FeedBackText;

    private void Start()
    {
        FeedBack = GetComponent<Text>();
    }

    public void BlueBuff()
    {
        FeedBack.text = "BLUE BUFF ACTIVATED, " +
            "Enemy's Strength Bar Is Now Crazy";
        FeedBackText.color = Color.blue;
    }

    public void YellowBuff()
    {
        FeedBack.text = "YELLOW BUFF ACTIVATED, " +
            "Score Doubled";
        FeedBackText.color = Color.yellow;

    }

    public void BlackBuff()
    {
        FeedBack.text = "BLACK BUFF ACTIVATED, " +
            "Destroying Enemy's Balls";
        FeedBackText.color = Color.black;
    }

    public IEnumerator DestroyFeedBack()
    {
        yield return new WaitForSeconds(1);
        FeedBack.text = "";
    }


}
