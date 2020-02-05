using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour
{
    #region Declarations
    private bool swipeUp;
    private Vector2 startTouch, swipeDelta;
    private bool isDragging;
    private Rigidbody rb;
    private Transform ts;
    private Vector3 playerpos;
    private int score;
    private int increment;

    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeUp { get { return swipeUp; } }
    public GameObject player;
    public Text scoreText;
    public Transform target;
    public Text winText;
    public float upforce, frontforce;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ts = GetComponent<Transform>();
        totalReset();
    }

    private void Update()
    {
        swipeUp = false;
        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startTouch = Input.mousePosition;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
        #endregion

        #region Mobile Inputs
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isDragging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
        #endregion

        # region calculate distance
        swipeDelta = Vector2.zero;
        if (isDragging)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }
        #endregion

        #region deadzone crossed?
        if (swipeDelta.magnitude > 100)
        {
            //which direction?
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) < Mathf.Abs(y))
            {
                if (y > 0)
                {
                    swipeUp = true;
                    rb.useGravity = true;
                    rb.AddRelativeForce(0, y * upforce, y * frontforce);
                }
            }
        }
        #endregion

        #region If Loops
        if (score >= 10)
        {
            winText.enabled = true;
        }

        if (ts.position.y < -15 || ts.position.y > 30)
        {
            Reset();
        }

        #endregion

        scoreText.text = "Score: " + score.ToString();
    } 

    private void OnTriggerEnter(Collider other){
        if (other.tag == "target") {
            score += increment;
            StartCoroutine("Fall");
        } else if (other.tag == "court") {
            StartCoroutine("Fall");
            Reset();
        }
    }

    public void totalReset()
    {        
        player.transform.position = playerpos;
        player.SetActive(true);
        rb.angularVelocity = rb.velocity = new Vector3(0, 0, 0);
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
        rb.useGravity = false;
        player.transform.LookAt(target);
        score = 0;
        winText.enabled = false;
    }

    private void Reset()
    {
        player.transform.localPosition = playerpos;
        player.SetActive(true);
        rb.angularVelocity = rb.velocity = new Vector3(0, 0, 0);
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
        rb.useGravity = false;
        player.transform.LookAt(target);
        winText.enabled = false;
    }

    public void setDistance(float dist) {
        playerpos = new Vector3(0f, 0.07f, dist);
        Reset();
    }

    public void SetScore(int p) {
        increment = p;
    }

    IEnumerator Fall () {
        yield return new WaitForSeconds(0.75f);
        Reset();
    }
}
