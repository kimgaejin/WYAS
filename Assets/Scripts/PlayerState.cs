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
    private Animator anim;
    private Collider2D coll;

    private Collider2D curObj;
    private Collider2D adjacentObj;
    private ObjectProperty curObjAct;

    // setting
    private float horizonSpeed = 0.05f;
	private float jumpPower = 80.0f;
    private float horizon;
    private float vertical;
    private bool isFacedR;
    private bool isReversed;
    private Vector3 scaleLeft = new Vector3(1, 1, 1);
    private Vector3 scaleRight = new Vector3(-1, 1, 1);


    // save
    private Vector3 fixedPoint = Vector3.zero;

    // bit switch
    private bool isFixed;
    private bool canMove;
    private bool isJumping = false;
    private bool canJump;
    private bool isInteracting;
    private bool canInteract;

    // time
    private float rotateTime = 0.5f;

    //
    Quaternion myQuat;

    // etc
    WaitForSeconds wait01 = new WaitForSeconds(0.1f);
    WaitForSeconds wait005 = new WaitForSeconds(0.05f);

    // ==[function]

    private void Awake()
    {
       
        InitializedSetting();
        InitializeBitSwitch();
        InitializeComponent();
        InitializeKeypad();
    }

    private void Update()
    {
        AdjustBalance();
        ResetVelocityX();
        Move();
        keyboardInteract();
        keyboardJump();
        
        CheckColliders();
        InteractWithJumping();
        KeepInteracting();
        
        LimitVelocityY();
    }

    private void OnCollisionStay2D(Collision2D other)
    {

        bool isGround = (other.gameObject.tag == "GROUND");
        bool isBox = (other.gameObject.tag == "BOX");

        // 점프 유효성 검사
        float yErrorValue = .5f;

        if (isGround || isBox)
        {
            SpriteRenderer otherSpr = other.gameObject.GetComponent<SpriteRenderer>();
            if (otherSpr == null)
            {
                Debug.Log("ERROR: Hasn't SpriteRenderer***********************");
                return;
            }

            
            float otherSizeX = other.collider.bounds.size.x;
            float otherSizeY = other.collider.bounds.size.y;

            // 원점부터 모서리까지의 기본거리.
            float diagonalLength = Mathf.Sqrt(otherSizeX * otherSizeX + otherSizeY * otherSizeY) / 2.0f ;
            // 사각형의 모서리까지 기본 각도. 45도
            float theta = 45.0f * Mathf.PI / 180.0f;
            // 오브젝트가 가지고있는 rotation.z 값에 따른 각도.
            float angle = other.gameObject.GetComponent<Transform>().rotation.eulerAngles.z * Mathf.PI / 180.0f;
            // 기본 각과 오브젝트 각 합산
            float calcAngle = theta + angle;
            if (calcAngle >= Mathf.PI / 2.0f) calcAngle %= Mathf.PI / 2.0f;


            bool isOneSidePlatform = true; //= angle <= 22.5f * Mathf.PI / 180.0f || 66.5f * Mathf.PI / 180.0f <= angle;
            // 올라갈 수 있는 면이 하나. (사각형 꼴)
            if (isOneSidePlatform)
            {
                //Debug.Log("===================================================");
                // 극좌표계를 직교좌표계로 바꿉니다.
               
                // P1, P2는 밟는 사각 땅의 가장 위 두 모서리.
                Vector3 P1 = other.transform.position +
                            new Vector3(diagonalLength * Mathf.Cos(calcAngle)
                                        , diagonalLength * Mathf.Sin(calcAngle)
                                        , 0);
                Vector3 P2 = other.transform.position +
                            new Vector3(diagonalLength * Mathf.Cos(calcAngle + (90.0f * Mathf.PI / 180.0f))
                                        , diagonalLength * Mathf.Sin(calcAngle + (90.0f * Mathf.PI / 180.0f))
                                        , 0);

                // 플레이어의 발가락 좌표
                float pSizeX = spr.size.x;
                float pSizeY = spr.size.y;
                float pRotationZ = this.transform.rotation.eulerAngles.z * Mathf.PI / 180.0f;
                //if (pRotationZ > 180) pRotationZ -= 360;
                Vector3 playerFoot = new Vector3( this.transform.position.x + (Mathf.Sin(pRotationZ) * pSizeY/2.0f)
                                                 ,this.transform.position.y - (Mathf.Cos(pRotationZ) * pSizeY/2.0f)
                                                 ,0);
                Vector3 playerVec = new Vector3( (Mathf.Sin(pRotationZ * Mathf.PI / 180.0f) * pSizeY / 2.0f)
                                                 ,-Mathf.Abs((Mathf.Cos(pRotationZ * Mathf.PI / 180.0f) * pSizeY / 2.0f))
                                                 , 0);
                // 플레이어가 X축 기준으로 들어와 있는가.
                bool isOnGroundX = P2.x <= playerFoot.x && playerFoot.x <= P1.x;

                // 플레이어가 Y축 기준으로 들어와 있는가.
                float gradient = (P1.y - P2.y) / (P1.x - P2.x);
                // 예외처리; P1과 P2는 언제나 y값이 높은 두 모서리이기 때문에 gradient가 0이 나올일은 없음.
                float surfaceY = P2.y + gradient * (playerFoot.x - P2.x);

                bool isOnGroundY = (surfaceY <= playerFoot.y && playerFoot.y <= surfaceY + yErrorValue);
                
                // 디버깅용 ㅠ
                //Debug.Log("[theta + angle] : " + theta + "  "+ angle);
                //Debug.Log("[difference] : " + (playerFoot.y-surfaceY));
                //Debug.Log("[P1.y , P2.y]: ( " + P1.y + " , " + P2.y + " )");
                //Debug.Log("[P1.x ] : " + P1.x + "  [P2.x ] : " + P2.x);
                //Debug.Log("[gradient] : " + gradient);
                //Debug.Log("[surfaceY] : " + surfaceY + "  [playerFoot] : " + playerFoot);
                //Debug.Log("[pRotationZ] : " + pRotationZ );
                //Debug.Log("[sin cos0] : " + Mathf.Sin(pRotationZ * Mathf.PI / 180.0f) +" "+ Mathf.Cos(pRotationZ) * Mathf.PI / 180.0f);
                //Debug.Log("[playerVec] : " + playerVec );
               // Debug.Log("On X " + isOnGroundX + "/ On Y " + isOnGroundY);

                if (isOnGroundX 
                 && isOnGroundY 
                 && rigid.velocity.y <= 0.05f)
                {
                    //Debug.Log("Clear");
                    isJumping = false;
                    anim.SetBool("isFlying", false);
                }
            }

        }
        
    }


    // =====================[이 스크립트에서 참조용 함수]

    private void Move()
    {
        if (canMove == false) return;

        // 키보드 전용

        float keyboardHorizon = 0;
        float keyboardVertical = 0;

        if (Input.GetKey(KeyCode.A)) keyboardHorizon = -1.0f;
        else if (Input.GetKey(KeyCode.D)) keyboardHorizon = 1.0f;
        else keyboardHorizon = 0;
        
        if (Input.GetKey(KeyCode.W)) keyboardVertical = 1.0f;
        else if (Input.GetKey(KeyCode.S)) keyboardVertical = -1.0f;
        else keyboardVertical = 0;
        

        // 조이스틱 전용
        horizon = joystick.GetHorizontalValue();
        vertical = joystick.GetVerticalValue();

        transform.Translate(Vector3.right * horizonSpeed * keyboardHorizon * Time.deltaTime);
        transform.Translate(Vector3.right * horizonSpeed * horizon * Time.deltaTime);

        if (horizon > 0 || keyboardHorizon > 0)
        {
            isFacedR = true;
            if (!isInteracting) transform.transform.localScale = scaleRight;
            anim.SetBool("isWalking", true);
        }
        else if (horizon < 0 || keyboardHorizon < 0)
        {
            isFacedR = false;
            if (!isInteracting) transform.transform.localScale = scaleLeft;
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    
    // 2018-11-24 애니메이션 넣기 이전 점프입니다.
    // 애니메이션을 추가한 이후로 점프에 선 딜레이가 존재해야 하기에 코루틴으로 수정했습니다.
    // 점프에서 도움닫기 애니메이션을 없애면서 다시 이 함수로 수정했습니다.
    public void Jump()
    {
        if (isJumping == true) return;
        if (canJump == false) return;

        Vector2 arrow;
        if (isReversed == false) arrow= Vector2.up;
        else arrow = Vector2.down;

        rigid.AddForce(arrow * jumpPower, ForceMode2D.Impulse);
        isJumping = true;
        anim.SetTrigger("doJumping");
        anim.SetBool("isFlying", true);

    }

    private void keyboardJump()
    {

        if (Input.GetKeyDown(KeyCode.J))
        {
            Jump();
            //StartCoroutine("CorJump");
        }
    }

    /*
     * 점프 함수를 코루틴으로 수정했으나, 도움닫기가 사라졌기에 즉발형식으로 바꾸면서 그냥 함수로 롤백했습니다.
    public void Jump()
    {
        StartCoroutine("CorJump");
    }

    IEnumerator CorJump()
    {
        float jumpFrontDelay = 0.5f;    // 점프의 선 딜레이
        WaitForSeconds wait01 = new WaitForSeconds(0.1f);


        while (true)
        {
            if (isJumping == true) yield break;
            if (canJump == false) yield break;

            anim.SetTrigger("doJumping");
            anim.SetBool("isFlying", true);
            anim.SetBool("actionPreJump", true);

            for (int i = 0; i < jumpFrontDelay * 10; i++)
            {
                yield return wait01;
            }

            anim.SetBool("actionPreJump", false);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJumping = true;
            
        }
    }
    */

    private void CheckColliders()
    {
        int layerMask = (1 << LayerMask.NameToLayer("OBJECT_1ST"))
                        | (1 << LayerMask.NameToLayer("OBJECT_2ST"))
                        | (1 << LayerMask.NameToLayer("OBJECT_4ST")); // OBJECT_3ST인 밧줄, 사다리는 점프할때만 interact하므로 제외.

        // 이미 상호작용중인 오브젝트가 있다면, 리턴
        if (curObj) return;

        // NOT PressurePlate
        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(2.0f, 10.0f), 0.0f, layerMask, 0);
        adjacentObj = null;
        foreach (Collider2D col in colls)
        {
            try
            {
                ObjectProperty objState = null;
                objState = col.GetComponent<ObjectProperty>();
                if (objState == null) Debug.Log(col.gameObject.name + " has not objState!!!!!");


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
            catch
            {
                Debug.Log("catch in platerState");
            }

        }
    }

    private void KeepInteracting()
    {
        if (isInteracting == true)
        {
            if (curObj == null) return;

            if (curObjAct.GetInterctingState() == false)
            {
                curObjAct.StopInteracting();
                curObj = null;
                curObjAct = null;
                isInteracting = false;
            }
            else
            {
                curObjAct.IsInteracting();
            }
        }
    }

    private void keyboardInteract()
    {
        if (Input.GetKeyDown(KeyCode.K))
            Interact();
    }

    public void Interact()
    {
        // 이미 연결중인 상호작용 물체가 있는경우
        if (curObj)
        {
            // ObjectProperty가 넣어주기
            if (curObjAct == null) curObjAct = curObj.GetComponent<ObjectProperty>();

            isInteracting = false;
            curObjAct.StopInteracting();
            curObj = null;
            curObjAct = null;

            isInteracting = false;
        }
        // 연결중인 상호작용 물체가 없던경우
        else
        {
            // 인접한 오브젝트가 있는경우에만 플레이어와 링크해준다.
            if (adjacentObj)
            {
                curObj = adjacentObj;
                curObjAct = curObj.GetComponent<ObjectProperty>();
                isInteracting = true;

                curObjAct.DoInteracting();
            }
        }
        
    }

    private void InteractWithJumping()
    {
        /* CheckColliders() + Interact() 의 기능을 하고있지만
         * 사다리와 밧줄같은, 점프해야만 상호작용 할 수 있는 물체들(Layer:OBJECT_3ST)에게 적용 됨.
         * 점프중이며, 범위내에 있는 밧줄 혹은 사다리에게 바로 실행 됨.
         * + 위 아래키로도 가능하게 바뀜, 단 조이스틱은 적용 안됐음.
         */

        float keyboardVertical = 0;

        if (Input.GetKey(KeyCode.W)) keyboardVertical = 1.0f;
        else if (Input.GetKey(KeyCode.S)) keyboardVertical = -1.0f;
        else keyboardVertical = 0;

        vertical = joystick.GetVerticalValue();

        if (curObj == null)
        {
            if (isJumping == true 
                || keyboardVertical != 0
                || vertical != 0)
            {
                int layerMask = (1 << LayerMask.NameToLayer("OBJECT_3ST")); // 밧줄과 사다리
               // int layerMask = (1 << 17 );
                Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.0f, Mathf.Infinity), 0.0f, layerMask, 0);
               
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

                if (adjacentCol)
                {
                    curObj = adjacentCol;
                    curObjAct = curObj.GetComponent<ObjectProperty>();
                    isInteracting = true;

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

    private void AdjustBalance()
    {
        if (transform.rotation.eulerAngles.z >= 30 && transform.rotation.eulerAngles.z <= 180)
        {
           // transform.Rotate(new Vector3(0, 0, -10), Space.Self);
           transform.Rotate(new Vector3(0, 0, -5), Space.World);
        }
        if (transform.rotation.eulerAngles.z >= 45 && transform.rotation.eulerAngles.z <= 180)
        {
            // transform.Rotate(new Vector3(0, 0, -20), Space.Self);
            transform.Rotate(new Vector3(0, 0, -20), Space.World);
        }

        if (transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z <= 360 - 30)
        {
            // transform.Rotate(new Vector3(0, 0, 10), Space.Self);
            transform.Rotate(new Vector3(0, 0, 5), Space.World);
        }
        if (transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z <= 360 - 45)
        {
            //  transform.Rotate(new Vector3(0, 0, 20), Space.Self);
            transform.Rotate(new Vector3(0, 0, 20), Space.World);
        }
    }

    // 경사로에서 미끄러지는 걸 막음 && 절벽에서 미끄러지는 속도를 줄임.
    private void ResetVelocityX()
    {
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }

    private void RotatePlayerGPH(bool reverseType)
    {
        if (reverseType == true)
        {
            if (isReversed == false)
            {
               // transform.localRotation = Quaternion.Euler(0, 180, 180);
                // -> 이동도 바뀜
                Debug.Log("rever1");
                //  StartCoroutine("RotateAnimation");
               // spr.flipY = true;
               // spr.flipX = false;
            }

        }
        else
        {
            if (isReversed == true)
            {
                Debug.Log("rever2");
                //transform.localRotation = Quaternion.Euler(0, 0, 0);
                // StartCoroutine("RotateAnimation");
                //spr.flipY = false;
               // spr.flipX = true;
            }
        }
    }

    //----------------------------- initializing

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
        // pGraphic 이엿음
        spr = this.transform.Find("playerGHP").GetComponent<SpriteRenderer>();
        anim = this.transform.Find("playerGHP").GetComponent<Animator>();
        coll = this.GetComponent<Collider2D>();
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

    IEnumerator RotateAnimation()
    {
        float deg = 180 / (rotateTime * 20);
        for (int i = 0; i < rotateTime * 20; i++)
        {
            transform.Rotate(0,0,deg);
            yield return wait005;
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
        RotatePlayerGPH(type);
        isReversed = type;
    }

    public void SetPlayerAmimationBool(string setting, bool value)
    {
        anim.SetBool(setting, value);
    }

    public float GetHorizon()
    {
        return joystick.GetHorizontalValue();
    }

    public float GetVertical()
    {
        return joystick.GetVerticalValue();
    }

}
