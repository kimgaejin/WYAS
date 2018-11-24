using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : ObjectProperty {

    private Rigidbody2D rigid;

    // setting
    private float horizonSpeedSave;
    private float decelration;
    private float distantErrorValue;

    private Vector3 distant;    // 플레이어-박스 간 x축의 이상적 차이
    private Vector3 heightDifference;
    private Vector3 collPosition;

    private void Start()
    {
        // 왜 scale이 3일때 transform.getChild(0).GetSpriteRenderer.bounds.size.x -> 4.15가 나올까?
        base.rangeX = .1f+ (1.0f * transform.localScale.x);
        base.mustFaced = true;

        decelration = 0.5f;
        distantErrorValue = 1.075f;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        AdjustBalance();
        ResetVelocityX();
    }

    override public void DoInteracting()
    {
        pState.makeJump(false);
        pState.makeMove(true);
        rigid.mass = 1;

        player = GameObject.Find("Player").GetComponent<Transform>();
        distant = new Vector3( ( base.GetSize().x + pState.GetSizeX() ) / 2.0f, 0, 0);

       // Debug.Log("distant.x  "+ ( base.GetSize().x + pState.GetSizeX() ) / 2.0f);
        bool isPlayerLocatedBoxsRight = player.position.x >= transform.position.x;
        bool isPlayerReversed = pState.GetIsReversed();

        heightDifference = new Vector3(0, pState.transform.position.y - transform.position.y);

       // Debug.Log("distant: " + distant);
        if (isPlayerLocatedBoxsRight)
        {
            // player.position = transform.position + distant * distantErrorValue + heightDifference;
            player.position = transform.position + distant + heightDifference;
        }
        else
        {
            //player.position = transform.position - distant * distantErrorValue + heightDifference;
            player.position = transform.position - distant + heightDifference;
        }
        
        interactingState = true;

        horizonSpeedSave = pState.GetHorizonSpeed();
        pState.makeHorizonspeed(horizonSpeedSave * decelration);
        pState.SetPlayerAmimationBool("isPulling", true);

    }

    override public void IsInteracting()
    {
        // if 멀면 리턴
        // else 아니면 이동
        Debug.Log("isInteracting");
        bool isCloseWithPlayerX = Mathf.Abs(player.transform.position.x- this.transform.position.x) - ((pState.GetSizeX()+base.GetSize().x)/2.0f)< .2;
        if (!isCloseWithPlayerX)
        {
            Debug.Log("상자와의 X축 거리가 너무 멉니다.");
            StopInteracting();
            return;
        }

        bool isCloseWithPlayerY = Mathf.Abs(player.transform.position.y - this.transform.position.y)  < .5;
        //Debug.Log("Y차이 :"+Mathf.Abs(player.transform.position.y - this.transform.position.y));
        if (!isCloseWithPlayerY)
        {
            Debug.Log("상자와의 Y축 거리가 너무 멉니다.");
            StopInteracting();
            return;
        }

        if (pState.GetHorizon() > 0 || Input.GetKey(KeyCode.D))
            rigid.transform.Translate(Vector2.right * horizonSpeedSave * decelration * Time.deltaTime);

        if (pState.GetHorizon() < 0 || Input.GetKey(KeyCode.A))
            rigid.transform.Translate(Vector2.left * horizonSpeedSave * decelration * Time.deltaTime);
            
    }

    override public void StopInteracting()
    {
        pState.makeJump(true);
        pState.makeMove(true);
        pState.makeHorizonspeed(horizonSpeedSave);
        interactingState = false;
        pState.SetPlayerAmimationBool("isPulling", false);
        rigid.mass = 100;
    }

    override public void SetSize()
    {
        try
        {
            size = transform.localScale;
        }
        catch
        {
            //Debug.Log(transform.name.ToString() + "의 자식의 SpriteRenderer를 ObjectProperty에서 찾지 못함.");
            size = Vector2.zero;
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
} 
