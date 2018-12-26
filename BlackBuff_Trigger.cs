//Script responsible for managing the black buff, when triggered by any ball.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBuff_Trigger : MonoBehaviour {
    public static bool BlackBuffBlueOn;
    public static bool BlackBuffRedOn;
    public static bool BlackTriggered;
    private void Start()
    {
        StartCoroutine(DestroyBuffs());
    }
    private void OnTriggerEnter(Collider ball)
    {
        
        if (ball.gameObject.tag == "BlueBall")
        {
            ShooterBlue.blackBuff = true;
            ShooterRed.blackBuff = true;
            ShooterBlue.blackBuffAudio = true;
            BlackTriggered = true;
            BlackBuffBlueOn = true;
            if (Buff_Spawner.BlackPosi2 == true)
            {
                Destroy(Buff_Spawner.BlackBuffClone[2]);
                Destroy(Buff_Spawner.BlackTriggerClone[2]);
            }
            Destroy(Buff_Spawner.BlackBuffClone[Buff_Spawner.l]);
            Destroy(Buff_Spawner.BlackTriggerClone[Buff_Spawner.l]);
           // Debug.Log("BALL TRIGGERED THE BUFF!!!");
            //Debug.Log("Black DESTROYED");
            Buff_Spawner.l--;
        }

        if (ball.gameObject.tag == "RedBall")
        {
            ShooterBlue.blackBuff = true;
            ShooterRed.blackBuff = true;
            ShooterRed.blackBuffAudio = true;
            BlackTriggered = true;
            BlackBuffRedOn = true;
            if (Buff_Spawner.BlackPosi2 == true)
            {
                Destroy(Buff_Spawner.BlackBuffClone[2]);
                Destroy(Buff_Spawner.BlackTriggerClone[2]);
            }
            Destroy(Buff_Spawner.BlackBuffClone[Buff_Spawner.l]);
            Destroy(Buff_Spawner.BlackTriggerClone[Buff_Spawner.l]);
           // Debug.Log("BALL TRIGGERED THE BUFF!!!");
           // Debug.Log("Black DESTROYED");
            Buff_Spawner.l--;
        }
    }

    IEnumerator DestroyBuffs()
    {
        yield return new WaitForSeconds(20);
        if (Buff_Spawner.BlackPosi2 == true)
        {
            Destroy(Buff_Spawner.BlackBuffClone[2]);
            Destroy(Buff_Spawner.BlackTriggerClone[2]);
        }
        Destroy(Buff_Spawner.BlackBuffClone[Buff_Spawner.l]);
        Destroy(Buff_Spawner.BlackTriggerClone[Buff_Spawner.l]);
        // Debug.Log("BALL TRIGGERED THE BUFF!!!");
        // Debug.Log("Black DESTROYED");
        Buff_Spawner.l--;
    }

}
