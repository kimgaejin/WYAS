using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : ObjectProperty {

    private Rigidbody2D rigid;

    // setting
    private float horizonSpeedSave;
    private float decelration;
    private float distantErrorValue;

    private Vector3 distant;
    private Vector3 heightDifference;
    private Vector3 collPosition;

    private void Start()
    {
        base.rangeX = 1.1f;
        base.mustFaced = true;

        decelration = 0.5f;
        distantErrorValue = 1.075f;
        rigid = GetComponent<Rigidbody2D>();
    }

    override public void DoInteracting()
    {
        pState.makeJump(false);
        pState.makeMove(true);
        rigid.mass = 1;

        player = GameObject.Find("Player").GetComponent<Transform>();
        distant = new Vector3(base.GetSize().x / 2.0f + pState.GetSizeX() / 2.0f, 0, 0);

        bool isPlayerLocatedBoxsRight = player.position.x >= transform.position.x;
        bool isPlayerReversed = pState.GetIsReversed();

        heightDifference = new Vector3(0, pState.transform.position.y - transform.position.y);

        if (isPlayerLocatedBoxsRight)
        {
            player.position = transform.position + distant * distantErrorValue + heightDifference;
        }
        else
        {
            player.position = transform.position - distant * distantErrorValue + heightDifference;
        }
        
        interactingState = true;

        horizonSpeedSave = pState.GetHorizonSpeed();
        pState.makeHorizonspeed(horizonSpeedSave * decelration);

    }

    override public void IsInteracting()
    {
        /*
        if (Mathf.Abs(pState.GetVerticalSpeed()) > 0.1f)
        {
            Debug.Log("Player y Spped < 0 -> stop : " + pState.GetVerticalSpeed());
            StopInteracting();
        }
        else if ( Mathf.Abs(rigid.velocity.y) > 0.1f)
        {
            Debug.Log("Box y Spped < 0 -> stop");
            StopInteracting();
        }
        */
        float pX = player.transform.position.x;
        float pY = player.transform.position.y;
        float bX = transform.position.x;
        float bY = transform.position.y;
        bool soFarBox = Mathf.Abs( (pX - bX)*(pX - bX)+(pY - bY)*(pY - bY)) >= 3.0f;
        if (soFarBox) {
            Debug.Log("soFarBox");
            StopInteracting();
        }
        else
        {
            Vector2 distance2d = new Vector2(pX - bX, pY - bY);
            float distantX = transform.position.x - player.transform.position.x;
            if (Mathf.Abs(distantX)
                > (GetSize().x / 2 + pState.GetSizeX() / 2) * distantErrorValue)
            {
                if (Input.GetKey(KeyCode.D))
                    rigid.AddForce(distance2d * 100, ForceMode2D.Force);
                    //rigid.transform.Translate(Vector2.right * horizonSpeedSave * decelration * Time.deltaTime);

                if (Input.GetKey(KeyCode.A))
                    rigid.AddForce(distance2d * 100, ForceMode2D.Force);
                    //rigid.transform.Translate(Vector2.left * horizonSpeedSave * decelration * Time.deltaTime);
            }
            else
            {
                if (distantX > 0)
                {
                    transform.position += -distant * (1 - distantErrorValue) / 2;
                }
                else
                {
                    transform.position += distant * (1 - distantErrorValue) / 2;
                }
            }
        }
    }

    override public void StopInteracting()
    {
        pState.makeJump(true);
        pState.makeMove(true);
        pState.makeHorizonspeed(horizonSpeedSave);
        interactingState = false;
        rigid.mass = 100;
    }
} 
