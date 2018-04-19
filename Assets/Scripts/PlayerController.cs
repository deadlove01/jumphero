using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float threshHoldX = 7f;
    [SerializeField] private float threshHoldY = 14f;
    [SerializeField] private float maxForceX = 6f;
    [SerializeField] private float maxForceY = 13f;
    [SerializeField] private float minForceX = 1f;
    [SerializeField] private float minForceY = 2f;
    [SerializeField] private float powerBarMaxValue = 10f;
    [SerializeField] private float powerBarChargeSpeed = 10f;

    private Rigidbody2D rg;
    private Animator anim;

    private float curForceX = 0;
    private float curForceY = 0;

    private bool isCharging = false;
    private bool isJumping = false;
    public static PlayerController Instance;

    private Slider powerBar;
    private float powerBarValue = 0f;
    private bool canJump = false;
    private void Awake()
    {
        if (!Instance)
            Instance = this;

        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        powerBar = GameObject.Find("PowerBar").GetComponent<Slider>();
        powerBar.minValue = 0;
        powerBar.maxValue = powerBarMaxValue;
        powerBar.value = powerBarValue;
        canJump = true;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!canJump || GameManager.Instance.GameOver)
	        return;
	    if (isCharging)
	    {
	        curForceX += threshHoldX * Time.deltaTime;
	        curForceY += threshHoldY * Time.deltaTime;

	        if (curForceX > maxForceX)
	            curForceX = maxForceX;
            else if (curForceX < minForceX)
	            curForceX = minForceX;
	        if (curForceY > maxForceY)
	            curForceY = maxForceY;
	        else if (curForceY < minForceY)
	            curForceY = minForceY;

            powerBarValue += powerBarChargeSpeed * Time.deltaTime;
	        powerBar.value = powerBarValue;

	    }
	    
	}

    void Jump()
    {
        canJump = false;
        rg.velocity = new Vector2(curForceX, curForceY);
        print("vel: "+rg.velocity.sqrMagnitude);
        curForceX = curForceY = 0;
        isJumping = true;
        anim.SetBool("Jump", true);

        powerBar.value = powerBarValue = 0;
    }
    public void Charging(bool value)
    {
        if (!canJump)
            return;
        isCharging = value;
        if (!isCharging)
        {
            Jump();
        }
    }



    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isJumping)
        {
            if (collider.tag == "Platform")
            {
                isJumping = false;
                anim.SetBool("Jump", isJumping);
                GameManager.Instance.CreateNewPlatform(collider.transform.position.x);
                canJump = true;
                ScoreManager.Instance.AddScore();
            }
        }
       


        if (collider.tag == "Deadend")
        {
            GameManager.Instance.EndGame();
        }
    }
}
