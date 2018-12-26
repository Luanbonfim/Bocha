//Script responsible to manage the blue buff, when it is triggered by any ball.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBuff_Trigger : MonoBehaviour {
    public static bool BlueBuffOn;
    public static bool BlueTriggered;
    // public GameObject BlueBuffObj;

    private void Start()
    {
        StartCoroutine(DestroyBuffs());
    }

    private void OnTriggerEnter(Collider ball)
    {
        
        if (ball.gameObject.tag == "BlueBall" || ball.gameObject.tag == "RedBall")
        {
            BlueTriggered = true;
            BlueBuffOn = true;
            if (Buff_Spawner.bluePosi2 == true)
            {
                Destroy(Buff_Spawner.BlueBuffClone[2]);
                Destroy(Buff_Spawner.BlueTriggerClone[2]);
            }
            Destroy(Buff_Spawner.BlueBuffClone[Buff_Spawner.j]);
            Destroy(Buff_Spawner.BlueTriggerClone[Buff_Spawner.j]);
          //  Debug.Log("BALL TRIGGERED THE BUFF!!!");
           // Debug.Log("BLUE DESTROYED");
            Buff_Spawner.j--;
        }
    }

    IEnumerator DestroyBuffs()
    {
        yield return new WaitForSeconds(20);
            if (Buff_Spawner.bluePosi2 == true)
            {
                Destroy(Buff_Spawner.BlueBuffClone[2]);
                Destroy(Buff_Spawner.BlueTriggerClone[2]);
            }
            Destroy(Buff_Spawner.BlueBuffClone[Buff_Spawner.j]);
            Destroy(Buff_Spawner.BlueTriggerClone[Buff_Spawner.j]);
        //  Debug.Log("BALL TRIGGERED THE BUFF!!!");
        // Debug.Log("BLUE DESTROYED");
        Buff_Spawner.j--;       

    }



}

