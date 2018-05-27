using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    public float limitVel = 9.8f;

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
    private bool isReversed;

    // save
    private Vector3 fixedPoint = Vector3.zero;

    // bit switch
    private bool isFixed;
    private bool canMove;
    private bool isJumping;
    private bool canJump;
    private bool isInteracting;
    private bool canInteract;

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
        InteractWithJumping();
        LimitVelocityY();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        float otherSizeX = other.bounds.size.x;
        float otherSizeY = other.bounds.size.y;
        float distanceX = this.transform.position.x - other.transform.position.x;
        float distanceY = this.transform.position.y - other.transform.position.y;

        distanceX = Mathf.Abs(distanceX);
        if ((isReversed == false && (distanceX < otherSizeX/2 + spr.bounds.size.x/2 && distanceY >= otherSizeY/2))
            || (isReversed == true  && (distanceX < otherSizeX / 2 + spr.bounds.size.x / 2 && distanceY <= otherSizeY / 2))    )
        {
            if (Mathf.Abs(rigid.velocity.y)<= 0.05f)
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

    public void makeIsJumping(bool flag)
    {
        isJumping = flag;
    }

    public void makeHorizonspeed(float in_horizonSpeed = 0.05f)
    {
        horizonSpeed = in_horizonSpeed;
    }

    public void makeResetSpeed()
    {
        rigid.velocity = Vector2.zero;
    }

    public bool GetIsJumping()
    {
        return isJumping;
    }

    public bool GetIsFacedR()
    {
        return isFacedR;
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

    public bool GetIsReversed()
    {
        return isReversed;
    }

    public void MakeIsReversed(bool type)
    {
        isReversed = type;
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

        //Debug.Log("isJ:" + isJumping + " rev:" + isReversed);

        if (Input.GetKey(KeyCode.J))
        {
            Vector2 arrow;
            if (isReversed == false) arrow= Vector2.up;
            else arrow = Vector2.down;

            Debug.Log("호잇짜다!!");
            rigid.AddForce(arrow * jumpPower, ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    private void CheckColliders()
    {
        int layerMask = (1 << LayerMask.NameToLayer("OBJECT_1ST"))
                        | (1 << LayerMask.NameToLayer("OBJECT_2ST"))
                        | (1 << LayerMask.NameToLayer("OBJECT_4ST")); // OBJECT_3ST인 밧줄, 사다리는 점프할때만 interact하므로 제외.
        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(2.0f, 10.0f), 0.0f, layerMask, 0);
        adjacentObj = null;
        foreach (Collider2D col in colls)
        {
            ObjectProperty objState = null;
            objState = col.GetComponent<ObjectProperty>();

            Vector3 colPoint = col.transform.position;

            if (objState.GetIsInRange() == true)
            {
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

    private void Interact()
    {
        // 이미 curObj가 연결되어있는 경우, 상호작용 실행
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

        // 상호작용 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.K)) {
            // curObj가 연결되어있다면, 상호작용 실행, 아니라면 curObj로 설정.
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

    private void InteractWithJumping()
    {
        /* CheckColliders() + Interact() 의 기능을 하고있지만
         * 사다리와 밧줄같은, 점프해야만 상호작용 할 수 있는 물체들(Layer:OBJECT_3ST)에게 적용 됨.
         * 점프중이며, 범위내에 있는 밧줄 혹은 사다리에게 바로 실행 됨.
         */

        if (curObj == null)
        {
            if (isJumping == true)
            {
                int layerMask = (1 << LayerMask.NameToLayer("OBJECT_3ST")); // 밧줄과 사다리
                Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.0f, 20.0f), 0.0f, layerMask, 0);
               
                Collider2D adjacentCol = null;
                foreach (Collider2D col in colls)
                {
                    ObjectProperty colState = col.GetComponent<ObjectProperty>();
                    if (colState.GetIsInRange() == true)
                    {
                        if (adjacentCol != null)
                        {
                            if (Mathf.Abs(transform.position.x - adjacentCol.transform.position.x)
                                < Mathf.Abs(transform.position.x - col.transform.position.x))
                            {
                                adjacentCol = col;
                            }
                        }
                        else
                        {
                            adjacentCol = col;
                        }
                    }
                }

                if (adjacentCol != null)
                {
                    curObj = adjacentCol;
                    curObjAct = adjacentCol.GetComponent<ObjectProperty>();
                    curObjAct.DoInteracting();
                }

            }
        }
    }

    private void LimitVelocityY()
    {
        if (Mathf.Abs(rigid.velocity.y) > limitVel)
        {
            if (rigid.velocity.y > 0) rigid.AddForce(Physics2D.gravity * 2, ForceMode2D.Force);
            else rigid.AddForce(-Physics2D.gravity * 2, ForceMode2D.Force);
        }
    }

    private void InitializedSetting()
    {
        horizonSpeed = 2.00f;
        jumpPower = 5.0f;
        horizon = 0;
        vertical = 0;
        isFacedR = true;
        isReversed = false;
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
        spr = this.transform.Find("pGraphic").GetComponent<SpriteRenderer>();
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

        joystick = keypadCanvas.transform.Find("Joystick").GetComponent<Joystick>();
        if (joystick == null) Debug.Log("error: can't not find joystick at player");
    }
    
}
