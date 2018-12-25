//Script for the Blue Ball (Blue Team)
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody))]
public class ShooterBlue : MonoBehaviour
{ 
    public GameObject WhiteBallPosi;
    public GameObject whiteBall;

    public Transform posi1;
    public Transform posi2;
    public Transform posi3;
    public Transform posi4;
    public Transform posi5;

    public Camera cameraSide;
    public Camera mainCamera;

    public enum cameraState
    {
        front, side, none
    }
    public cameraState currentCamState = cameraState.none;

    public static bool move = false;
    public static bool move2 = false;
    public bool m_Turn;
    public static bool blueBuffActive;
    private bool m_Shoot;
    private bool blueBuff;
    private bool yellowBuff;
    public static bool blackBuff;
    public static bool blackBuffAudio;

    int distanceText;
    public int distanceWhiteBall;
    private int randPosi;

    public Team m_Team;
    public ShooterRed m_Red;
    public BuffsFeedBack FeedBack;
    public B_ScoreFeedBack Score;
    public Rigidbody Rb;

    private Arrow m_Arrow;
    [Header("Physics")]
    private Rigidbody m_Rigidbody;
    public float m_MaxForce;

    [Header("Force UI")]
    public static float m_TimeToMaxForce = 2;
    public Slider m_ForceSlider;
    public Image m_ForceFillImage;
    public Color m_MinForceColor;
    public Color m_MaxForceColor;
    public AnimationCurve m_Curve;

    private float m_ForceDirection = 1;

