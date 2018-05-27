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

        player = GameObject.Find("Player").GetComponent<Transform>();
        distant = new Vector3(base.GetSize().x / 2.0f + pState.GetSizeX() / 2.0f, 0, 0);

        if (pState.GetIsReversed() == false) heightDifference = new Vector3(0, pState.GetSizeY() / 2.0f - GetSize().y / 2.0f, 0);
        else heightDifference = new Vector3(0, -(pState.GetSizeY() / 2.0f - GetSize().y) / 2.0f, 0);

        if (player.position.x >= transform.position.x)
            player.position = transform.position + distant * distantErrorValue + heightDifference;
        else
            player.position = transform.position - distant * distantErrorValue + heightDifference;

        Debug.Log("pPos:" + player.position + "  dist:" + distant + " heightDist:" + heightDifference);

        interactingState = true;

        horizonSpeedSave = pState.GetHorizonSpeed();
        pState.makeHorizonspeed(horizonSpeedSave * decelration);

    }

    override public void IsInteracting()
    {
        if (pState.GetVerticalSpeed() < -0.1f)
        {
            Debug.Log("Player y Spped < 0 -> stop : " + pState.GetVerticalSpeed());
            StopInteracting();
        }
        else if (rigid.velocity.y < -0.1f)
        {
            Debug.Log("Box y Spped < 0 -> stop");
            StopInteracting();
        }
        else
        {
            float distantX = transform.position.x - player.transform.position.x;
            if (Mathf.Abs(distantX) 
                > (GetSize().x / 2 + pState.GetSizeX() / 2 ) * distantErrorValue)
            {
                if (Input.GetKey(KeyCode.D))
                    rigid.transform.Translate(Vector2.right * horizonSpeedSave * decelration * Time.deltaTime);

                if (Input.GetKey(KeyCode.A))
                    rigid.transform.Translate(Vector2.left * horizonSpeedSave * decelration * Time.deltaTime);
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
    }
} 
