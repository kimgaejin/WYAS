using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    // references of objects and scripts and components
    private GameObject keypadCanvas;
    private Joystick joystick;
    private Rigidbody2D rigid;
    private SpriteRenderer spr;

    // setting
    private float horizonSpeed = 0.05f;
	private float jumpPower = 5.0f;
    private float horizon;
    private float vertical;

    // save
    private Vector3 fixedPoint = Vector3.zero;

    // bit switch
    private bool isFixed;
    private bool canMove;
    private bool isJumping;
    private bool canJump;
    private bool isInteracting;
    private bool canInteract;


    void Awake()
    {
        InitializeBitSwitch();
        InitializeComponent();
        InitializeKeypad();
    }
    
    void Update()
    {
        Move();
        Jump();
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            float otherSizeX = other.bounds.size.x;
            float otherSizeY = other.bounds.size.y;
            float distanceX = this.transform.position.x - other.transform.position.x;
            float distanceY = this.transform.position.y - other.transform.position.y;

            distanceX = Mathf.Abs(distanceX);

            if (distanceX < otherSizeX/2 + spr.bounds.size.x/2 && distanceY >= otherSizeY/2)
            {
                if (rigid.velocity.y <= 0)
                {
                    isJumping = false;
                }
            }
        }
    }

    private void Move()
    {
        if (isFixed == true) {
            rigid.velocity = Vector2.zero;
            rigid.position = fixedPoint;
            return;
        }
        if (canMove == false) return;

        //horizon = joystick.GetHorizontalValue();  // 조이스틱용
        //vertical = joystick.VerticaltalValue();  // 조이스틱용
        // 키보드용

        if (Input.GetKey(KeyCode.A)) horizon = -1.0f;
        else if (Input.GetKey(KeyCode.D)) horizon = 1.0f;
        else  horizon = 0;

        if (Input.GetKey(KeyCode.W)) vertical = 1.0f;
        else if (Input.GetKey(KeyCode.S)) vertical = -1.0f;
        else vertical = 0;

        this.transform.position += horizon * horizonSpeed * Vector3.right;

        fixedPoint = this.transform.position;
    }

    private void Jump()
    {
        if (isJumping == true) return;
        if (canJump == false) return;

        //bool isTouchingJump = false;
        //if (GetStateJumpImg() == true)    // 터치패널용
        // 키보드용
        if (Input.GetKey(KeyCode.J))
        {
            Debug.Log("쩜프!");
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
		
		isJumping = true;
    }

    private void Interact()
    {
        
    }

    private void InitializeBitSwitch()
    {
        isFixed = false;
        canMove = true;
        canJump = true;
        isJumping = true;
        isInteracting = false;
        canInteract = true;
    }

    private void InitializeComponent()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        spr = this.transform.FindChild("pGraphic").GetComponent<SpriteRenderer>();
    }

    private void InitializeKeypad()
    {
        horizon = 0.0f;
        vertical = 0.0f;

        joystick = null;
        keypadCanvas = null;

        keypadCanvas = GameObject.Find("KeypadCanvas");
        if (keypadCanvas == null) Debug.Log("error: can't not find keypadCanvas at player");

        joystick = keypadCanvas.transform.FindChild("Joystick").GetComponent<Joystick>();
        if (joystick == null) Debug.Log("error: can't not find joystick at player");
    }
    
}
