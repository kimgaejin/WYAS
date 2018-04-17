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
    private Vector3 fixedPoint;
    private Vector3 collPosition;

    private bool isColl = false;

    private int ONLY_RIGHT_COLLIDER = 1;
    private int NO_COLLIDER = 0;
    private int ONLY_LEFT_COLLIDER = -1;
    private int ALL_SIDE_COLLIDER = 2;

    private List<GameObject> items= new List<GameObject>();
   
    new private void Awake()
    {
        Debug.Log("Awake!");
        base.Awake();
        base.rangeX = 1.1f;
        fixedPoint = Vector3.zero;

        decelration = 0.5f;
        distantErrorValue = 1.075f;
        rigid = GetComponent<Rigidbody2D>();
    }

    override public void DoInteracting()
    {
        pState.makeJump(false);
        pState.makeMove(true);

        player = GameObject.Find("Player").GetComponent<Transform>();
        distant = new Vector3(base.size.x / 2.0f + player.FindChild("pGraphic").GetComponent<SpriteRenderer>().bounds.size.x / 2.0f, 0, 0);
        heightDifference = new Vector3(0, pState.GetSizeY() / 2.0f - GetSize().y / 2.0f, 0);

        if (player.position.x >= transform.position.x)
            player.position = transform.position + distant * distantErrorValue + heightDifference;
        else
            player.position = transform.position - distant * distantErrorValue + heightDifference;

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
        else if (rigid.velocity.y < -1f)
        {
            Debug.Log("Box y Spped < 0 -> stop");
            StopInteracting();
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
                rigid.transform.Translate(Vector2.right * horizonSpeedSave * decelration * Time.deltaTime);

            if (Input.GetKey(KeyCode.A))
                rigid.transform.Translate(Vector2.left * horizonSpeedSave * decelration * Time.deltaTime);

        }
    }

    override public void StopInteracting()
    {
        pState.makeJump(true);
        pState.makeMove(true);
        pState.makeHorizonspeed(horizonSpeedSave);
        //transform.parent = null;
        interactingState = false;
        //BackToParentObject();
    }
} 
