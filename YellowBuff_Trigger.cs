//Script responsible for managing the yellow buff behavior when triggered by any ball.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBuff_Trigger : MonoBehaviour {
    public static bool YellowBuffOn;
    public static bool YellowTriggered;
    public static bool playYellow;
    // public GameObject YellowBuffObj;
    private void Start()
    {
        StartCoroutine(DestroyBuffs());
    }
    private void OnTriggerEnter(Collider ball)
    {

        if (ball.gameObject.tag == "BlueBall" || ball.gameObject.tag == "RedBall")
        {
            playYellow = true;
            YellowBuffOn = true;
            if (Buff_Spawner.yellowPosi2 == true)
            {
                Destroy(Buff_Spawner.BuffClone[2]);
                Destroy(Buff_Spawner.YellowTriggerClone[2]);
            }
            Destroy(Buff_Spawner.BuffClone[Buff_Spawner.i]);
            Destroy(Buff_Spawner.YellowTriggerClone[Buff_Spawner.i]);
            // Debug.Log("BALL TRIGGERED THE BUFF!!!");
            //Debug.Log("YELLOW DESTROYED");
            Buff_Spawner.i--;
        }
    }

    private static void NewMethod()
    {
        YellowTriggered = true;
    }

    IEnumerator DestroyBuffs()
    {
        yield return new WaitForSeconds(20);
        if (Buff_Spawner.yellowPosi2 == true)
        {
            Destroy(Buff_Spawner.BuffClone[2]);
            Destroy(Buff_Spawner.YellowTriggerClone[2]);
        }
        Destroy(Buff_Spawner.BuffClone[Buff_Spawner.i]);
        Destroy(Buff_Spawner.YellowTriggerClone[Buff_Spawner.i]);
        // Debug.Log("BALL TRIGGERED THE BUFF!!!");
        //Debug.Log("YELLOW DESTROYED");
        Buff_Spawner.i--;

    }


 }

