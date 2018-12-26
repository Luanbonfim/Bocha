//Script responsible for the Red Ball's (Red Team) behavior
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class ShooterRed : MonoBehaviour
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
    private bool m_Shoot;
    public bool m_Turn;
    public static bool blueBuffActive;
    private bool blueBuff;
    private bool yellowBuff;
    public static bool blackBuff;
    public static bool blackBuffAudio;

    public int distanceWhiteBall;
    int calc = 0;
    int distanceText;
    private int randPosi;
    public Team m_Team;
    ShooterBlue m_Blue;
    public BuffsFeedBack FeedBack;
    public R_ScoreFeedBack Score;
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
    private ShooterState m_CurrentState = ShooterState.None;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Arrow = GetComponent<Arrow>();
    }

    public void Start()
    {
        m_TimeToMaxForce = 2; //Used to set the strength bar's speed at the start.
        Rb.mass = 3000000; //Used to set the ball's mass at the start, avoiding it to move due to any collision
        m_CurrentState = ShooterState.None;
        currentCamState = cameraState.front;
        randomPosition(); // set the white ball's position at the start.
        changeWhiteBall();// set the white ball's position at the start.
    }

    private void Update()
    {
        if (ShooterBlue.blueBuffActive == true) //if the blue buff was activated by the enemy on the last round 
        {
            m_TimeToMaxForce = 0.1f;
            ShooterBlue.blueBuffActive = false;
        }
        BuffsFeedBack();
        if (YellowBuff_Trigger.YellowBuffOn==true) //if the Yellow buff was activated by this ball.
        {
            yellowBuff = true;
            this.tag = "OffRedBall";
        }

        if (YellowBuff_Trigger.playYellow == true)
        {
            FindObjectOfType<AudioManager>().play("Buff");
            YellowBuff_Trigger.playYellow = false;
        }


        if (BlueBuff_Trigger.BlueBuffOn == true)//if the Blue buff was activated by this ball.
        {
            FindObjectOfType<AudioManager>().play("Buff");
            blueBuff = true;
            this.tag = "OffRedBall";
            blueBuffActive = true;
            BlueBuff_Trigger.BlueBuffOn = false;
        }

        if (BlackBuff_Trigger.BlackBuffBlueOn == true || BlackBuff_Trigger.BlackBuffRedOn == true)//if the black buff was activated by this ball.
        {
            this.tag = "OffRedBall";
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


        if (m_Team.turnoAtual == Team.turn.none)
        {
            this.enabled = false;
            m_Blue.enabled = false;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool move = horizontal != 0 || vertical != 0;

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
            m_Arrow.SetPosition(0, transform.position);
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
            move = true;
            FindObjectOfType<AudioManager>().play("ThrowBall");
            float force = m_ForceSlider.value * m_MaxForce;
            m_Rigidbody.AddForce(transform.forward * force);
            currentCamState = cameraState.side; 
            m_CurrentState = ShooterState.Moving;
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

    private void FixedUpdate()
    {

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
        if (randPosi == 1 )
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
        m_CurrentState = ShooterState.ok;
        m_Shoot = true;
        this.tag = "OffRedBall";
        StartCoroutine(FeedBack.DestroyFeedBack());

    }
    public void checkBalls()
    {
        if (m_Shoot == true)
        {
          
            R_Balls.r_ballsValue--;
            m_Shoot = false;
            m_Turn = true;
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
                if (YellowBuff_Trigger.YellowBuffOn == true)
                {
                    R_Score.r_scoreValue += 0 * 2;
                    R_ScoreFeedBack.r_scoreFeedBack = 0;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
                if (YellowBuff_Trigger.YellowBuffOn == false)
                {
                    R_Score.r_scoreValue += 0;
                    R_ScoreFeedBack.r_scoreFeedBack = 0;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }
            if (distanceWhiteBall >= 10 && distanceWhiteBall <= 20)
            {
                FindObjectOfType<AudioManager>().play("Augh");
                if (YellowBuff_Trigger.YellowBuffOn == true)
                {
                    R_Score.r_scoreValue += 2 * 2;
                    R_ScoreFeedBack.r_scoreFeedBack = 4;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());

                }
                if (YellowBuff_Trigger.YellowBuffOn == false)
                {
                    R_Score.r_scoreValue += 2;
                    R_ScoreFeedBack.r_scoreFeedBack = 2;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }

            if (distanceWhiteBall <= 9 && distanceWhiteBall >= 6)
            {
                FindObjectOfType<AudioManager>().play("Augh");
                if (YellowBuff_Trigger.YellowBuffOn == true)
                {
                    R_Score.r_scoreValue += 5 * 2;
                    R_ScoreFeedBack.r_scoreFeedBack = 10;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
                if (YellowBuff_Trigger.YellowBuffOn == false)
                {
                    R_Score.r_scoreValue += 5;
                    R_ScoreFeedBack.r_scoreFeedBack = 5;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }
            if (distanceWhiteBall <= 5 && distanceWhiteBall >= 3)
            {
                FindObjectOfType<AudioManager>().play("Cool");

                if (YellowBuff_Trigger.YellowBuffOn == true)
                {
                    R_Score.r_scoreValue += 10 * 2;
                    R_ScoreFeedBack.r_scoreFeedBack = 20;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
                if (YellowBuff_Trigger.YellowBuffOn == false)
                {
                    R_Score.r_scoreValue += 10;
                    R_ScoreFeedBack.r_scoreFeedBack = 10;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }
            if (distanceWhiteBall <= 2)
            {
                FindObjectOfType<AudioManager>().play("Cool");

                if (YellowBuff_Trigger.YellowBuffOn == true)
                {
                    R_Score.r_scoreValue += 20 * 2;
                    R_ScoreFeedBack.r_scoreFeedBack = 40;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
                if (YellowBuff_Trigger.YellowBuffOn == false)
                {
                    R_Score.r_scoreValue += 20;
                    R_ScoreFeedBack.r_scoreFeedBack = 20;
                    Score.ChangeScore();
                    StartCoroutine(DestroyText());
                }
            }
            YellowBuff_Trigger.YellowBuffOn = false;
            Debug.Log("BUFF = " + YellowBuff_Trigger.YellowBuffOn);
        }
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
    public void calcDistanceText()
    {
        float distan = ((this.transform.position.x - whiteBall.transform.position.x) * (this.transform.position.x - whiteBall.transform.position.x))
       + ((this.transform.position.z - whiteBall.transform.position.z) * (this.transform.position.z - whiteBall.transform.position.z));

        distanceText = (int)Mathf.Sqrt(distan);
        DistanceText.distance = distanceText;


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
        if (blueBuff == true)
        {
            FeedBack.BlueBuff();
            blueBuff = false;
        }
        if (blackBuff == true)
        {
            FeedBack.BlackBuff();
            Debug.Log("ESTOU MUDANDO O TEXTO BLACK BUFF");
            blackBuff = false;
        }
    }
    public void checkTurn()
    {


        if (m_Turn == true && R_Balls.r_ballsValue != 0)
        {
            if (m_Turn == true)
            {
                m_Team.turnoAtual = Team.turn.blue;
                m_Team.Instanciar();

                m_Turn = false;
                this.enabled = false;

            }



        }
    }
}
