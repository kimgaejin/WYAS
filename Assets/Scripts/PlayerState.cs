﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    // references of objects and scripts and components
    private GameObject keypadCanvas;
    private Joystick joystick;
    private Rigidbody2D rigid;
    private SpriteRenderer spr;

    private Collider2D curObj;
    private Collider2D adjacentObj;
    private ObjectProperty curObjAct;

    // setting
    private float horizonSpeed = 0.05f;
	private float jumpPower = 5.0f;
    private float horizon;
    private float vertical;
    private bool isFacedR;

    private Vector3 considerObjectVector;
    private float considerObjectRange;

    // save
    private Vector3 fixedPoint = Vector3.zero;

    // bit switch
    private bool isFixed;
    private bool canMove;
    private bool isJumping;
    private bool canJump;
    private bool isInteracting;
    private bool canInteract;
    private bool onGravity;


    private void Awake()
    {
        InitializedSetting();
        InitializeBitSwitch();
        InitializeComponent();
        InitializeKeypad();
    }
    
    private void Update()
    {
        Move();
        Jump();
        CheckColliders();
        Interact();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        float otherSizeX = other.bounds.size.x;
        float otherSizeY = other.bounds.size.y;
        float distanceX = this.transform.position.x - other.transform.position.x;
        float distanceY = this.transform.position.y - other.transform.position.y;

        distanceX = Mathf.Abs(distanceX);
        if (distanceX < otherSizeX/2 + spr.bounds.size.x/2 && distanceY >= otherSizeY/2)
        {
            if (rigid.velocity.y <= 0.005)
            {
                isJumping = false;
            }
        }

    }

    // =====================[외부 스크립트에서 참조용 함수]

    public void makeMove(bool flag)
    {
        if (flag == true) canMove = true;
        else canMove = false;
    }

    public void makeJump(bool flag)
    {
        if (flag == true) canJump = true;
        else canJump = false;
    }

    public void makeHorizonspeed(float in_horizonSpeed = 0.05f)
    {
        horizonSpeed = in_horizonSpeed;
    }

    public bool GetIsJumping()
    {
        return isJumping;
    }

    public float GetSizeX()
    {
        return spr.bounds.size.x;
    }

    public float GetSizeY()
    {
        return spr.bounds.size.y;
    }

    public float GetVerticalSpeed()
    {
        return rigid.velocity.y;
    }

    public float GetHorizonSpeed()
    {
        return horizonSpeed;
    }

    // =====================[이 스크립트에서 참조용 함수]

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

        transform.Translate(Vector3.right * horizonSpeed * horizon * Time.deltaTime);

        if (horizon > 0) isFacedR = true;
        else if (horizon < 0) isFacedR = false;

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
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
		
		isJumping = true;
    }

    private void CheckColliders()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(2.0f, 2.0f), 0.0f, 1 << 11, 0);
        adjacentObj = null;
        foreach (Collider2D col in colls)
        {
            ObjectProperty objState = null;
            objState = col.GetComponent<ObjectProperty>();

            Vector3 colPoint = col.transform.position;

            // 혹시 플레이어 + 오브젝트 높이 보다 차이나는지 확인
            if (Mathf.Abs(transform.position.y - colPoint.y) < spr.bounds.size.y/2 + objState.GetSize().y/2)
            {
                // distanceX가 양수면 플레이어가 오른쪽에 있음.
                float distanceX = transform.position.x - colPoint.x;
                // 거리가 그 오브젝트의 충돌범위에 들어와있는지 확인.
                if (Mathf.Abs(distanceX) <= objState.GetRangeX()) {
                    // 보고있는 방향에 있는 오브젝트인지 확인.
                    if ( (distanceX >= 0 && isFacedR == false)
                        || (distanceX < 0 && isFacedR == true))
                    {
                        // 이미 인접한 오브젝트가 설정되어있는지 확인..
                        if (adjacentObj != null)
                        {
                            // 거리가 더 가까운지 확인.
                            if (Mathf.Abs(transform.position.x - adjacentObj.transform.position.x)
                                < Mathf.Abs(transform.position.x - col.transform.position.x))
                            {
                                adjacentObj = col;
                            }
                        }
                        // 인접한 오브젝트가 없었다면 얘가 인접한 오브젝트.
                        else
                        {
                            adjacentObj = col;
                        }
                    }
                }
            }
        }
    }

    private void Interact()
    {

        if (curObj != null)
        {
            if (curObjAct == null) curObjAct = curObj.GetComponent<ObjectProperty>();
            if (curObjAct.GetInterctingState() == false)
            {
                curObjAct.StopInteracting();
                curObj = null;
                curObjAct = null;
            }
            else
            {
                curObjAct.IsInteracting();
            }
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            if (curObj != null)
            {
                curObjAct.StopInteracting();
                curObj = null;
                curObjAct = null;
            }
            else
            {
                if (adjacentObj == null) return;
                curObj = adjacentObj;
                curObjAct = curObj.GetComponent<ObjectProperty>();
                curObjAct.DoInteracting();
            }
        }
    }

    private void InitializedSetting()
    {
        horizonSpeed = 2.00f;
        jumpPower = 5.0f;
        horizon = 0;
        vertical = 0;
        isFacedR = true;

        considerObjectVector = Vector3.zero;
        considerObjectRange = 0.0f;
}

    private void InitializeBitSwitch()
    {
        isFixed = false;
        canMove = true;
        canJump = true;
        isJumping = true;
        isInteracting = false;
        canInteract = true;
        onGravity = true;
    }

    private void InitializeComponent()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        spr = this.transform.FindChild("pGraphic").GetComponent<SpriteRenderer>();
        curObj = null;
        isFacedR = true;
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