    public enum ShooterState
    {
        None, Charging, Release, Moving, ok
    }
    public ShooterState m_CurrentState = ShooterState.None;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Arrow = GetComponent<Arrow>();
    }


    public void Start()
    {
        Rb.mass = 3000000; //Used to set the ball's mass at the start, avoiding it to move due to any collision
        m_TimeToMaxForce = 2; //Used to set the strength bar's speed at the start.
        this.tag = "BlueBall"; //Ball's tag before it stops rolling.
        m_CurrentState = ShooterState.None;
        currentCamState = cameraState.front;
        randomPosition(); // set the white ball's position at the start.
        changeWhiteBall(); // set the white ball's position at the start.
    }

    private void Update()
    {   
        if (ShooterRed.blueBuffActive == true) //if the blue buff was activated by the enemy on the last round 
        {
            m_TimeToMaxForce = 0.1f;
            ShooterRed.blueBuffActive = false;
        }
        BuffsFeedBack();

        if (YellowBuff_Trigger.YellowBuffOn==true)//if the Yellow buff was activated by this ball.
        {
            yellowBuff = true;
            this.tag = "OffBlueBall";
        }

        if (YellowBuff_Trigger.playYellow == true)
        {
            FindObjectOfType<AudioManager>().play("Buff");
            YellowBuff_Trigger.playYellow = false;
        }

        if (BlueBuff_Trigger.BlueBuffOn == true) //if the Blue buff was activated by this ball.
        {
            FindObjectOfType<AudioManager>().play("Buff");
            blueBuff = true;
            this.tag = "OffBlueBall";
            blueBuffActive = true;
            BlueBuff_Trigger.BlueBuffOn=false;
        }
        if (BlackBuff_Trigger.BlackBuffBlueOn == true || BlackBuff_Trigger.BlackBuffRedOn==true)//if the Black buff was activated by this ball.
        {
            this.tag = "OffBlueBall";
        }

        if (blackBuffAudio == true)
        {
            FindObjectOfType<AudioManager>().play("Buff");
            blackBuffAudio = false;
        }
     
        if (currentCamState == cameraState.side)
        {
            cameraSide.targetDisplay = 0;
            mainCamera.targetDisplay = 1;
        }
        if (currentCamState == cameraState.front)
        {
            mainCamera.targetDisplay = 0;
            cameraSide.targetDisplay = 1;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
       

        RotateArrow(horizontal, vertical);
        Shoot();
        calcDistance();
        calcDistanceText();
        checkScore();
        checkBalls();
        checkTurn();

    }

    public void RotateArrow(float horizontal, float vertical)
    {
        if (m_CurrentState == ShooterState.None)
        {
            transform.Rotate(vertical, horizontal, 0);
            m_Arrow.SetPosition(5, transform.position);
            m_Arrow.SetPosition(1, transform.position + transform.forward * (m_ForceSlider.value + 2));
        }
    }

    

    public void Shoot()
    {
        if (m_CurrentState == ShooterState.None && Input.GetButtonDown("Fire1"))
        {
            m_ForceSlider.value = 0.0f;
            m_ForceDirection = 1;
            m_CurrentState = ShooterState.Charging;
            StartCoroutine(ReleasesBall());
        }

        if (m_CurrentState == ShooterState.Charging && Input.GetButton("Fire1"))
        {
            m_ForceSlider.value += (Time.deltaTime / m_TimeToMaxForce) * m_ForceDirection;
            m_ForceFillImage.color = Color.Lerp(m_MinForceColor, m_MaxForceColor, m_Curve.Evaluate(m_ForceSlider.value));
            if (m_ForceSlider.value >= 1.0f || m_ForceSlider.value <= 0.0f)
            {
                m_ForceDirection *= -1;
            }
        }

        if (m_CurrentState == ShooterState.Charging && Input.GetButtonUp("Fire1"))
        {
            Rb.mass = 0.1f;
            move=true;
            FindObjectOfType<AudioManager>().play("ThrowBall");
            float force = m_ForceSlider.value * m_MaxForce;
            m_Rigidbody.AddForce(transform.forward * force);
            m_CurrentState = ShooterState.Moving;
            currentCamState = cameraState.side;
            StartCoroutine(CheckStop());
        }
    }

    IEnumerator ReleasesBall()
    {
        yield return new WaitForSeconds(6);
        if (move == false)
        {
            Rb.mass = 0.1f;
            FindObjectOfType<AudioManager>().play("ThrowBall");
            float force = m_ForceSlider.value * m_MaxForce;
            m_Rigidbody.AddForce(transform.forward * force);
            currentCamState = cameraState.side;
            m_CurrentState = ShooterState.Moving;
            StartCoroutine(CheckStop());
        }
        move = false;
    }

    public void randomPosition()
    {
        randPosi = Random.Range(0, 6);
    }

    private void changeWhiteBall()
    {
        if (randPosi == 0)
        {
            if (BlackBuff_Trigger.BlackTriggered == true || YellowBuff_Trigger.YellowTriggered == true || 
                BlueBuff_Trigger.BlueTriggered == true)
            {
                WhiteBallPosi.transform.position = posi1.position;
                WhiteBallPosi.transform.rotation = posi1.rotation;
                BlackBuff_Trigger.BlackTriggered = false;
                YellowBuff_Trigger.YellowTriggered = false;
                BlueBuff_Trigger.BlueTriggered = false;

            }
            Debug.Log("Random Posi: " + randPosi);
                
        }
        if (randPosi == 1)
        {
            if (BlackBuff_Trigger.BlackTriggered == true || YellowBuff_Trigger.YellowTriggered == true ||
                BlueBuff_Trigger.BlueTriggered == true)
            {
                WhiteBallPosi.transform.position = posi2.position;
                WhiteBallPosi.transform.rotation = posi2.rotation;
                BlackBuff_Trigger.BlackTriggered = false;
                YellowBuff_Trigger.YellowTriggered = false;
                BlueBuff_Trigger.BlueTriggered = false;
            }
            Debug.Log("Random Posi: " + randPosi);
        }
        if (randPosi == 2)
        {
            if (BlackBuff_Trigger.BlackTriggered == true || YellowBuff_Trigger.YellowTriggered == true ||
                 BlueBuff_Trigger.BlueTriggered == true)
            {
                WhiteBallPosi.transform.position = posi3.position;
                WhiteBallPosi.transform.rotation = posi3.rotation;
                BlackBuff_Trigger.BlackTriggered = false;
                YellowBuff_Trigger.YellowTriggered = false;
                BlueBuff_Trigger.BlueTriggered = false;
            }
            Debug.Log("Random Posi: " + randPosi);
        }
        if (randPosi == 3)
        {
            if (BlackBuff_Trigger.BlackTriggered == true || YellowBuff_Trigger.YellowTriggered == true ||
                BlueBuff_Trigger.BlueTriggered == true)
            {
                WhiteBallPosi.transform.position = posi4.position;
                WhiteBallPosi.transform.rotation = posi4.rotation;
                BlackBuff_Trigger.BlackTriggered = false;
                YellowBuff_Trigger.YellowTriggered = false;
                BlueBuff_Trigger.BlueTriggered = false;
            }
            Debug.Log("Random Posi: " + randPosi);
        }
        if (randPosi == 4)
        {
            if (BlackBuff_Trigger.BlackTriggered == true || YellowBuff_Trigger.YellowTriggered == true ||
                BlueBuff_Trigger.BlueTriggered == true)
            {
                WhiteBallPosi.transform.position = posi5.position;
                WhiteBallPosi.transform.rotation = posi5.rotation;
                BlackBuff_Trigger.BlackTriggered = false;
                YellowBuff_Trigger.YellowTriggered = false;
                BlueBuff_Trigger.BlueTriggered = false;
            }
            Debug.Log("Random Posi: " + randPosi);
        }
    }

    public IEnumerator CheckStop()
    {
        m_Arrow.SetPosition(0, transform.position);
        m_Arrow.SetPosition(1, transform.position);

        yield return new WaitForSeconds(1.0f);

        while (m_Rigidbody.velocity.magnitude > 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }

        m_Rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(-Vector3.right);
        currentCamState = cameraState.front;
        m_CurrentState = ShooterState.ok;
        m_Shoot = true;
        this.tag = "OffBlueBall";
        StartCoroutine(FeedBack.DestroyFeedBack());

    }
    public void checkBalls()
    {
        if (m_Shoot == true)
        {
            B_Balls.b_ballsValue--;
            m_Shoot = false;
            m_Turn =true;


        }
    }

    public void checkScore()
    {
        if (m_Shoot == true)
        {
            Debug.Log("BUFF = " + YellowBuff_Trigger.YellowBuffOn);
            if (distanceWhiteBall > 20)
            {
                FindObjectOfType<AudioManager>().play("Augh");
                if(YellowBuff_Trigger.YellowBuffOn == true)
                {
                    Debug.Log("0 PONTOS * 2");
                    B_Score.b_scoreValue += 0*2;
                    B_ScoreFeedBack.b_scoreFeedBack = 0;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
                if(YellowBuff_Trigger.YellowBuffOn == false) {
                    B_Score.b_scoreValue += 0;
                    Debug.Log("0 PONTOS");
                    B_ScoreFeedBack.b_scoreFeedBack = 0;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }
            if (distanceWhiteBall >= 10 && distanceWhiteBall <= 20)
            {
                FindObjectOfType<AudioManager>().play("Augh");          
                if (YellowBuff_Trigger.YellowBuffOn == true)
                {
                    Debug.Log("4 PONTOS");
                    B_Score.b_scoreValue += 2*2;
                    B_ScoreFeedBack.b_scoreFeedBack = 4;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
                if (YellowBuff_Trigger.YellowBuffOn == false) {
                    B_Score.b_scoreValue += 2;
                    Debug.Log("2 PONTOS");
                    B_ScoreFeedBack.b_scoreFeedBack = 2;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }

            if (distanceWhiteBall <= 9 && distanceWhiteBall >= 6)
            {
                FindObjectOfType<AudioManager>().play("Augh");
                if (YellowBuff_Trigger.YellowBuffOn == true)
                {
                    Debug.Log("10 PONTOS");
                    B_Score.b_scoreValue += 5 * 2;
                    B_ScoreFeedBack.b_scoreFeedBack = 10;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
                if (YellowBuff_Trigger.YellowBuffOn == false) {
                    B_Score.b_scoreValue += 5;
                    Debug.Log("5 PONTOS");
                    B_ScoreFeedBack.b_scoreFeedBack = 5;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }
            if (distanceWhiteBall <= 5 && distanceWhiteBall >= 3)
            {
                FindObjectOfType<AudioManager>().play("Cool");
                
                if (YellowBuff_Trigger.YellowBuffOn == true)
                {
                    Debug.Log("20 PONTOS");
                    B_Score.b_scoreValue += 10*2;
                    B_ScoreFeedBack.b_scoreFeedBack = 20;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
                if (YellowBuff_Trigger.YellowBuffOn == false)
                { B_Score.b_scoreValue += 10;
                    Debug.Log("10 PONTOS");
                    B_ScoreFeedBack.b_scoreFeedBack = 10;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }
            if (distanceWhiteBall <= 2)
            {
                FindObjectOfType<AudioManager>().play("Cool");
              
                if (YellowBuff_Trigger.YellowBuffOn == true)
                {
                    B_Score.b_scoreValue += 20*2;
                    Debug.Log("40 PONTOS");
                    B_ScoreFeedBack.b_scoreFeedBack = 40;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
                if (YellowBuff_Trigger.YellowBuffOn == false) {
                    B_Score.b_scoreValue += 20;
                    Debug.Log("20 PONTOS");
                    B_ScoreFeedBack.b_scoreFeedBack = 20;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }
            YellowBuff_Trigger.YellowBuffOn = false;

            Debug.Log("BUFF= " + YellowBuff_Trigger.YellowBuffOn);
        }
    }
    public void calcDistanceText()
    {
            float distan = ((this.transform.position.x - whiteBall.transform.position.x) * (this.transform.position.x - whiteBall.transform.position.x))
           + ((this.transform.position.z - whiteBall.transform.position.z) * (this.transform.position.z - whiteBall.transform.position.z));

            distanceText = (int)Mathf.Sqrt(distan);
            DistanceText.distance = distanceText;


    }

    public void calcDistance()
    {
        if (m_Shoot == true)
        {
            float distan = ((this.transform.position.x - whiteBall.transform.position.x) * (this.transform.position.x - whiteBall.transform.position.x))
           + ((this.transform.position.z - whiteBall.transform.position.z) * (this.transform.position.z - whiteBall.transform.position.z));

            distanceWhiteBall = (int)Mathf.Sqrt(distan);
        }

    }



    public IEnumerator DestroyText()
    {
        yield return new WaitForSeconds(1);
        Score.ChangeScoreBack();
    }

    private void BuffsFeedBack()
    {
        if (yellowBuff == true)
        {
            FeedBack.YellowBuff();
            yellowBuff = false;
        }
        if (blueBuff==true)
        {
            FeedBack.BlueBuff();
            blueBuff = false;
        }
        if (blackBuff==true)
        {
            FeedBack.BlackBuff();
            Debug.Log("ESTOU MUDANDO O TEXTO BLACK BUFF");
            blackBuff = false;
        }
    }

    public void checkTurn(){

          if (m_Turn == true)
            {
                m_Team.turnoAtual = Team.turn.red;
                m_Team.Instanciar();
                m_Turn = false;
                this.enabled = false;
            
        }

    }

}
