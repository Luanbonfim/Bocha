//Script responsible for managing all the buffs spawning and destroying.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Spawner : MonoBehaviour
{
    public GameObject[] Buffs;
    public GameObject YellowTrigger;
    public GameObject BlueTrigger;
    public GameObject BlackTrigger;
    public static GameObject[] BuffClone;
    public static GameObject[] YellowTriggerClone;
    public static GameObject[] BlueBuffClone;
    public static GameObject[] BlueTriggerClone;
    public static GameObject[] BlackBuffClone;
    public static GameObject[] BlackTriggerClone;
    public Transform BuffReference;
    public Transform ReferenceTrigger;
    public float SpawnWait;
    public float SpawnMostWait;
    public float SpawnLeastWait;
    public int StartWait;
    int randBuff;
    public static int i;
    public static int j;
    public static int l;
    public bool Stop;
    public static bool YellowOn;
    public static bool BlueOn;
    public static bool BlackOn;
    public static bool bluePosi2;
    public static bool yellowPosi2;
    public static bool BlackPosi2;

    private enum turn
    {
        yellow, blue, black, none
    }
    private turn buffAtual;

    private void Start()
    {
        i =0;
        j =0;
        l = 0;
        bluePosi2=false;
        yellowPosi2=false;
        BlackPosi2 = false;
        BuffClone = new GameObject[3];
        YellowTriggerClone = new GameObject[3];
        BlueBuffClone = new GameObject[3];
        BlueTriggerClone = new GameObject[3];
        BlackBuffClone = new GameObject[3];
        BlackTriggerClone = new GameObject[3];
        YellowOn = false;
        BlueOn = false;
        BlackOn = false;

        buffAtual = turn.none;
        StartCoroutine(waitSpawner());
    }

    private void Update()
    {
        Debug.Log("BUFF: " + randBuff);
        SpawnWait = Random.Range(SpawnLeastWait, SpawnMostWait);

        //Destruindo Azul caso seja vez do Azul
        if (buffAtual == turn.blue && BlueOn == true && j == 2)
        {
            bluePosi2 = true;
            j--;
            Destroy(BlueBuffClone[1]);
            Destroy(BlueTriggerClone[1]);
           // Debug.Log("BLUE DESTROYED");
           // Debug.Log("j= " + j);
        }

        //Destruindo Azul caso seja vez do amarelo
        if (buffAtual == turn.yellow && BlueOn == true)
        {
            //Se Houver buff azul na posição 2 do array será destruido
            if (bluePosi2 == true)
            {
                Destroy(BlueBuffClone[2]);
                Destroy(BlueTriggerClone[2]);
            }

                //Debug.Log("j= " + j);
                Destroy(BlueBuffClone[j]);
                Destroy(BlueTriggerClone[j]);
               // Debug.Log("BLUE DESTROYED");
                BlueOn = false;
                j--;
              //  Debug.Log("j= " + j);
            
        }

        //Destruindo Azul caso seja vez do preto
        if (buffAtual == turn.black && BlueOn == true)
        {
            //Se Houver buff azul na posição 2 do array será destruido
            if (bluePosi2 == true)
            {
                Destroy(BlueBuffClone[2]);
                Destroy(BlueTriggerClone[2]);
            }

            //Debug.Log("j= " + j);
            Destroy(BlueBuffClone[j]);
            Destroy(BlueTriggerClone[j]);
            // Debug.Log("BLUE DESTROYED");
            BlueOn = false;
            j--;
            //  Debug.Log("j= " + j);

        }


        //Destruindo Amarelo caso seja vez do Amarelo
        if (buffAtual == turn.yellow && YellowOn == true && i == 2)
        {
            yellowPosi2 = true;
            i--;
            Destroy(BuffClone[i]);
            Destroy(YellowTriggerClone[i]);
           // Debug.Log("YELLOW DESTROYED");
           // Debug.Log("i= " + i);
        }

        //Destruindo Amarelo caso seja vez do azul
        if (buffAtual == turn.blue && YellowOn == true)
        {
            //Se Houver buff amarelo na posição 2 do array será destruido
            if (yellowPosi2 == true)
            {
                Destroy(BuffClone[2]);
                Destroy(YellowTriggerClone[2]);
            }

               // Debug.Log("i= " + i);
                Destroy(BuffClone[i]);
                Destroy(YellowTriggerClone[i]);
               // Debug.Log("YELLOW DESTROYED");
                YellowOn = false;
                i--;
               // Debug.Log("i= " + i);
            
         }

        //Destruindo Amarelo caso seja vez do preto
        if (buffAtual == turn.black && YellowOn == true)
        {
            //Se Houver buff amarelo na posição 2 do array será destruido
            if (yellowPosi2 == true)
            {
                Destroy(BuffClone[2]);
                Destroy(YellowTriggerClone[2]);
            }

            // Debug.Log("i= " + i);
            Destroy(BuffClone[i]);
            Destroy(YellowTriggerClone[i]);
            // Debug.Log("YELLOW DESTROYED");
            YellowOn = false;
            i--;
            // Debug.Log("i= " + i);

        }

        //Destruindo Preto caso seja vez do Preto
        if (buffAtual == turn.black && BlackOn == true && l == 2)
        {
            BlackPosi2 = true;
            l--;
            Destroy(BlackBuffClone[l]);
            Destroy(BlackTriggerClone[l]);
            // Debug.Log("YELLOW DESTROYED");
            // Debug.Log("i= " + i);
        }

        //Destruindo Preto caso seja vez do azul
        if (buffAtual == turn.blue && BlackOn == true)
        {
            //Se Houver buff amarelo na posição 2 do array será destruido
            if (BlackPosi2 == true)
            {
                Destroy(BlackBuffClone[2]);
                Destroy(BlackTriggerClone[2]);
            }

            // Debug.Log("i= " + i);
            Destroy(BlackBuffClone[l]);
            Destroy(BlackTriggerClone[l]);
            // Debug.Log("YELLOW DESTROYED");
            BlackOn = false;
            l--;
            // Debug.Log("i= " + i);

        }

        //Destruindo Preto caso seja vez do Amarelo
        if (buffAtual == turn.yellow && BlackOn == true)
        {
            //Se Houver buff azul na posição 2 do array será destruido
            if (BlackPosi2 == true)
            {
                Destroy(BlackBuffClone[2]);
                Destroy(BlackTriggerClone[2]);
            }

            //Debug.Log("j= " + j);
            Destroy(BlackBuffClone[l]);
            Destroy(BlackTriggerClone[l]);
            // Debug.Log("BLUE DESTROYED");
            BlackOn = false;
            l--;
            //  Debug.Log("j= " + j);

        }


    }

    IEnumerator waitSpawner()
    {
        yield return new WaitForSeconds(StartWait);
        while (!Stop)
        {
            
            randBuff = Random.Range(0, 3);
            //Instanciando Amarelo
            if (randBuff == 1) {
                if (i == 1)//Se Houver buff amarelo na posição 2 do array será destruido
                {
                    Destroy(BuffClone[2]);
                    Destroy(YellowTriggerClone[2]);
                }
                i++;
                BuffClone[i] = Instantiate(Buffs[randBuff], BuffReference.position, BuffReference.rotation) as GameObject;
                YellowTriggerClone[i] =  Instantiate(YellowTrigger, ReferenceTrigger.position, BuffReference.rotation) as GameObject;
                buffAtual = turn.yellow;
                YellowOn = true;
              //  Debug.Log("YELLOW TURN");
               // Debug.Log("i= " + i);
            }
            //Instanciando Azul
            if (randBuff == 0)
            {
                if (j == 1)//Se Houver buff azul na posição 2 do array será destruido
                {
                    Destroy(BlueBuffClone[2]);
                    Destroy(BlueTriggerClone[2]);
                }
                j++;
                BlueBuffClone[j] = Instantiate(Buffs[randBuff], BuffReference.position, BuffReference.rotation) as GameObject;
                BlueTriggerClone[j] = Instantiate(BlueTrigger, ReferenceTrigger.position, BuffReference.rotation) as GameObject;
                buffAtual = turn.blue;
              //  Debug.Log("BLUE TURN");
                BlueOn = true;
             //   Debug.Log("j= " + j);
            }
            if (randBuff == 2)
            {
                if (l == 1)//Se Houver buff azul na posição 2 do array será destruido
                {
                    Destroy(BlackBuffClone[2]);
                    Destroy(BlackTriggerClone[2]);
                }
                l++;
                BlackBuffClone[l] = Instantiate(Buffs[randBuff], BuffReference.position, BuffReference.rotation) as GameObject;
                BlackTriggerClone[l] = Instantiate(BlackTrigger, ReferenceTrigger.position, BuffReference.rotation) as GameObject;
                buffAtual = turn.black;
                //  Debug.Log("BLUE TURN");
                BlackOn = true;
                //   Debug.Log("j= " + j);
            }
            yield return new WaitForSeconds(SpawnWait);
        }
    }
}
